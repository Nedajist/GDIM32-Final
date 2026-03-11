using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
public enum State
{
    None,
    Accepted, 
    Ready,
    Completed
}


public class player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Camera _playercamera;
    [SerializeField] private GameObject _camera_destination;
    [SerializeField] private float _movespeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<Interactable> _playerInventory;
    [SerializeField] private float _max_upward_momentum;
    [SerializeField] private float _max_forward_momentum;
    [SerializeField] float _min_upward_momentum;
    [SerializeField] private float _min_forward_momentum;
    [SerializeField] float _upward_charge_velocity;
    [SerializeField] float _forward_charge_velocity;
    [SerializeField] float _fall_height_threshold;
    [SerializeField] AudioSource _explosion_audio_manager;
    [SerializeField] AudioSource _footsteps_audio_manager;
    [SerializeField] AudioSource _music_audio_manager;
    [SerializeField] AudioSource _landing_audio_manager;
    [SerializeField] AudioSource _charging_audio_manager;




    // VFX 
    [SerializeField] GameObject _tiny_explosion;
    [SerializeField] GameObject _small_explosion;
    [SerializeField] GameObject _large_explosion;
    [SerializeField] GameObject _heat_distortion;
    [SerializeField] GameObject _tiny_dust_blast;
    [SerializeField] GameObject _medium_dust_blast;
    [SerializeField] GameObject _large_dust_blast;
    [SerializeField] GameObject _flame_trail;
    [SerializeField] GameObject _rocket_trail;
    [SerializeField] GameObject _giant_explosion;
    [SerializeField] GameObject _sparks;



    // AUDIO
    [SerializeField] AudioClip _charging_SFX;
    [SerializeField] AudioClip _footsteps_SFX;
    [SerializeField] AudioClip _tiny_explosion_SFX;
    [SerializeField] AudioClip _small_explosion_SFX;
    [SerializeField] AudioClip _large_explosion_SFX;
    [SerializeField] AudioClip _giant_explosion_SFX;
    [SerializeField] AudioClip _tiny_landing_SFX;
    [SerializeField] AudioClip _iris_out;




    public enum _movement_states
    {
        Charging,
        Walking,
        Falling,
        Idle
    }

    public _movement_states _movement_state;

    public static player Instance { get; private set; }
    private bool _grounded = true;
    private ArrayList _list_of_colliders = new ArrayList();
    public bool _canMove = true;
    public bool _on_slope = false;
    private float _space_held_time = 0;
    private float _space_held_frames = 0;
    private Vector3 _max_upward_momentum_vector;
    private Vector3 _max_forward_momentum_vector;
    private RaycastHit _raycast_results;
    private float _starting_fall_height;
    private float _seconds_airborne;
    private float _jump_grace_period = 0;
    private int _inventory_selected_index = 0;
    private float _target_camera_FOV = 60;
    private float _default_camera_FOV = 60;
    private float _player_camera_FOV_lerp_speed = 3;
  
    public float _health = 200;
    public float _maxHealth = 200;
    public float _max_air_jump_charges = 0;
    public float _air_jump_charges = 0;


    public delegate void HandDelegate(string hand);
    public event HandDelegate HandSelected;

    public delegate void InventoryDelegate(List<Interactable> inventory);
    public event InventoryDelegate InventoryUpdated;

    public float _held_forward_momentum;
    public float _charge_percent;

    private bool bomb_ending = false;
    private float _seconds_after_ending = 0;

    //Quest variables

    public State _currentState { get; private set; }

    //UI variables


    //NPC variables
    [SerializeField] public bool _dialogueActive;
    [SerializeField] private bool _nearNPC;

    private NPC _currentNPC;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }


    void Start()
    {
        _currentState = State.None;
        _max_upward_momentum_vector = _max_upward_momentum * transform.up;
        _max_forward_momentum_vector = _max_forward_momentum * transform.forward;
        _ClearInteractable(0);
        _ClearInteractable(1);
        _flame_trail.GetComponent<FollowPlayer>().transform_additive = transform.up * 1.5f;
        _heat_distortion.GetComponent<FollowPlayer>().transform_additive = transform.up * -1.0f;
        _movement_state = _movement_states.Idle;

    }


    void Update()
    {
        if (bomb_ending)
        {
            _BombEndingProgression();
        }

        if (_playercamera.fieldOfView != _target_camera_FOV)
        {
            _playercamera.fieldOfView = Mathf.Lerp(_playercamera.fieldOfView, _target_camera_FOV, Time.deltaTime * _player_camera_FOV_lerp_speed);
        }

        _DisplayInteractable();
        _jump_grace_period += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && ( (_grounded == true || _on_slope == true) || _air_jump_charges > 0)) // checks if player is holding down space bar. Can't be in the air unless has an air jump charge
        {
            if (_movement_state != _movement_states.Charging) // Will transition to charging if at any point the player somehow leaves the charging state
            {
                _transition_movement_state(_movement_states.Charging);
            }

            if (_grounded == false && _on_slope == false) // if in air, slows player down 
            {
                _rigidbody.AddForce(new Vector3(0, 0.4f, 0) * Mathf.Abs(_rigidbody.velocity.y));
            }


            else if (_movement_state == _movement_states.Charging && _space_held_frames > 70) // freezes charging animation 
            {
                _animator.speed = 0;
            }

            if (_charging_audio_manager.pitch < 1.5) // pitch of charging sound gradually increases
            {
                _charging_audio_manager.pitch += 0.001f;
            }

            _space_held_time += Time.deltaTime;
            _space_held_frames += 1;

            _held_forward_momentum = _forward_charge_velocity * _space_held_time;
            if (_held_forward_momentum > _max_forward_momentum)
            {
                _held_forward_momentum = _max_forward_momentum;
            }

            _charge_percent = (_held_forward_momentum / (_max_forward_momentum - _min_forward_momentum));

        }



        if (Input.GetKeyUp(KeyCode.Space) && _space_held_time > 0 && ((_grounded == true || _air_jump_charges> 0))) // check if space was released, player jumps
        {
            _rigidbody.velocity = new Vector3(0, 0, 0);
            _jump_grace_period = 0;
            Vector3 _upward_momentum = (_upward_charge_velocity * transform.up * _space_held_time) + _min_upward_momentum * transform.up;
            Vector3 _forward_momentum = (_forward_charge_velocity * transform.forward * _space_held_time) + _min_forward_momentum * transform.forward;

            if (_upward_momentum.y > _max_upward_momentum_vector.y){
                _upward_momentum = _max_upward_momentum_vector;
            }

            if (_forward_momentum.z > _max_forward_momentum_vector.z){
                _forward_momentum = _max_forward_momentum_vector;
            }
            //Debug.Log("After " + _space_held_time.ToString() + " seconds, launched with a force of " + _upward_momentum.y.ToString() + " " + _forward_momentum.z.ToString());
            _rigidbody.AddForce(_upward_momentum);
            _rigidbody.AddForce(_forward_momentum);


            add_camera_shake(_charge_percent);

            if (_charge_percent < 0.20)
            {
                _explosion_audio_manager.clip = _tiny_explosion_SFX;
                Instantiate(_tiny_explosion, transform.position + transform.up*2, Quaternion.identity);
                _target_camera_FOV = _default_camera_FOV + 5;
            }
            else if (_charge_percent < 0.40)
            {
                _explosion_audio_manager.clip = _small_explosion_SFX;
                Instantiate(_small_explosion, transform.position + transform.up*2, Quaternion.identity);
                Instantiate(_heat_distortion, transform.position, Quaternion.identity);
                Instantiate(_flame_trail, transform.position + transform.up, Quaternion.identity);
                _target_camera_FOV = _default_camera_FOV + 10;
            }
            else
            {
                _explosion_audio_manager.clip = _large_explosion_SFX;
                Instantiate(_large_explosion, transform.position + transform.up*2, Quaternion.identity);
                Instantiate(_heat_distortion, transform.position, Quaternion.identity);
                Instantiate(_flame_trail, transform.position + transform.up, Quaternion.identity);
                _target_camera_FOV = _default_camera_FOV + 20;
            }
            _transition_movement_state(_movement_states.Falling);
        }
        List<bool> keylist = new List<bool> { Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.A), Input.GetKey(KeyCode.D) }; //WASD movement
        List<Vector3> normal_move_list = new List<Vector3> { transform.forward * _movespeed, transform.forward * _movespeed * -1 , transform.right * _movespeed * -1 , transform.right * _movespeed };
        List<Vector3> falling_move_list = new List<Vector3> { 0.15f * transform.forward * _movespeed, -0.15f * transform.forward * _movespeed, -0.15f * transform.right * _movespeed , 0.15f * transform.right * _movespeed };

        foreach (int i in new List<int> {0,1,2,3})
        {
            if (keylist[i] == true && _movement_state != _movement_states.Charging && _on_slope == false)
            {
                if (_grounded == true && _jump_grace_period>0.1f) // moving immediately after jumping glitches things out
                {
                    _rigidbody.velocity = normal_move_list[i];
                }
                else
                {
                    _rigidbody.AddForce(falling_move_list[i]);
                }

                if (_movement_state == _movement_states.Idle)
                {
                    _transition_movement_state(_movement_states.Walking);
                }

            }
        }


        if (Input.anyKey == false && _movement_state == _movement_states.Walking) // transitions to idle if player stops walking
        {
            _transition_movement_state(_movement_states.Idle);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                _playercamera.GetComponent<CameraController>()._frozen_rotation = transform.rotation;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray interaction_detector = _playercamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(interaction_detector, out _raycast_results, 30f)){
                if (_raycast_results.transform.CompareTag("NPC") && _dialogueActive == false){
                    _currentNPC = _raycast_results.transform.GetComponent<NPC>();
                    TalkToNPC(_currentNPC);
                    Debug.Log("talking to npc");
                }

                if (_raycast_results.transform.CompareTag("Interactable")){
                    Interactable _item = _raycast_results.transform.GetComponent<Interactable>();
                    _AddInteractable(_item);
                }

            }
        }
        
        if (Input.GetKeyDown(KeyCode.E) && _dialogueActive == false)
        {
            if (_playerInventory[_inventory_selected_index].name != "None")
            {
                Instantiate(_sparks, transform.position + transform.up * 1.9f, Quaternion.identity);
                _playerInventory[_inventory_selected_index].interact();

            }
            _ClearInteractable(_inventory_selected_index);
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) && _dialogueActive == false)
        {
            HandSelected?.Invoke("left");
            _playerInventory[1].gameObject.SetActive(false);
            _inventory_selected_index = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && _dialogueActive == false)
        {
            HandSelected?.Invoke("right");
            _playerInventory[0].gameObject.SetActive(false);
            _inventory_selected_index = 1;
        }

        bool _groundcheck1 = Physics.Raycast(transform.position, transform.up * -1, 0.1f);
        bool _groundcheck2 = Physics.Raycast(transform.position - transform.forward * -0.3f, transform.up * -1, 0.1f);
        bool _groundcheck3 = Physics.Raycast(transform.position, transform.up * -1, 1.0f); // ensures that colliding to the SIDE of a slope does not count as the player being on GROUND


        if (_groundcheck1 == false && _groundcheck2==false && _on_slope == false)
        {
            player_falls();
        }
        else if (_groundcheck1 == true || _groundcheck2 == true || ( _on_slope == true && _groundcheck3 == true)) 
        {
            player_lands();
        }

        //ChangeState(_currentState);


        if (!_canMove)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }


        _playercamera.GetComponent<CameraController>().UpdateCamera();
    }
    void player_falls() // will always set _grounded to false
    {
        if (_grounded == true) // checks if the player was on the ground the previous frame.
        {
            if (_movement_state != _movement_states.Falling) { // the player immediately transitions to falling after releasing space, no need to double transition
                _transition_movement_state(_movement_states.Falling);
            }
            
            _starting_fall_height = transform.position.y; // when the player starts to fall, sets their starting fall height. The only place where _starting_fall_height is set
        }

        _grounded = false;
        _seconds_airborne += Time.deltaTime;
    }

    void player_lands() // will alsways set _grounded to true 
    {
        if (_grounded == false) // checks if player was not on the ground the previous frame
        {
            _charge_percent = 0; // resets player momentum
            _held_forward_momentum = 0;
            _target_camera_FOV = _default_camera_FOV;
            float fall_distance = _starting_fall_height - transform.position.y;
            _transition_movement_state(_movement_states.Idle);
            Debug.Log("Fell a distance of :" + fall_distance + " to the new height of " + transform.position.y.ToString());
            if (fall_distance > _fall_height_threshold) 
            {
                GameController.Instance.UIController.losehealth((fall_distance - 5) / 2); //seconds of fall damage player will take
            }

            if (_seconds_airborne > 2.5) //shakes camera and plays dust explosion upon landing. 
            {
                add_camera_shake(0.12f);
                Instantiate(_large_dust_blast, transform.position, Quaternion.identity);
                _landing_audio_manager.pitch = 0.9f;
                _landing_audio_manager.clip = _tiny_landing_SFX;
                _landing_audio_manager.Play();
            }

            else if (_seconds_airborne > 2)
            {
                add_camera_shake(0.10f);
                Instantiate(_medium_dust_blast, transform.position, Quaternion.identity);
                _landing_audio_manager.pitch = 0.7f;
                _landing_audio_manager.clip = _tiny_landing_SFX;
                _landing_audio_manager.Play();
            }

            else if (_seconds_airborne > 1)
            {
                add_camera_shake(0.08f);
                Instantiate(_tiny_dust_blast, transform.position, Quaternion.identity);
                _landing_audio_manager.pitch = 0.5f;
                _landing_audio_manager.clip = _tiny_landing_SFX;
                _landing_audio_manager.Play();
            }
            _seconds_airborne = 0; //resets _seconds_airbone. The only place where it is reset. 
        }
        _grounded = true;
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _list_of_colliders.Remove(collision);
        }
        if (collision.transform.CompareTag("Obstacle"))
        {
            _list_of_colliders.Remove(collision);
        }
        if (collision.transform.CompareTag("Slope"))
        {
            _list_of_colliders.Remove(collision);
            _on_slope = false;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            _starting_fall_height = transform.position.y;
            _list_of_colliders.Add(collision);
            bouncy_object obstacle = collision.transform.GetComponent<bouncy_object>();
            _rigidbody.AddExplosionForce(obstacle.repel_force, transform.position, 100);

        }

        if (collision.transform.CompareTag("Slope"))
        {
            _list_of_colliders.Add(collision);
            _on_slope = true;
        }

        if (collision.transform.CompareTag("Ground"))
        {
            _list_of_colliders.Add(collision);
        }
    }
    
    private void _ClearInteractable(int index)
    {
        _playerInventory[index].name = "None";
        _playerInventory[index].type = "None";
        _playerInventory[index].GetComponent<MeshRenderer>().gameObject.SetActive(false);
        InventoryUpdated?.Invoke(_playerInventory);
    }

    private void _DisplayInteractable() // if the player is holding an item, moves it above their head 
    {
        Interactable selected_interactable = _playerInventory[_inventory_selected_index];
        if (selected_interactable.type != "None")
        {
            selected_interactable.gameObject.SetActive(true);
            selected_interactable.transform.position = transform.position + new Vector3(0, 2, 0);
            selected_interactable.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + selected_interactable.carry_vector);
        }
    }

    private void _AddInteractable(Interactable item)
    {
        foreach (int i in Enumerable.Range(0, 2))
        {
            if (_playerInventory[i].name == "None")
            {
                _playerInventory[i] = item;
                item.GetComponent<BoxCollider>().enabled = (false);
                item.gameObject.SetActive(false);
                InventoryUpdated?.Invoke(_playerInventory);
                break;
            }
        }
    }

    public void TalkToNPC(NPC npc)
    {
        
    }

    public void _transition_movement_state(_movement_states new_state)
    {
        switch (new_state)
        {
            case _movement_states.Charging:
                _space_held_time = 0;
                _footsteps_audio_manager.Stop();
                _charging_audio_manager.Play();
                _charging_audio_manager.pitch = 0.9f;

                if (_grounded == true)
                {
                    _animator.SetBool("Falling", false);
                }
                

                switch (_movement_state)
                    {
                        case _movement_states.Walking:
                            _rigidbody.velocity = new Vector3(0, 0, 0);
                            _animator.SetBool("Walking", false);
                            _animator.SetBool("Falling", false);
                            _animator.SetTrigger("Charging");
                            break;
                    }

                _animator.SetTrigger("Charging");
                _animator.speed = 0.4f;

                break;

            case _movement_states.Falling:
                _charging_audio_manager.Stop();
                _footsteps_audio_manager.Stop();
                switch (_movement_state)
                {
                    case (_movement_states.Charging):
                    {
                        _explosion_audio_manager.Play();
                        break;
                    }
                }

                if (_grounded == false && _on_slope == false)
                {
                    _air_jump_charges -= 1;
                }

                _space_held_frames = 0;
                _animator.SetBool("Falling", true);
                _animator.SetBool("Walking", false);
                _animator.speed = 1;
                break;

            case _movement_states.Walking:
                _air_jump_charges = _max_air_jump_charges;
                _footsteps_audio_manager.Play();
                _animator.SetBool("Falling", false);
                _animator.SetBool("Walking", true);
                break;

            case _movement_states.Idle:
                _air_jump_charges = _max_air_jump_charges;
                _charging_audio_manager.Stop();
                _footsteps_audio_manager.Stop();
                _animator.SetBool("Walking", false);
                _animator.SetBool("Falling", false);
                break;
        }

        _movement_state = new_state;
    }


    public void ChangeState (State newState)
    {
        _currentState = newState;
        //Debug.Log ("Player state changed to: " + newState);

        GameController.Instance.UIController.UpdateQuestState(newState);
    }

    public void BigDeathExplosion()
    {
        Instantiate(_large_explosion, transform.position + transform.up * 2, Quaternion.identity);
        _explosion_audio_manager.clip = _large_explosion_SFX;
        _explosion_audio_manager.Play();
    }

    public void BombEnd() // called ONCE when player interacts with bomb 
    {
        _transition_movement_state(_movement_states.Falling);
        Quaternion rocket_angle = Quaternion.Euler(new Vector3(-90, 0, 68));
        Instantiate(_rocket_trail, transform.position, rocket_angle);
        Instantiate(_giant_explosion, transform.position, Quaternion.identity);

        transform.position += transform.up * 4.5f;
        RenderSettings.fog = false;

        _playercamera.GetComponent<CameraController>()._cameraFollowSpeed = 10;
        _camera_destination.transform.localPosition = transform.up * 12 + transform.forward * -3;
        add_camera_shake(60);
        bomb_ending = true;
        _playercamera.GetComponent<CameraController>().bomb_ending = true;

        _explosion_audio_manager.clip = _giant_explosion_SFX;
        _explosion_audio_manager.Play();

        _music_audio_manager.loop = false;
        _music_audio_manager.volume = 1;
        _music_audio_manager.clip = _large_explosion_SFX;
        _music_audio_manager.pitch = 0.7f;
        _music_audio_manager.Play();
    }

    private void _BombEndingProgression() // called every frame AFTER player has interacted with bomb
    {
        _rigidbody.velocity = transform.up * _movespeed * 4;
        _seconds_after_ending += Time.deltaTime; // seconds after player has interacted with bomb 
        Debug.Log(_seconds_after_ending);

        if (_seconds_after_ending > 8f && _seconds_after_ending < 9f) // starts playing IRIS OUT 
        {
            _music_audio_manager.pitch = 1f;
            _music_audio_manager.clip = _iris_out;
            _music_audio_manager.Play();
        }

        if (_seconds_after_ending >= 20f) // Screen fades to white 
        {
            GameController.Instance.UIController._game_end = true;
        }

        if (_seconds_after_ending >= 23f) // ABSOLUTE CINEMA
        {
            GameController.Instance.UIController._display_cinema = true;
        }

    }

    public Rigidbody get_rigidbody()
    {
        return (_rigidbody);
    }

    public void add_camera_shake(float seconds)
    {
        _playercamera.GetComponent<CameraController>()._seconds_of_camera_shake += seconds;
    }
    
}
        
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
public enum State
{
    Accepted, 
    Ready,
    Completed
}


public class player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Camera _playercamera;
    [SerializeField] private float _movespeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<Interactable> _playerInventory;
    [SerializeField] private float _max_upward_momentum;
    [SerializeField] private float _max_forward_momentum;
    [SerializeField] float _min_upward_momentum;
    [SerializeField] private float _min_forward_momentum;
    [SerializeField] float _upward_charge_velocity;
    [SerializeField] float _forward_charge_velocity;
    [SerializeField] AudioSource _audio_manager;

    // VFX 
    [SerializeField] GameObject _tiny_explosion;
    [SerializeField] GameObject _small_explosion;
    [SerializeField] GameObject _large_explosion;
    [SerializeField] GameObject _heat_distortion;
    [SerializeField] GameObject _tiny_dust_blast;
    [SerializeField] GameObject _medium_dust_blast;
    [SerializeField] GameObject _large_dust_blast;
    [SerializeField] GameObject _flame_trail;

    // AUDIO
    [SerializeField] AudioClip _charging_SFX;
    [SerializeField] AudioClip _tiny_explosion_SFX;
    [SerializeField] AudioClip _small_explosion_SFX;
    [SerializeField] AudioClip _large_explosion_SFX;
    [SerializeField] AudioClip _tiny_landing_SFX;


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
    private bool _on_slope = false;
    private float _space_held_time = 0;
    private float _space_held_frames = 0;
    private Vector3 _max_upward_momentum_vector;
    private Vector3 _max_forward_momentum_vector;
    private RaycastHit _raycast_results;
    private float _starting_fall_height;
    private float _seconds_airborne;
    private float _jump_grace_period = 0;
    private int _inventory_selected_index = 0;

    public float _health = 200;
    public float _maxHealth = 200;

    public delegate void HandDelegate(string hand);
    public event HandDelegate HandSelected;

    public delegate void InventoryDelegate(List<Interactable> inventory);
    public event InventoryDelegate InventoryUpdated;

    public float _held_forward_momentum;
    public float _charge_percent;


    //Quest variables
    
    public State _currentState {get; private set; }

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
        _DisplayInteractable();
        _jump_grace_period += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && (_grounded == true || _on_slope == true)) // checks if player is holding down space bar. Can't be in the air. 
        {
            if (_space_held_frames == 0)
            {
                _transition_movement_state(_movement_states.Charging);
            }


            else if (_movement_state == _movement_states.Charging && _space_held_frames > 80)
            {
                _animator.speed = 0;
            }

            if (_audio_manager.pitch < 1.5)
            {
                _audio_manager.pitch += 0.001f;
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

        if (Input.GetKeyUp(KeyCode.Space) && _space_held_time > 0 && (_grounded == true || _on_slope == true)) // check if space was released, player jumps
        {
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


            _playercamera.GetComponent<CameraController>()._seconds_of_camera_shake += _charge_percent;

            if (_charge_percent < 0.20)
            {
                _audio_manager.clip = _tiny_explosion_SFX;
                Instantiate(_tiny_explosion, transform.position + transform.up*2, Quaternion.identity);
            }
            else if (_charge_percent < 0.40)
            {
                _audio_manager.clip = _small_explosion_SFX;
                Instantiate(_small_explosion, transform.position + transform.up*2, Quaternion.identity);
                Instantiate(_heat_distortion, transform.position, Quaternion.identity);
                Instantiate(_flame_trail, transform.position + transform.up, Quaternion.identity);
            }
            else
            {
                _audio_manager.clip = _large_explosion_SFX;
                Instantiate(_large_explosion, transform.position + transform.up*2, Quaternion.identity);
                Instantiate(_heat_distortion, transform.position, Quaternion.identity);
                Instantiate(_flame_trail, transform.position + transform.up, Quaternion.identity);
            }
            _transition_movement_state(_movement_states.Falling);
        }
        List<bool> keylist = new List<bool> { Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.A), Input.GetKey(KeyCode.D) }; //WASD movement
        List<Vector3> normal_move_list = new List<Vector3> { transform.forward * _movespeed, transform.forward * _movespeed * -1 , transform.right * _movespeed * -1 , transform.right * _movespeed };
        List<Vector3> falling_move_list = new List<Vector3> { 0.15f * transform.forward * _movespeed, -0.15f * transform.forward * _movespeed, -0.15f * transform.right * _movespeed , 0.15f * transform.right * _movespeed };

        foreach (int i in new List<int> {0,1,2,3})
        {
            if (keylist[i] == true && _movement_state != _movement_states.Charging)
            {
                if (_grounded == true && _jump_grace_period>0.1f)
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


        if (Input.anyKey == false && _movement_state == _movement_states.Walking)
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
            _playerInventory[_inventory_selected_index].interact();
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

        bool _groundcheck1 = (Physics.Raycast(transform.position, Vector3.down, 0.1f));
        bool _groundcheck2 = (Physics.Raycast(transform.position - transform.forward, Vector3.down, 0.1f));

        if (_groundcheck1 == false && _groundcheck2 == false)
        {
            if (_grounded == true)
            {
                _transition_movement_state(_movement_states.Falling);
                _starting_fall_height = transform.position.y;
            }

            _grounded = false;
            _seconds_airborne += Time.deltaTime;
        }
        else
        {
            if (_grounded == false)
            {
                float fall_distance = _starting_fall_height - transform.position.y;
                _transition_movement_state(_movement_states.Idle);
                Debug.Log("Fell a distance of :" + fall_distance + " to the new height of " + transform.position.y.ToString());
                if (fall_distance > 10)
                {
                    GameController.Instance.UIController.losehealth((fall_distance - 5) / 2);
                }
            }

            _grounded = true;

            if (_seconds_airborne > 0.5)
            {
                Debug.Log(_seconds_airborne);
            }
            if (_seconds_airborne > 2.5)
            {
                _playercamera.GetComponent<CameraController>()._seconds_of_camera_shake += 0.12f;
                Instantiate(_large_dust_blast, transform.position, Quaternion.identity);
                _audio_manager.pitch = 0.9f;
                _audio_manager.clip = _tiny_landing_SFX;
                _audio_manager.Play();
            }

            else if (_seconds_airborne > 2)
            {
                _playercamera.GetComponent<CameraController>()._seconds_of_camera_shake += 0.10f;
                Instantiate(_medium_dust_blast, transform.position, Quaternion.identity);
                _audio_manager.pitch = 0.7f;
                _audio_manager.clip = _tiny_landing_SFX;
                _audio_manager.Play();
            }

            else if (_seconds_airborne > 1)
            {
                _playercamera.GetComponent<CameraController>()._seconds_of_camera_shake += 0.08f;
                Instantiate(_tiny_dust_blast, transform.position, Quaternion.identity);
                _audio_manager.pitch = 0.5f;
                _audio_manager.clip = _tiny_landing_SFX;
                _audio_manager.Play();
            }

            _seconds_airborne = 0;
        }




        if (!_canMove)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }


        _playercamera.GetComponent<CameraController>().UpdateCamera();
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
            _starting_fall_height = transform.position.y;
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

    private void _DisplayInteractable()
    {
        Interactable selected_interactable = _playerInventory[_inventory_selected_index];
        if (selected_interactable.type != "None")
        {
            selected_interactable.gameObject.SetActive(true);
            selected_interactable.transform.position = transform.position + new Vector3(0, 2, 0);
            selected_interactable.transform.rotation = transform.rotation;
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
        Debug.Log(new_state);
        switch (new_state)
        {
            case _movement_states.Charging:

                _space_held_time = 0;
                _audio_manager.clip = _charging_SFX;
                _audio_manager.loop = true;
                _audio_manager.Play();
                _audio_manager.pitch = 0.9f;

                switch (_movement_state)
                {
                    case _movement_states.Walking:
                        _rigidbody.velocity = new Vector3(0, 0, 0);
                        _animator.SetBool("Walking", false);
                        _animator.SetBool("Falling", false);
                        _animator.SetTrigger("Charging");
                        _animator.speed = 0.4f;
                        break;
                }

                _animator.SetBool("Falling", false);
                _animator.SetTrigger("Charging");
                _animator.speed = 0.4f;

                break;

            case _movement_states.Falling:
                _audio_manager.loop = false;
                _audio_manager.pitch = 1f;
                switch (_movement_state)
                {
                    case (_movement_states.Charging):
                    {
                        _audio_manager.Play();
                        break;
                    }
                }


                _space_held_frames = 0;
                _animator.SetBool("Falling", true);
                _animator.SetBool("Walking", false);
                _animator.speed = 1;
                break;

            case _movement_states.Walking:
                _animator.SetBool("Falling", false);
                _animator.SetBool("Walking", true);
                break;

            case _movement_states.Idle:
                _animator.SetBool("Walking", false);
                _animator.SetBool("Falling", false);
                break;
        }

        _movement_state = new_state;
    }


    public void ChangeState (State newState)
    {
        _currentState = newState;
        Debug.Log ("Player state changed to: " + newState);

        GameController.Instance.UIController.UpdateQuestState(newState);
    }

    public void BigDeathExplosion()
    {
        Instantiate(_large_explosion, transform.position + transform.up * 2, Quaternion.identity);
        _audio_manager.clip = _large_explosion_SFX;
        _audio_manager.Play();
    }

}
        
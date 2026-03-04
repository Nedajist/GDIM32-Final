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

    // VFX 
    [SerializeField] GameObject _tiny_explosion;
    [SerializeField] GameObject _small_explosion;
    [SerializeField] GameObject _large_explosion;
    [SerializeField] GameObject _heat_distortion;
    [SerializeField] GameObject _tiny_dust_blast;
    [SerializeField] GameObject _medium_dust_blast;
    [SerializeField] GameObject _large_dust_blast;
    [SerializeField] GameObject _flame_trail;


    public static player Instance { get; private set; }
    private bool _grounded = true;
    private ArrayList _list_of_colliders = new ArrayList();
    public bool _canMove = true;
    public bool _charging = false;
    private bool _on_slope = false;
    private float _space_held_time = 0;
    private float _space_held_frames = 0;
    private Vector3 _max_upward_momentum_vector;
    private Vector3 _max_forward_momentum_vector;
    private RaycastHit _raycast_results;
    private float _starting_fall_height;
    private float _seconds_airborne;
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

    private State _currentState;
    
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

    }


    void Update()
    {

        _DisplayInteractable();
        if (Input.GetKey(KeyCode.Space) && (_grounded == true || _on_slope == true)) // checks if player is holding down space bar. Can't be walking or in the air. 
        {

            if (_animator.GetBool("Walking") == true)
            {
                _rigidbody.velocity = new Vector3(0, 0, 0);
                _animator.SetBool("Walking", false);
            }

            if (_space_held_frames == 0)
            {
                _animator.SetTrigger("Jumping");
                _animator.speed = 0.4f;
            }
            else if (_space_held_frames > 80)
            {
                _animator.speed = 0;
            }
            _space_held_time += Time.deltaTime;
            _space_held_frames += 1;
            _charging = true;

            _held_forward_momentum = _forward_charge_velocity * _space_held_time;
            if (_held_forward_momentum > _max_forward_momentum)
            {
                _held_forward_momentum = _max_forward_momentum;
            }

            _charge_percent = (_held_forward_momentum / (_max_forward_momentum - _min_forward_momentum));

        }

        if (Input.GetKeyUp(KeyCode.Space) && _space_held_time > 0 && (_grounded == true || _on_slope == true)) // check if space was released, frog jumps
        {
            _animator.SetBool("Landing", true);
            _charging = false;


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
            _animator.speed = 1;
            _space_held_time = 0;
            _space_held_frames = 0;

            _playercamera.GetComponent<CameraController>()._seconds_of_camera_shake += _charge_percent;

            if (_charge_percent < 0.20)
            {
                Instantiate(_tiny_explosion, transform.position + transform.up*2, Quaternion.identity);
            }
            else if (_charge_percent < 0.40)
            {
                Instantiate(_small_explosion, transform.position + transform.up*2, Quaternion.identity);
                Instantiate(_heat_distortion, transform.position, Quaternion.identity);
                Instantiate(_flame_trail, transform.position + transform.up, Quaternion.identity);
            }
            else
            {
                Instantiate(_large_explosion, transform.position + transform.up*2, Quaternion.identity);
                Instantiate(_heat_distortion, transform.position, Quaternion.identity);
                Instantiate(_flame_trail, transform.position + transform.up, Quaternion.identity);
            }
        }
        if (Input.GetKey(KeyCode.W) && _charging == false && _on_slope == false)
        {
            if (_grounded == true)
            {
                _animator.SetBool("Walking", true);
                _rigidbody.velocity = (transform.forward * _movespeed);

            }
            else
            {
                _rigidbody.AddForce(0.15f * transform.forward * _movespeed);
            }
        }

        if (Input.GetKey(KeyCode.S) && _charging == false && _on_slope == false)
        {
            if (_grounded == true)
            {
                _animator.SetBool("Walking", true);
                _rigidbody.velocity = (transform.forward * _movespeed * -1);

            }
            else
            {
                _rigidbody.AddForce(-0.15f * transform.forward * _movespeed);
            }
        }

        if (Input.GetKey(KeyCode.A) && _charging == false && _on_slope == false)
        {
            if (_grounded == true)
            {
                _animator.SetBool("Walking", true);
                _rigidbody.velocity = (transform.right * _movespeed * -1);

            }
            else
            {
                _rigidbody.AddForce(-0.15f * transform.right * _movespeed);
            }
        }

        if (Input.GetKey(KeyCode.D) && _charging == false && _on_slope == false)
        {
            if (_grounded == true)
            {
                _animator.SetBool("Walking", true);
                _rigidbody.velocity = (transform.right * _movespeed);

            }
            else
            {
                _rigidbody.AddForce(0.15f * transform.right * _movespeed);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Cursor.lockState == CursorLockMode.Locked) {
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

        if (Input.anyKey == false)
        {
            _animator.SetBool("Walking", false);
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
                Debug.Log("Fell a distance of :" + fall_distance + " to the new height of " + transform.position.y.ToString());
                if (fall_distance > 10)
                {
                    GameController.Instance.UIController.losehealth((fall_distance - 5) / 2);
                }
            }
            _grounded = true;


            if (_seconds_airborne > 3)
            {
                _playercamera.GetComponent<CameraController>()._seconds_of_camera_shake += 0.12f;
                Instantiate(_large_dust_blast, transform.position, Quaternion.identity);
            }

            else if (_seconds_airborne > 2)
            {
                _playercamera.GetComponent<CameraController>()._seconds_of_camera_shake += 0.10f;
                Instantiate(_medium_dust_blast, transform.position, Quaternion.identity);
            }

            else if (_seconds_airborne > 1)
            {
                _playercamera.GetComponent<CameraController>()._seconds_of_camera_shake += 0.08f;
                Instantiate(_tiny_dust_blast, transform.position, Quaternion.identity);
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

    public void ChangeState (State newState)
    {
        _currentState = newState;
        Debug.Log ("Player state changed to: " + newState);

        GameController.Instance.UIController.UpdateQuestState(newState);
    }

}

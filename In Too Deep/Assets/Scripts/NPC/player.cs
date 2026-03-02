using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    public static player Instance {get; private set; }
    private bool _grounded = true;
    private ArrayList _list_of_colliders = new ArrayList();
    public bool _canMove = true;
    private bool _charging = false;
    private bool _falling = false;
    private bool _on_slope = false;
    private float _space_held_time = 0;
    private float _space_held_frames = 0;
    private Vector3 _max_upward_momentum;
    private Vector3 _max_forward_momentum;
    private RaycastHit _raycast_results;
    private float _starting_fall_height;
    private int _inventory_selected_index = 0;

    public delegate void HandDelegate(string hand);
    public event HandDelegate HandSelected;


    //Quest variables

    private State _currentState;

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
        _max_upward_momentum = 1000 * transform.up;
        _max_forward_momentum = 600 * transform.forward;
        _ClearInteractable(0);
        _ClearInteractable(1);
    }

    
    void Update()
    {
        Debug.Log(_playerInventory[0]);
        _DisplayInteractable();
        // print("grounded: " + _grounded.ToString() + " velocity: " + _rigidbody.velocity.ToString());
        if (Input.GetKey(KeyCode.Space) && ( _grounded == true || _on_slope == true)) // checks if player is holding down space bar. Can't be walking or in the air. 
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
            else if (_space_held_frames > 200)
            {
                _animator.speed = 0;
            }
            _space_held_time += Time.deltaTime;
            _space_held_frames += 1;
            _charging = true;

        }
        if (Input.GetKeyUp(KeyCode.Space) && _space_held_time > 0 && _grounded == true) // check if space was released, frog jumps
        {
            _animator.SetBool("Landing", true);
            _charging = false;
            _grounded = false;
            Vector3 _upward_momentum = (600 * transform.up * _space_held_time) + 250 * transform.up;
            Vector3 _forward_momentum = (300 * transform.forward * _space_held_time) + 150 * transform.forward;

            if (_upward_momentum.y > _max_upward_momentum.y){
                _upward_momentum = _max_upward_momentum;
            }

            if (_forward_momentum.z > _max_forward_momentum.z){
                _forward_momentum = _max_forward_momentum;
            }
            Debug.Log("After " + _space_held_time.ToString() + " seconds, launched with a force of " + _upward_momentum.y.ToString() + " " + _forward_momentum.z.ToString());
            _rigidbody.AddForce(_upward_momentum);
            _rigidbody.AddForce(_forward_momentum);
            _animator.speed = 1;
            _space_held_time = 0;
            _space_held_frames = 0;
        }
        if (Input.GetKey(KeyCode.W) && _charging == false)
        {
            if (_falling != true)
            {
                _animator.SetBool("Walking", true);
                _rigidbody.velocity = (transform.forward * _movespeed);

            }
            else
            {
                _rigidbody.AddForce(0.15f * transform.forward * _movespeed);
            }
        }

        if (Input.GetKey(KeyCode.S) && _charging == false)
        {
            if (_falling != true)
            {
                _animator.SetBool("Walking", true);
                _rigidbody.velocity = (transform.forward * _movespeed * -1);

            }
            else
            {
                _rigidbody.AddForce(-0.15f * transform.forward * _movespeed);
            }
        }

        if (Input.GetKey(KeyCode.A) && _charging == false)
        {
            if (_falling != true)
            {
                _animator.SetBool("Walking", true);
                _rigidbody.velocity = (transform.right * _movespeed * -1);

            }
            else
            {
                _rigidbody.AddForce(-0.15f * transform.right * _movespeed);
            }
        }

        if (Input.GetKey(KeyCode.D) && _charging == false)
        {
            if (_falling != true)
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
       
        
        if (Input.GetKeyDown(KeyCode.Escape) && _dialogueActive == true)
        {
            Hide();
        }

        if(!_canMove)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        
        

        UpdateState();

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

        if (_list_of_colliders.Count == 0)
        {
            _grounded = false;
            _falling = true;
            _starting_fall_height = transform.position.y;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _list_of_colliders.Add(collision);
            float fall_distance = _starting_fall_height - transform.position.y;
            Debug.Log("Fell a distance of :" + fall_distance);
            if (fall_distance > 10)
            {
                GameController.Instance.UIController.losehealth(10 * fall_distance);
            }
            _grounded = true;
            _falling = false;
        }
        if (collision.transform.CompareTag("Obstacle"))
        {
            _list_of_colliders.Add(collision);
            bouncy_object obstacle = collision.transform.GetComponent<bouncy_object>();
            _rigidbody.AddExplosionForce(obstacle.repel_force, transform.position, 100);

        }
        if (collision.transform.CompareTag("Slope"))
        {
            _list_of_colliders.Add(collision);
            _grounded = true;
            _on_slope = true;
            _falling = false;
        }
    }
    
    private void _ClearInteractable(int index)
    {
        _playerInventory[index].name = "None";
        _playerInventory[index].type = "None";
        _playerInventory[index].GetComponent<MeshRenderer>().gameObject.SetActive(false);
    }

    private void _DisplayInteractable()
    {
        Interactable selected_interactable = _playerInventory[_inventory_selected_index];
        if (selected_interactable.type != "None")
        {
            selected_interactable.gameObject.SetActive(true);
            selected_interactable.transform.position = transform.position + new Vector3(0, 2, 0);
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
                break;
            }
        }
    }

    public void TalkToNPC(NPC npc)
    {
        
    }

        public void Show()
    {
        _canMove = false;
        _dialogueActive = true;
  
    }

    public void Hide()
    {
        _canMove = true;
        _dialogueActive = false;
       
    }


    private void UpdateState()
    {
        if (_dialogueActive == true && Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Working");
            StateChanged(State.Accepted);
            Hide();
            GameController.Instance.UIController._currentQuestStatus = "Accepted";
        }
        else if (_dialogueActive == true && Input.GetKeyDown(KeyCode.Alpha2))
        {
            Hide();
        }
    }

    private void StateChanged(State newState)
    {
        
    }

}

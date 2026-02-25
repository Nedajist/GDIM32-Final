using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private UIController _UIcontroller;
    private bool _grounded = true;
    public bool _canMove = true;
    private bool _charging = false;
    private float _space_held_time = 0;
    private Vector3 _max_upward_momentum;
    private Vector3 _max_forward_momentum;

    private float _starting_fall_height;

    //Quest variables

    private State _currentState;
    [SerializeField] private PlayerInteract _playerInteract;


    void Start()
    {
        _max_upward_momentum = 1200 * transform.up;
        _max_forward_momentum = 600 * transform.forward;
    }

    
    void Update()
    {
        // print("grounded: " + _grounded.ToString() + " velocity: " + _rigidbody.velocity.ToString());
        if (Input.GetKey(KeyCode.Space) && _grounded == true && ( (_rigidbody.velocity.x + _rigidbody.velocity.y+_rigidbody.velocity.z)<6))
        {
            _space_held_time += Time.deltaTime;
            _charging = true;
        }
        else
        {
            if (_space_held_time > 0 && _grounded == true)
            {
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
                _space_held_time = 0;

            }
        }

        if (Input.GetKey(KeyCode.W) && _charging == false && _grounded == true)
        {
            _rigidbody.velocity=(transform.forward * _movespeed);
        }

        if (Input.GetKey(KeyCode.S) && _charging == false && _grounded == true)
        {
            _rigidbody.velocity = (transform.forward * _movespeed * -1);
        }

        if (Input.GetKey(KeyCode.A) && _charging == false && _grounded == true)
        {
            _rigidbody.velocity = (transform.right * _movespeed * -1);
        }

        if (Input.GetKey(KeyCode.D) && _charging == false && _grounded == true)
        {
            _rigidbody.velocity = (transform.right * _movespeed);
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
            _grounded = false;
            _starting_fall_height = transform.position.y;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            float fall_distance = _starting_fall_height - transform.position.y;
            Debug.Log("Fell a distance of :" + fall_distance);
            if (fall_distance > 10)
            {
                _UIcontroller.losehealth(10 * fall_distance);
            }
            _grounded = true;
        }
    }


    private void UpdateState()
    {
        if (_playerInteract._dialogueActive == true && Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Working");
            //StateChanged(State.Accepted);
            _playerInteract.Hide();
            _UIcontroller._currentQuestStatus = "Accepted";
        }
        else if (_playerInteract._dialogueActive == true && Input.GetKeyDown(KeyCode.Alpha2))
        {
            _playerInteract.Hide();
        }
    }

    private void StateChanged(State newState)
    {
        
    }

}

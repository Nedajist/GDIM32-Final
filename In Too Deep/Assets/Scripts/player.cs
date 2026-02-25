using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Camera _playercamera;
    [SerializeField] private float _movespeed;
    [SerializeField] private UIController _UIcontroller;
    [SerializeField] private Animator _animator;
    private bool _grounded = true;
    public bool _canMove = true;
    private bool _charging = false;
    private float _space_held_time = 0;
    private float _space_held_frames = 0;
    private Vector3 _max_upward_momentum;
    private Vector3 _max_forward_momentum;

    private float _starting_fall_height;


    // Start is called before the first frame update
    void Start()
    {
        _max_upward_momentum = 1200 * transform.up;
        _max_forward_momentum = 600 * transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        // print("grounded: " + _grounded.ToString() + " velocity: " + _rigidbody.velocity.ToString());
        if (Input.GetKey(KeyCode.Space) && _grounded == true) // checks if player is holding down space bar. Can't be walking or in the air. 
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
        if (Input.GetKey(KeyCode.W) && _charging == false && _grounded == true)
        {
            _animator.SetBool("Walking", true);
            _rigidbody.velocity = (transform.forward * _movespeed);
        }

        if (Input.GetKey(KeyCode.S) && _charging == false && _grounded == true)
        {
            _animator.SetBool("Walking", true);
            _rigidbody.velocity = (transform.forward * _movespeed * -1);
        }

        if (Input.GetKey(KeyCode.A) && _charging == false && _grounded == true)
        {
            _animator.SetBool("Walking", true);
            _rigidbody.velocity = (transform.right * _movespeed * -1);
        }

        if (Input.GetKey(KeyCode.D) && _charging == false && _grounded == true)
        {
            _animator.SetBool("Walking", true);
            _rigidbody.velocity = (transform.right * _movespeed);
        }

        if (Input.anyKey == false)
        {
            _animator.SetBool("Walking", false);
        }
        if(!_canMove)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }

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
        if (collision.transform.CompareTag("Obstacle"))
        {
            bouncy_object obstacle = collision.transform.GetComponent<bouncy_object>();
            _rigidbody.AddExplosionForce(obstacle.repel_force, transform.position, 100);

        }
    }
}

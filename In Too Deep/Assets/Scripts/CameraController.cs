using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _rotationspeed = 5;
    [SerializeField] private float _cameraSpeed = 5;
    [SerializeField] public float _cameraFollowSpeed = 3;
    [SerializeField] public float _minimum_pursue_distance = 1f;

    [SerializeField] private GameObject _player;
    private Transform destination;
    private float _follow_time = 0;
    private Vector3 _camera_rotation = new Vector3(0f, 0f, 0f);

    public float _seconds_of_camera_shake = 0;
    public Quaternion _frozen_rotation;
    public bool bomb_ending = false;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        destination = _player.transform.Find("CameraDestination");
    }

    // Update is called once per frame
    public void UpdateCamera()
    {
        _follow_time -= Time.deltaTime;
        float mouse_x_movement = Input.GetAxis("Mouse X") * _rotationspeed;
        float mouse_y_movement = Input.GetAxis("Mouse Y") * _rotationspeed;
        if (mouse_x_movement != 0 || mouse_y_movement != 0) // if mouse has moved, follow time increases
        {
            _follow_time = 0.5f;
        }
   
        // The camera will follow the player if at least one of three conditions are met
        // 1: The player isn't idle
        // 2: The player has moved their mouse within the last 0.5 seconds
        // 3: The camera is sufficiently far from the player
        if (GameController.Instance.Player._movement_state != player._movement_states.Idle || _follow_time > 0 || Vector3.Distance(transform.position, destination.transform.position) > _minimum_pursue_distance)
        {
            transform.position = Vector3.Lerp(transform.position, destination.transform.position, Time.deltaTime * _cameraFollowSpeed);
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            _camera_rotation.x += mouse_x_movement;
            _camera_rotation.y -= mouse_y_movement;

            _camera_rotation.y = Mathf.Clamp(_camera_rotation.y, -13f, 50f);

            if (bomb_ending)
            {
                _camera_rotation.y = 90;
            }

            Quaternion _camera_quaternion = Quaternion.Euler(_camera_rotation.y, _camera_rotation.x, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, _camera_quaternion, Time.deltaTime * _cameraSpeed);

            Quaternion _player_quaternion = Quaternion.Euler(0, _camera_rotation.x, 0);
            _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, _player_quaternion, _cameraSpeed);
        }
        else
        {
            _player.transform.rotation = _frozen_rotation;
        }

        if (_seconds_of_camera_shake > 0)
        {
            _seconds_of_camera_shake -= Time.deltaTime;
            ShakeCamera();
        }
        
    }
    
    private void ShakeCamera()
    {
        transform.localEulerAngles += _RandomNormalVector3();
    }

    private Vector3 _RandomNormalVector3()
    {
        return (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f,1f), Random.Range(-1f, 1f)));
    }

}

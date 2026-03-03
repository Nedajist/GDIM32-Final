using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _rotationspeed = 20;
    [SerializeField] private float _cameraSpeed = 10;
    [SerializeField] private float _cameraFollowSpeed = 3;
    [SerializeField] private GameObject _player;
    private Transform destination;

    private Vector3 _camera_rotation = new Vector3(0f, 0f, 0f);
    public Quaternion _frozen_rotation;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        destination = _player.transform.Find("CameraDestination");
    }

    // Update is called once per frame
    public void UpdateCamera()
    {
        
       transform.position=Vector3.Lerp(transform.position, destination.transform.position, Time.deltaTime * _cameraFollowSpeed);

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            _camera_rotation.x += Input.GetAxis("Mouse X") * _rotationspeed;
            _camera_rotation.y -= Input.GetAxis("Mouse Y") * _rotationspeed;

            _camera_rotation.y = Mathf.Clamp(_camera_rotation.y, -13f, 50f);
            Quaternion _camera_quaternion = Quaternion.Euler(_camera_rotation.y, _camera_rotation.x, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, _camera_quaternion, Time.deltaTime * _cameraSpeed);

            Quaternion _player_quaternion = Quaternion.Euler(0, _camera_rotation.x, 0);
            _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, _player_quaternion, _cameraSpeed);
        }
        else
        {
            _player.transform.rotation = _frozen_rotation;
        }




    }   
}

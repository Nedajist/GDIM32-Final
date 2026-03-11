using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MakiBean : Interactable
{
    [SerializeField] public float healing_seconds;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] AudioSource _audio_manager;


    [SerializeField] ParticleSystem _red_explosion;
    private void Start()
    {
        type = "Makibean";
    }

    public override void interact()
    {
        GameController.Instance.UIController.gainhealth(healing_seconds);
        Instantiate(_red_explosion, transform.position, Quaternion.identity);
        GameController.Instance.Player.get_rigidbody().AddForce(transform.forward * 1000 + transform.up * 600);
        GameController.Instance.Player.add_camera_shake(1);
        _audio_manager.Play();
    }

}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombItem : Interactable
{
    [SerializeField] Rigidbody _rigidbody;
    private void Start()
    {
        type = "Bomb";
        
    }

    public override void interact()
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!");
        GameController.Instance.Player.BombEnd();
    }

}
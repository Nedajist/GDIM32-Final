using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coffee : Interactable
{
    [SerializeField] Rigidbody _rigidbody;
    private void Start()
    {
        type = "coffee";
    }

    public override void interact()
    {
        Debug.Log("Drank coffee");
        GameController.Instance.Player._max_air_jump_charges += 1;
    }

}
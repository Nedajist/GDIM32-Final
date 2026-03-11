using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MakiBean : Interactable
{
    [SerializeField] public float healing_seconds;
    [SerializeField] Rigidbody _rigidbody;
    private void Start()
    {
        type = "Makibean";
    }

    public override void interact()
    {
        GameController.Instance.UIController.gainhealth(healing_seconds);
    }

}
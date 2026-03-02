using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string type = "None";
    [SerializeField] public string item_name = "None";

    public virtual void interact()
    {

    }



}
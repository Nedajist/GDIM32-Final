using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string type = "None";
    public Vector3 carry_vector = new Vector3(0, 0, 0);
    [SerializeField] public string item_name = "None";

    public virtual void interact()
    {

    }



}
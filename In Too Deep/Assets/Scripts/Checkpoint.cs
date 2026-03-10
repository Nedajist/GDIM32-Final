using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIController ui = FindObjectOfType<UIController>();
            ui.SetRespawnPoint(transform);   
            Debug.Log("respawn point set to " + transform.position);
        }
    }
}

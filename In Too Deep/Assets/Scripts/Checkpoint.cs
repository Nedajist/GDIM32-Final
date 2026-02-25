using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private void OnTriggerEnter(Collider isPlayer)
    {
        if (isPlayer.CompareTag("Player"))
        {
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private int _currentCheckpointID;
    public Transform spawnPoint;
    private void OnTriggerEnter(Collider isPlayer)
    {
        if (isPlayer.CompareTag("Player"))
        {
            
        }
    }
}

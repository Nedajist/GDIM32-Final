using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private int _currentCheckpointID;
    [SerializeField] player _player;
    public Transform spawnPoint;
    void Start()
    {
        spawnPoint = _player.transform;
    }
    private void OnTriggerEnter(Collider isPlayer)
    {
        if (isPlayer.CompareTag("Player"))
        {
            
        }
    }
}

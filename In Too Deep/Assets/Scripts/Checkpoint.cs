using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private int _currentCheckpointID;
    //declared a public variable to store the spawn point transform
    public Transform spawnPoint;
    void Start()
    {
        //sets the initial spawn point
        spawnPoint = GameController.Instance.Player.transform;
    }
    private void OnTriggerEnter(Collider isPlayer)
    {
        if (isPlayer.CompareTag("Player"))
        {
            //resets the spawn point whenever player collides with checkpoint collider
            spawnPoint = GameController.Instance.Player.transform;
        }
    }
}

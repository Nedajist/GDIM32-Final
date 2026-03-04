using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private GameObject mushroomManIsland;

    void Start()
    {
    /// <summary>
    /// FOR TESTING PURPOSES ONLY
    /// I am spawning the player at the mushroom man island, so I need to set the respawn point
    /// </summary>
        UIController ui = FindObjectOfType<UIController>();
        ui.SetRespawnPoint(mushroomManIsland.transform);
    }
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

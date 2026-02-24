using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool _nearNPC;
    private bool _dialogueActive;
    [SerializeField] private player _player;
    [SerializeField] private GameObject _dialogueText;
    [SerializeField] private GameObject _enabler;
    private NPC _npc;

    void Update()
    {
        //initial interaction with the NPC. Checking if the player is near an NPC
        if (Input.GetKeyDown(KeyCode.E) && _nearNPC && !_dialogueActive)
        {
            Show();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _nearNPC && _dialogueActive)
        {
            Hide();
        }


    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("NPC"))
        {
            Debug.Log("near NPC");
            _nearNPC = true;   
            _enabler.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("NPC"))
        {
            _nearNPC = false;
            _enabler.SetActive(false);
        }
    }


    void Show()
    {
        _player._canMove = false;
        _dialogueActive = true;
        _dialogueText.SetActive(true);
    }

    void Hide()
    {
        _player._canMove = true;
        _dialogueActive = false;
        _dialogueText.SetActive(false);
    }
}

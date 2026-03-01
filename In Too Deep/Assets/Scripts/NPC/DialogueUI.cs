using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _npcText;
    [SerializeField] private GameObject _npcDialogue;
    [SerializeField] private GameObject _playerOptions;
    [SerializeField] private GameObject _playerMultiOptions;
    [SerializeField] private TMP_Text _option1;
    [SerializeField] private TMP_Text _option2;
    [SerializeField] private TMP_Text _option3;


    public void ShowDialogue(string dialogue)
    {
        gameObject.SetActive(true);
        
        _npcDialogue.SetActive(true);
        _playerOptions.SetActive(false);
        _playerMultiOptions.SetActive(false);

        _npcText.text = dialogue;
    }

    public void ShowPlayerOptions()
    {
        gameObject.SetActive(true);

        _npcDialogue.SetActive(false);
        _playerOptions.SetActive(true);
        _playerMultiOptions.SetActive(false);
    }   

    public void ShowPlayerOptions(string[] options)
    {
        gameObject.SetActive(true);

        _npcDialogue.SetActive(false);
        _playerOptions.SetActive(false);
        _playerMultiOptions.SetActive(true);

        _option1.text = options[0];

        if(options.Length >= 2)
        {
            _option2.transform.parent.gameObject.SetActive(true);
            _option2.text = options[1];
        }
        else 
        {
            _option2.transform.parent.gameObject.SetActive(false);
        }

        if(options.Length >= 3)
        {
            _option3.transform.parent.gameObject.SetActive(true);
            _option3.text = options[2];
        }
        else 
        {
            _option3.transform.parent.gameObject.SetActive(false);
        }
    }

    public void HideDialogue()
    {
        _playerOptions.SetActive(false);
        _npcDialogue.SetActive(false);
        gameObject.SetActive(false);
    }
}


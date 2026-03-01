using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueNPC : NPC
{
    [SerializeField] private TMP_Text _text;
    public override void Interact(string dialogue)
    {
        _text.text = dialogue;
        
    }
}

using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] protected string[] _dialogueLines;
    protected virtual void Interact()
    {
        for (int i = 0; i < _dialogueLines.Length; i++)
        {
            Debug.Log(_dialogueLines[i]);
        }
    }
}

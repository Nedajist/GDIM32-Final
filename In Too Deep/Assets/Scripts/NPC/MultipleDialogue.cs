using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum NPCType
{
    MushroomMan,
    HopHop,
    Monster
}
public class MultipleDialogue : MonoBehaviour
{
    [SerializeField] private float _interactionDistance = 2.0f;
    [SerializeField] private Sprite _interactionPromptSprite;
    [SerializeField] private Image _thoughtBubble;
    [SerializeField] private DialogueUI _dialogue;
    [SerializeField] private DialogueNode _startNode;
    
    [SerializeField] private DialogueNode _questAcceptNode;
    [SerializeField] private NPCType _npcType;
    
    public UIController _uicontroller;

    private DialogueNode _currentNode;
    private int _currentLine = 0;
    private bool _runningDialogue;
    private bool _waitingForPlayerResponse;

    private void Start ()
    {
        
    }

    private void Update ()
    {
        if(player.Instance == null) return;

        if(Vector3.Distance(transform.position, player.Instance.transform.position) < _interactionDistance)
        {
            _thoughtBubble.gameObject.SetActive(true);

           
            if(!_waitingForPlayerResponse && Input.GetKeyDown(KeyCode.Mouse0))
            {
                AdvanceDialogue();
            }
            else if(!_runningDialogue)
            {
                _thoughtBubble.sprite = _interactionPromptSprite;
            }            

    
        }
        else
        {
            EndDialogue();
        }
    }

    private void AdvanceDialogue ()
    {
        if (!_runningDialogue)
        {
            _currentNode = _startNode;
        }

        if(_currentNode == null) return;

        _runningDialogue = true;

        _runningDialogue = true;
        _thoughtBubble.sprite = _currentNode._thoughtBubbleSprite;

        if(_currentLine < _currentNode._lines.Length)
        {
            _dialogue.ShowDialogue(_currentNode._lines[_currentLine]);
            _currentLine++;
        }
        else if(_currentNode._playerReplyOptions != null && _currentNode._playerReplyOptions.Length > 0)
        {
            _waitingForPlayerResponse = true;
            _dialogue.ShowPlayerOptions(_currentNode._playerReplyOptions);
        }
        else 
        {
            EndDialogue();
        }
    }

    private void EndDialogue ()
    {
        _runningDialogue = false;
        _waitingForPlayerResponse = false;
        _currentLine = 0;

        _currentNode = null;

        _dialogue.HideDialogue();
        _thoughtBubble.gameObject.SetActive(false);
    }

    public void SelectedOption(int option)
    {
        _currentLine = 0;
        _waitingForPlayerResponse = false;
        //Mushroom Man gives quest 1
        if (_npcType == NPCType.MushroomMan && _currentNode == _questAcceptNode && option == 0)
        {
            player.Instance.quest1Stage = 1;
            Debug.Log("quest 1 stage = 1");
        }

        //Hop Hop gives quest 2
        if(_npcType == NPCType.HopHop && player.Instance.quest1Stage == 1)
        {
            player.Instance.quest1Stage = 2;
        }

        if (_npcType == NPCType.HopHop && player.Instance.quest1Stage == 2 && _currentNode == _questAcceptNode && option == 0)
        {
            player.Instance.quest2Stage = 1;
        }

        _currentNode = _currentNode._npcReplies[option];
        Debug.Log(_currentNode);
        AdvanceDialogue();
    }

}

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
    [SerializeField] private Sprite _speakingSprite;
    [SerializeField] private Image _thoughtBubble;
    [SerializeField] private DialogueUI _dialogue;

    [SerializeField] private DialogueNode _startNode;
    [SerializeField] private DialogueNode _oneLineNode;
    [SerializeField] private DialogueNode _finalNode;

    [SerializeField] private DialogueNode _questAcceptNode;

    [SerializeField] private NPCType _npcType;
    [SerializeField] private int _requiredQuest1Stage = 0;
    [SerializeField] private int _requiredQuest2Stage = 0;
    private bool activateFinalLine;

    private DialogueNode _currentNode;
    private int _currentLine = 0;


    private bool _runningDialogue;
    private bool _waitingForPlayerResponse;
    private static MultipleDialogue _activeNPC;

    private void Start()
    {
        _thoughtBubble.enabled = false;
        _thoughtBubble.sprite = _interactionPromptSprite;
    }

    private void Update()
    {
        if (_activeNPC != null && _activeNPC != this)
            return;
        
        if (player.Instance == null) return;

        float distance = Vector3.Distance(transform.position, player.Instance.transform.position);

        if (distance < _interactionDistance)
        {
            if (!_runningDialogue)
            {
                _thoughtBubble.enabled = true;
            }

            if (!_waitingForPlayerResponse && Input.GetKeyDown(KeyCode.Mouse0))
            {
                AdvanceDialogue();
            }
        }
        else if (_runningDialogue && !_waitingForPlayerResponse)
        {
            EndDialogue();
        }

        else
        {
            _thoughtBubble.enabled = false;
        }
    }

    private void AdvanceDialogue()
    {
        _thoughtBubble.sprite = _speakingSprite;
        if (player.Instance._dialogueActive && !_runningDialogue)
            return;

        if (!_runningDialogue)
        {
            if(_activeNPC != null && _activeNPC != this)
                return;
            
            _activeNPC = this;

            if (player.Instance.quest1Stage < _requiredQuest1Stage || player.Instance.quest2Stage < _requiredQuest2Stage)
            {
                _currentNode = _oneLineNode;
                activateFinalLine = true;

                if (activateFinalLine && player.Instance.quest1Stage > _requiredQuest1Stage || player.Instance.quest2Stage > _requiredQuest2Stage)
                {
                    _currentNode = _finalNode;
                }
            }

            else if (player.Instance.quest1Stage > _requiredQuest1Stage || player.Instance.quest2Stage > _requiredQuest2Stage)
            {
                _currentNode = _finalNode;
            }
            else
            {
                _currentNode = _startNode;
                _currentLine = 0;
                _runningDialogue = true;
            }

        }

        if (_currentNode == null) return;

        _thoughtBubble.sprite = _speakingSprite;

        if (_currentLine < _currentNode._lines.Length)
        {
            _dialogue.ShowDialogue(_currentNode._lines[_currentLine]);
            _currentLine++;
            return;
        }

        if (_currentNode._playerReplyOptions != null && _currentNode._playerReplyOptions.Length > 0)
        {
            _waitingForPlayerResponse = true;
            _dialogue.ShowPlayerOptions(_currentNode._playerReplyOptions);
            return;
        }

        EndDialogue();
    }

    public void SelectedOption(int option)
    {
        _currentLine = 0;
        _waitingForPlayerResponse = false;

        if (_currentNode == null)
        {
            Debug.Log("Current node is NULL");
            return;
        }

        // Quest logic
        //accepted quest 1 by talking to mushroom man
        if (_npcType == NPCType.MushroomMan && _currentNode == _questAcceptNode && option == 0 && player.Instance.quest1Stage < 1) 
        {

            player.Instance.quest1Stage = 1;
            Debug.Log("Quest 1 started");
            Debug.Log("Quest stage = 1");
            Debug.Log(player.Instance.quest1Stage);
        }
        //completed quest 1 by finding and talking to hop hop
        if (_npcType == NPCType.HopHop && player.Instance.quest1Stage == 1 && player.Instance.quest1Stage < 2)
        {
            player.Instance.quest1Stage = 2;
            Debug.Log("Quest 1 complete");
            Debug.Log("Quest stage = 2");
            Debug.Log(player.Instance.quest1Stage);
        }
        //accepted quest 2 by talking to hop hop and choosing quest accept node in dialogue
        if (_npcType == NPCType.HopHop && player.Instance.quest1Stage == 2 && _currentNode == _questAcceptNode && option == 0)
        {
            player.Instance.quest2Stage = 1;
            Debug.Log("Quest 2 started");
            Debug.Log(player.Instance.quest2Stage);
        }
        //completed quest 2 by talking to monster
        if (_npcType == NPCType.Monster && player.Instance.quest2Stage == 1)
        {
            player.Instance.quest2Stage = 2;
            Debug.Log("Quest 2 complete");
            Debug.Log(player.Instance.quest1Stage);
        }

        if (_npcType == NPCType.Monster && player.Instance.quest2Stage == 2 && _currentNode == _questAcceptNode && option == 0)
        {
            player.Instance.quest2Stage = 3;
            Debug.Log("Quest 3 started");
        }
        if (_npcType == NPCType.Monster && player.Instance.quest2Stage == 4)
        {
            player.Instance.ClearBomb();
            Debug.Log("Quest 3 Completed");
        }


        if(_currentNode._npcReplies == null || option >= _currentNode._npcReplies.Length)
        {
            Debug.LogError("Dialogue node missing for option: " + option);
            EndDialogue();
            return;
        }

        _currentNode = _currentNode._npcReplies[option];
        AdvanceDialogue();
    }

    private void EndDialogue()
    {
        _runningDialogue = false;
        _waitingForPlayerResponse = false;
        _currentLine = 0;

        _currentNode = null;

        if (_activeNPC == this) 
            _activeNPC = null;

        _dialogue.HideDialogue();
        _thoughtBubble.sprite = _interactionPromptSprite;
    }


    public static void SelectOptionFromUI(int option)
{
    if (_activeNPC == null)
    {
        Debug.LogWarning("No active NPC dialogue.");
        return;
    }

    _activeNPC.SelectedOption(option);
}
}
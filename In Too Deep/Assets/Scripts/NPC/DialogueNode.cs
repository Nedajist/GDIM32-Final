using UnityEngine;

[CreateAssetMenu(fileName = "DialogueLine", menuName  = "ScriptableObjects")]
public class DialogueNode : ScriptableObject
{
    public string[] _lines;
    public Sprite _thoughtBubbleSprite;
    public string[] _playerReplyOptions;
    public DialogueNode[] _npcReplies;
}

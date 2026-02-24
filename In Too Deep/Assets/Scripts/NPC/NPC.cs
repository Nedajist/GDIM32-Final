using UnityEngine;
public enum questState
{
    Accepted, 
    Ready, 
    Completed
}
public abstract class NPC : MonoBehaviour
{
    [SerializeField] protected string npcName;
    protected questState _currentQuestState;
    

    void Start()
    {
        
    }

    public abstract void Interact(player player);
}

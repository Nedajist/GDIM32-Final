using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    [SerializeField] protected string npcName;

    public abstract void Interact(player player);
}

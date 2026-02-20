using UnityEngine;

public class FriendlyNPC : NPC
{
    public override void Interact(player player)
    {
        Debug.Log(npcName + ": Hello there!");
    }
}

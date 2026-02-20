using UnityEngine;

public class HostileNPC : NPC
{
    public override void Interact(player player)
    {
        Debug.Log(npcName + ": Get out of here!");
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "Dialogue System/NPC Dialogue")]
public class NPCDialogueData : ScriptableObject
{
    public string npcName;
    public string[] firstTimeDialogue;
    public string[] regularDialogue;
    public bool canOpenShop;
}

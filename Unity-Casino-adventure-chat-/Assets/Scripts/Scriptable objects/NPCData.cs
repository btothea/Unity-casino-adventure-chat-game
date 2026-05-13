using UnityEngine;

// Lets you create NPC assets in Unity:
[CreateAssetMenu(fileName = "NewNPC", menuName = "Text Adventure/NPC")]
public class NPCData : ScriptableObject
{
    // The display name of the NPC (ex: "Dealer", "Gatekeeper")
    public string npcName;

    // Their role in the game (ex: "Tutorial", "Quest Giver", "Merchant")
    public string role;

    // Adds a larger text box in the Inspector for writing details
    [TextArea(3, 6)]
    public string purpose;

    // The quest this NPC is tied to
    // Could be null if they don't give a quest
    public QuestData relatedQuest;

    // The dialogue tree this NPC uses when interacting with the player
    public DialogueData dialogue;
}
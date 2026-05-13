using UnityEngine;

// creates ScriptableObject from the Unity menu:
[CreateAssetMenu(fileName = "NewRoom", menuName = "Text Adventure/Room")]
public class RoomData : ScriptableObject
{
    // The name of the room (ex: "Casino Lobby", "Vault")
    public string roomName;

    // This adds a bigger text box in the Unity Inspector
    // (min 3 lines, max 6 lines)
    [TextArea(3, 6)]
    public string description;

    // Stores connections to other rooms
    public string[] connections;

    // Array of NPCs that exist in this room
    public NPCData[] npcsInRoom;

    // Array of items that exist in this room
    public ItemData[] itemsInRoom;
}
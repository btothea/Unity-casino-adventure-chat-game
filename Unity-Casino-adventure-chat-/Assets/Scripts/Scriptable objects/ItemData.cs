using UnityEngine;

// Lets you create items in Unity:
[CreateAssetMenu(fileName = "NewItem", menuName = "Text Adventure/Item")]
public class ItemData : ScriptableObject
{
    // The name shown to the player
    public string itemName;

    // Larger text box for describing the item
    // Used for flavor or gameplay hints
    [TextArea(3, 6)]
    public string description;

    // This links the item to a game flag
    public string relatedFlag;
}
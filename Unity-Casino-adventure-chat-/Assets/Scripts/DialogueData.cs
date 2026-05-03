using UnityEngine;

// Lets you create Dialogue assets in Unity:
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Text Adventure/Dialogue")]
public class DialogueData : ScriptableObject
{
    // The name of this dialogue tree
    public string dialogueName;

    // A large text box in the Inspector for writing the full dialogue tree
    // - NPC lines
    // - Player choices
    // - Branch paths
    // - Required flags/conditions
    // - End states
    [TextArea(10, 30)]
    public string fullDialogueTree;
}
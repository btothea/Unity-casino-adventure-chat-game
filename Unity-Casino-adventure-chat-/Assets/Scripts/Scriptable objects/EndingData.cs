using UnityEngine;

// Lets you create Ending assets in Unity:
[CreateAssetMenu(fileName = "NewEnding", menuName = "Text Adventure/Ending")]
public class EndingData : ScriptableObject
{
    // Name of the ending (used for identification)
    public string endingName;

    // The condition required to trigger this ending
    [TextArea(3, 6)]
    public string condition;

    // The text shown to the player when this ending is reached
    // This is your final narrative moment
    [TextArea(4, 8)]
    public string endingText;
}
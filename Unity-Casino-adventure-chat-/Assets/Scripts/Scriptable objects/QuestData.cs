using UnityEngine;

// Lets you create Quest assets in Unity:
[CreateAssetMenu(fileName = "NewQuest", menuName = "Text Adventure/Quest")]
public class QuestData : ScriptableObject
{
    // The name of the quest
    public string questName;

    // What must happen for the quest to start
    [TextArea(2, 5)]
    public string triggerCondition;

    // What must happen for the quest to complete
    [TextArea(2, 5)]
    public string completionCondition;

    // What happens when the quest is completed
    [TextArea(2, 5)]
    public string rewardOrOutcome;
}
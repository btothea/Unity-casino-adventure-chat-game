using UnityEngine;
using TMPro;


/// Controls the Save Game button.
/// Handles saving the player's progress
/// and displaying a save confirmation message.

public class SaveButtonController : MonoBehaviour
{
    // Text shown after the player saves the game.
    [SerializeField] private TMP_Text saveMessageText;


    /// Saves the current game data.
    /// Also plays a button sound and updates the UI message.

    public void SaveGame()
    {
        // Plays the button click sound effect.
        AudioManager.Instance.PlayButtonClick();

        // Calls GameManager to save all current player data.
        GameManager.Instance.SaveGame();

        // Makes sure the save message text exists before changing it.
        if (saveMessageText != null)
        {
            // Displays confirmation that the save worked.
            saveMessageText.text = "Game Saved!";
        }
    }
}
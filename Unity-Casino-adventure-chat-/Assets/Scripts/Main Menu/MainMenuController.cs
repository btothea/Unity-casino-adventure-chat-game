using UnityEngine;
using UnityEngine.SceneManagement;

/// Controls the Main Menu scene.
/// Handles starting a new game, loading a saved game,
/// opening the rules panel, closing the rules panel,
/// music, button sounds, and quitting the game.
public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]

    // First gameplay scene loaded when starting a new game.
    [SerializeField] private string gameplaySceneName = "EntranceLobby";

    [Header("UI References")]

    // Rules panel that explains how the game works.
    [SerializeField] private GameObject rulesPanel;

    private void Start()
    {
        // Plays main menu music if the AudioManager exists.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }

        // Makes sure the rules panel starts hidden.
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(false);
        }
    }

    /// Starts a brand new game.
    /// This resets save data and loads the first gameplay scene.
    public void PlayGame()
    {
        // Plays button click sound if AudioManager exists.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // Resets GameData and deletes the old save.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewGame();
        }

        // Loads the first scene of the game.
        SceneManager.LoadScene(gameplaySceneName);
    }

    /// Loads the player's saved game.
    /// If a save file exists, it loads the saved scene.
    public void LoadGame()
    {
        // Plays button click sound if AudioManager exists.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // Tries to load saved GameData from the save file.
        if (SaveManager.TryLoad(out GameData loadedData))
        {
            // Sends the loaded data into GameManager.
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadGameData(loadedData);
            }

            // Loads the scene the player saved in.
            SceneManager.LoadScene(loadedData.currentSceneName);
        }
        else
        {
            // Shows this message when no save file exists.
            Debug.Log("No save file found.");
        }
    }

    /// Opens the rules panel.
    public void OpenRules()
    {
        // Plays button click sound if AudioManager exists.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // Shows the rules panel.
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(true);
        }
    }

    /// Closes the rules panel.
    public void CloseRules()
    {
        // Plays button click sound if AudioManager exists.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // Hides the rules panel.
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(false);
        }
    }

    /// Quits the game.
    /// This works in a built game.
    /// In the Unity Editor, it will not fully close Play Mode.
    public void QuitGame()
    {
        // Plays button click sound if AudioManager exists.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // Closes the game application.
        Application.Quit();
    }
}
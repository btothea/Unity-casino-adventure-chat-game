using UnityEngine;
using UnityEngine.SceneManagement;


/// Controls the Ending Scene.
/// Handles returning to the Main Menu
/// and quitting the game.

public class EndingSceneController : MonoBehaviour
{
    [Header("Scene Names")]

    // Name of the Main Menu scene.
    // This scene loads when the player presses the return button.
    [SerializeField] private string mainMenuScene = "MainMenu";

    /// Sends the player back to the Main Menu scene.

    public void ReturnToMenu()
    {
        // Loads the Main Menu scene.
        SceneManager.LoadScene(mainMenuScene);
    }


    /// Closes the game application.
    /// This only fully works in a built version of the game.
    /// In the Unity Editor it will only print the debug message.

    public void QuitGame()
    {
        // Debug message so you can tell the button worked while testing in Unity.
        Debug.Log("Quit Game pressed.");

        // Closes the game application.
        Application.Quit();
    }
}
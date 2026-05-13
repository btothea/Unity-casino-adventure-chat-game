using UnityEngine;
using UnityEngine.SceneManagement;


/// Main manager for handling player save data.
/// Keeps track of the current GameData,
/// saving/loading, and scene progress.

public class GameManager : MonoBehaviour
{
    // Static instance allows global access to the GameManager.
    // Example:
    // GameManager.Instance.Data.moneyAmount += 10;
    public static GameManager Instance;

    // Holds all player save data.
    // Private set means other scripts can read the data,
    // but only GameManager can replace the whole object.
    public GameData Data { get; private set; }

    private void Awake()
    {
        // Makes sure only one GameManager exists.
        // Prevents duplicates when scenes change.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Sets this object as the main GameManager instance.
        Instance = this;

        // Keeps the GameManager alive between scenes.
        DontDestroyOnLoad(gameObject);

        // Creates new GameData if none exists yet.
        // Prevents null reference errors.
        if (Data == null)
        {
            Data = new GameData();
        }
    }


    /// Starts a completely new game.
    /// Resets all save data back to default values.
    /// Also deletes the existing save file.

    public void StartNewGame()
    {
        // Creates fresh default save data.
        Data = new GameData();

        // Deletes the old save file from disk.
        SaveManager.DeleteSave();
    }

    /// Loads existing save data into the GameManager.
    /// Usually called after using SaveManager.Load().
  
    public void LoadGameData(GameData loadedData)
    {
        Data = loadedData;
    }

    
    /// Saves the current active scene name into GameData.
    /// This allows the game to reload the player
    /// into the correct scene after loading a save.
    
    public void SaveCurrentScene()
    {
        Data.currentSceneName = SceneManager.GetActiveScene().name;
    }

   
    /// Saves all current game data.
    /// First stores the active scene name,
    /// then sends the data to SaveManager.
  
    public void SaveGame()
    {
        // Updates the current scene before saving.
        SaveCurrentScene();

        // Sends the GameData object to SaveManager for JSON saving.
        SaveManager.Save(Data);
    }
}
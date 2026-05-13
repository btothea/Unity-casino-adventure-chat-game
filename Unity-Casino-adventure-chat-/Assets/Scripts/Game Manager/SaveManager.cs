using System.IO;
using UnityEngine;


/// Handles saving, loading,
/// checking, and deleting save files.
/// Uses JSON to store GameData.

public static class SaveManager
{

    /// Full file path for the save file.
    /// Application.persistentDataPath points to
    /// Unity's safe save folder on the player's computer.

    private static string PathToFile =>
        Path.Combine(Application.persistentDataPath, "save.json");

    /// Checks if a save file already exists.
    /// Returns true if found, false if not.

    public static bool SaveExists()
    {
        return File.Exists(PathToFile);
    }


    /// Saves the current GameData to a JSON file.
 
    public static void Save(GameData data)
    {
        // Converts GameData into JSON format.
        // "true" makes the JSON easier to read with formatting.
        string json = JsonUtility.ToJson(data, true);

        // Writes the JSON data into the save file.
        File.WriteAllText(PathToFile, json);

        // Debug message so you can see where the save file was created.
        Debug.Log("Game saved to: " + PathToFile);
    }


    /// Attempts to load save data from the JSON file.
    /// Returns true if loading worked.
    /// Returns false if no save exists or loading failed.
 
    public static bool TryLoad(out GameData data)
    {
        try
        {
            // If the save file does not exist,
            // create fresh default GameData instead.
            if (!File.Exists(PathToFile))
            {
                data = new GameData();

                return false;
            }

            // Reads all JSON text from the save file.
            string json = File.ReadAllText(PathToFile);

            // Converts JSON back into a GameData object.
            data = JsonUtility.FromJson<GameData>(json);

            return true;
        }
        catch (System.Exception e)
        {
            // If loading fails for any reason,
            // print the error message to the console.
            Debug.LogError("Load failed: " + e.Message);

            // Creates fresh GameData so the game does not crash.
            data = new GameData();

            return false;
        }
    }

 
    /// Deletes the current save file if one exists.
    /// Used when starting a completely new game.
 
    public static void DeleteSave()
    {
        // Makes sure the file exists before trying to delete it.
        if (File.Exists(PathToFile))
        {
            File.Delete(PathToFile);
        }
    }
}
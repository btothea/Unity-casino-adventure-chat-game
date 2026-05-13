[System.Serializable]
public class GameData
{
    // Player money
    public int moneyAmount = 0;

    // Story flags
    public bool tookRisk = false;
    public bool hasVIPToken = false;
    public bool knowsSecret = false;
    public bool helpedGambler = false;
    public bool firstBetComplete = false;

    // Key items
    public bool hasGoldenCard = false;
    public bool hasHouseLedger = false;

    // Save current room name
    public string currentRoomName = "Entrance Lobby";

    // Save current scene name
    public string currentSceneName = "EntranceLobby";
}
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


/// Controls the High Roller Table scene.
/// Handles the final gamble, dialogue,
/// ending selection, scene transitions,
/// and player state UI.

public class HighRollerTableController : MonoBehaviour
{
    [Header("Scriptable Objects")]

    // Holds the room name and description data.
    [SerializeField] private RoomData currentRoom;

    // NPC data for the High Roller Dealer.
    [SerializeField] private NPCData highRollerDealer;

    [Header("Room UI")]

    // Displays the room title.
    [SerializeField] private TMP_Text roomTitleText;

    // Displays the room description.
    [SerializeField] private TMP_Text roomDescriptionText;

    [Header("Dialogue UI")]

    // Dialogue panel shown during conversations.
    [SerializeField] private GameObject dialoguePanel;

    // Main dialogue text box.
    [SerializeField] private TMP_Text dialogueText;

    // Optional typewriter effect for dialogue.
    [SerializeField] private TypewriterEffect typewriter;

    [Header("Choice Buttons")]

    // Button used to spin the House Wheel.
    [SerializeField] private GameObject spinWheelButton;

    // Button used to finish the game and trigger an ending.
    [SerializeField] private GameObject endGameButton;

    // Button used to return to the casino floor.
    [SerializeField] private GameObject returnButton;

    // Extra unused button hidden for now.
    [SerializeField] private GameObject unusedButton;

    [Header("State UI")]

    // Displays player money.
    [SerializeField] private TMP_Text moneyText;

    // Displays if the player knows the casino secret.
    [SerializeField] private TMP_Text knowsSecretText;

    // Displays if the player owns the VIP Token.
    [SerializeField] private TMP_Text vipTokenText;

    // Displays if the player helped the gambler.
    [SerializeField] private TMP_Text helpedGamblerText;

    // Displays if the player owns the Golden Card.
    [SerializeField] private TMP_Text goldenCardText;

    // Displays if the player owns the House Ledger.
    [SerializeField] private TMP_Text houseLedgerText;

    [Header("Scene Names")]

    // Scene used to return to the Main Casino Floor.
    [SerializeField] private string returnScene = "MainCasinoFloor";

    // Ending scene for becoming rich.
    [SerializeField] private string bigWinnerScene = "Ending_BigWinner";

    // Secret ending scene for escaping the casino system.
    [SerializeField] private string escapeSystemScene = "Ending_EscapeSystem";

    // Ending scene for losing everything.
    [SerializeField] private string lostEverythingScene = "Ending_LostEverything";

    private void Start()
    {
        // Starts the High Roller music.
        AudioManager.Instance.PlayHighRollerMusic();

        // Loads room information from the RoomData ScriptableObject.
        if (currentRoom != null)
        {
            roomTitleText.text = currentRoom.roomName;
            roomDescriptionText.text = currentRoom.description;

            // Saves the current room name into GameData.
            GameManager.Instance.Data.currentRoomName = currentRoom.roomName;
        }

        // Dialogue panel starts hidden.
        dialoguePanel.SetActive(false);

        // Buttons start hidden until the player talks to the dealer.
        spinWheelButton.SetActive(false);
        endGameButton.SetActive(false);
        returnButton.SetActive(false);

        // Hides unused button if it exists.
        if (unusedButton != null)
        {
            unusedButton.SetActive(false);
        }

        // Updates the player state panel at scene start.
        UpdateStatePanel();
    }


    /// Opens the High Roller Dealer dialogue.
    /// Reveals the final gamble options.

    public void TalkToHighRollerDealer()
    {
        AudioManager.Instance.PlayButtonClick();

        // Opens the dialogue panel.
        dialoguePanel.SetActive(true);

        ShowDialogue(
            "High Roller Dealer: This is it. One final game. Everything you have... or nothing at all.\n\n" +
            "Spin the House Wheel to risk your money, or end the game now and face your result.");

        // Shows the action buttons.
        spinWheelButton.SetActive(true);
        endGameButton.SetActive(true);
        returnButton.SetActive(true);

        // Keeps the unused button hidden.
        if (unusedButton != null)
        {
            unusedButton.SetActive(false);
        }
    }

   
    /// Spins the House Wheel.
    /// The player can lose all money
    /// or win different reward amounts.
 
    public void SpinHouseWheel()
    {
        AudioManager.Instance.PlayButtonClick();

        // Marks that the player took the final risk.
        GameManager.Instance.Data.tookRisk = true;

        // Random number between 1 and 6.
        int roll = Random.Range(1, 7);

        // -------------------------
        // Lose Everything Outcome
        // -------------------------
        if (roll <= 2)
        {
            // Player loses all money.
            GameManager.Instance.Data.moneyAmount = 0;

            AudioManager.Instance.PlayLoseSound();

            ShowDialogue(
                "The House Wheel lands on ruin.\n\n" +
                "High Roller Dealer: The house takes everything.");
        }

        // -------------------------
        // Small Win Outcome
        // -------------------------
        else if (roll <= 4)
        {
            // Player gains $100.
            GameManager.Instance.Data.moneyAmount += 100;

            AudioManager.Instance.PlayWinSound();

            ShowDialogue(
                "The House Wheel lands on profit.\n\n" +
                "You gain $100.");
        }

        // -------------------------
        // Medium Win Outcome
        // -------------------------
        else if (roll == 5)
        {
            // Player gains $250.
            GameManager.Instance.Data.moneyAmount += 250;

            AudioManager.Instance.PlayWinSound();

            ShowDialogue(
                "The House Wheel lands on high stakes.\n\n" +
                "You gain $250.");
        }

        // -------------------------
        // Jackpot Outcome
        // -------------------------
        else
        {
            // Player gains $500.
            GameManager.Instance.Data.moneyAmount += 500;

            AudioManager.Instance.PlayWinSound();

            ShowDialogue(
                "The House Wheel lands on jackpot.\n\n" +
                "You gain $500.");
        }

        // Saves the updated game data.
        GameManager.Instance.SaveGame();

        // Updates the player state UI.
        UpdateStatePanel();
    }

   
    /// Determines which ending the player receives.
    /// Different endings depend on money,
    /// story choices, and collected items.

    public void EndGame()
    {
        AudioManager.Instance.PlayButtonClick();

        // Saves before loading the ending scene.
        GameManager.Instance.SaveGame();

       
        // Escape the System Ending
        // Requires:
        // - Enough money
        // - Knows the secret
        // - Helped the gambler
        // - Golden Card
        // - House Ledger
   
        if (GameManager.Instance.Data.moneyAmount >= 500 &&
            GameManager.Instance.Data.knowsSecret &&
            GameManager.Instance.Data.helpedGambler &&
            GameManager.Instance.Data.hasGoldenCard &&
            GameManager.Instance.Data.hasHouseLedger)
        {
            SceneManager.LoadScene(escapeSystemScene);
        }

      
        // Big Winner Ending
        // Requires:
        // - Enough money
        // - Player took a risk
   
        else if (GameManager.Instance.Data.moneyAmount >= 200 &&
                 GameManager.Instance.Data.tookRisk)
        {
            SceneManager.LoadScene(bigWinnerScene);
        }

      
        // Default Ending
        // Player loses everything.
 
        else
        {
            SceneManager.LoadScene(lostEverythingScene);
        }
    }

   
    /// Returns the player to the Main Casino Floor.
    
    public void ReturnToCasinoFloor()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(returnScene);
    }

   
    /// Displays dialogue using the TypewriterEffect.
    /// Falls back to normal text if the effect is missing.
   
    private void ShowDialogue(string message)
    {
        if (typewriter != null)
        {
            typewriter.ShowText(message);
        }
        else
        {
            dialogueText.text = message;
        }
    }

    
    /// Updates the player state panel UI.
    /// Pulls the latest values directly from GameData.
    
    private void UpdateStatePanel()
    {
        moneyText.text =
            "Money: " + GameManager.Instance.Data.moneyAmount;

        knowsSecretText.text =
            "Knows Secret: " + GameManager.Instance.Data.knowsSecret;

        vipTokenText.text =
            "VIP Token: " + GameManager.Instance.Data.hasVIPToken;

        helpedGamblerText.text =
            "Helped Gambler: " + GameManager.Instance.Data.helpedGambler;

        goldenCardText.text =
            "Golden Card: " + GameManager.Instance.Data.hasGoldenCard;

        // Only updates House Ledger text if it exists.
        if (houseLedgerText != null)
        {
            houseLedgerText.text =
                "House Ledger: " + GameManager.Instance.Data.hasHouseLedger;
        }
    }
}
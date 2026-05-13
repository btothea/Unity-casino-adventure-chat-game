using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// Controls the Main Casino Floor scene.
/// Handles room navigation, Dealer choices,
/// Gatekeeper checks, VIP Token unlocking,
/// dialogue, audio, and player state UI.

public class MainCasinoFloorController : MonoBehaviour
{
    [Header("Room Data")]

    // Holds the room name and description.
    [SerializeField] private RoomData currentRoom;

    [Header("Dialogue Data")]

    // Dialogue ScriptableObject for the Dealer.
    [SerializeField] private DialogueData dealerDialogue;

    // Dialogue ScriptableObject for the Gatekeeper.
    [SerializeField] private DialogueData gatekeeperDialogue;

    [Header("Room UI")]

    // Displays the room title.
    [SerializeField] private TMP_Text roomTitleText;

    // Displays the room description.
    [SerializeField] private TMP_Text roomDescriptionText;

    [Header("Direction Buttons")]

    // North button leading to the VIP Room.
    [SerializeField] private Button northButton;

    // Color shown when the VIP Room is locked.
    [SerializeField] private Color lockedColor = Color.red;

    // Color shown when the VIP Room is unlocked.
    [SerializeField] private Color unlockedColor = Color.green;

    [Header("Dealer Panel")]

    // Main Dealer dialogue panel.
    [SerializeField] private GameObject dealerPanel;

    // Dealer dialogue text box.
    [SerializeField] private TMP_Text dealerDialogueText;

    // Typewriter effect for Dealer dialogue.
    [SerializeField] private TypewriterEffect dealerTypewriter;

    // Button to start the free introduction game.
    [SerializeField] private GameObject freeGameButton;

    // Button for safe betting.
    [SerializeField] private GameObject safeBetButton;

    // Button for risky betting.
    [SerializeField] private GameObject bigBetButton;

    // Extra blackjack-style button currently unused.
    [SerializeField] private GameObject hitButton;

    // Extra blackjack-style button currently unused.
    [SerializeField] private GameObject standButton;

    // Button to leave Dealer dialogue.
    [SerializeField] private GameObject leaveDealerButton;

    [Header("Gatekeeper Panel")]

    // Gatekeeper dialogue panel.
    [SerializeField] private GameObject gatekeeperPanel;

    // Gatekeeper dialogue text box.
    [SerializeField] private TMP_Text gatekeeperDialogueText;

    // Typewriter effect for Gatekeeper dialogue.
    [SerializeField] private TypewriterEffect gatekeeperTypewriter;

    [Header("Item Visual")]

    // Visual object for the VIP Token.
    [SerializeField] private GameObject vipTokenObject;

    [Header("State UI")]

    // Displays player money.
    [SerializeField] private TMP_Text moneyText;

    // Displays if player knows the casino secret.
    [SerializeField] private TMP_Text knowsSecretText;

    // Displays if player owns the VIP Token.
    [SerializeField] private TMP_Text vipTokenText;

    // Displays if player helped the gambler.
    [SerializeField] private TMP_Text helpedGamblerText;

    // Displays if player owns the Golden Card.
    [SerializeField] private TMP_Text goldenCardText;

    [Header("Scene Names")]

    // Scene north of the casino floor.
    [SerializeField] private string northScene = "VIPRoom";

    // Scene south of the casino floor.
    [SerializeField] private string southScene = "EntranceLobby";

    // Scene east of the casino floor.
    [SerializeField] private string eastScene = "BackOffice";

    // Scene west of the casino floor.
    [SerializeField] private string westScene = "HighRollerTable";

    private void Start()
    {
        // Starts casino music when scene loads.
        AudioManager.Instance.PlayCasinoMusic();

        // Loads room information from the RoomData ScriptableObject.
        if (currentRoom != null)
        {
            roomTitleText.text = currentRoom.roomName;
            roomDescriptionText.text = currentRoom.description;

            // Saves current room name into GameData.
            GameManager.Instance.Data.currentRoomName = currentRoom.roomName;
        }

        // Dialogue panels start hidden.
        dealerPanel.SetActive(false);
        gatekeeperPanel.SetActive(false);

        // Shows VIP Token object if player already owns it.
        if (vipTokenObject != null)
        {
            vipTokenObject.SetActive(GameManager.Instance.Data.hasVIPToken);
        }

        // Updates player state UI.
        UpdateStatePanel();

        // Updates VIP Room lock visuals.
        UpdateNorthButtonLock();
    }

    /// Moves player north into the VIP Room.
    /// If player does not have the VIP Token,
    /// the Gatekeeper dialogue opens instead.
    public void GoNorth()
    {
        AudioManager.Instance.PlayButtonClick();

        if (GameManager.Instance.Data.hasVIPToken)
        {
            SceneManager.LoadScene(northScene);
        }
        else
        {
            TalkToGatekeeper();
        }
    }

    /// Moves player south to the Entrance Lobby.
    public void GoSouth()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(southScene);
    }

    /// Moves player east to the Back Office.
    public void GoEast()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(eastScene);
    }

    /// Moves player west to the High Roller Table.

    public void GoWest()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(westScene);
    }

    /// Opens the Dealer dialogue panel.
    /// Starts the betting introduction dialogue.
    public void TalkToDealer()
    {
        AudioManager.Instance.PlayButtonClick();

        dealerPanel.SetActive(true);
        gatekeeperPanel.SetActive(false);

        ShowDealerDialogue(
            "Dealer: First time in the casino? Everyone starts with a choice... play it safe, or risk it all.");

        ShowStartDealerButtons();
    }

    /// Shows the starting Dealer buttons.
    private void ShowStartDealerButtons()
    {
        freeGameButton.SetActive(true);

        safeBetButton.SetActive(false);
        bigBetButton.SetActive(false);

        hitButton.SetActive(false);
        standButton.SetActive(false);

        leaveDealerButton.SetActive(true);
    }

    /// Shows the betting buttons after the intro.
    private void ShowBetButtons()
    {
        freeGameButton.SetActive(false);

        safeBetButton.SetActive(true);
        bigBetButton.SetActive(true);

        hitButton.SetActive(false);
        standButton.SetActive(false);

        leaveDealerButton.SetActive(true);
    }

    /// Hides Dealer choice buttons after betting is complete.
    private void HideDealerChoiceButtons()
    {
        freeGameButton.SetActive(false);
        safeBetButton.SetActive(false);
        bigBetButton.SetActive(false);

        hitButton.SetActive(false);
        standButton.SetActive(false);

        leaveDealerButton.SetActive(true);
    }

    /// Starts the free practice game.
    /// Marks the player as someone willing to take risks.
    public void StartFreeGame()
    {
        AudioManager.Instance.PlayButtonClick();

        GameManager.Instance.Data.tookRisk = true;

        ShowDealerDialogue(
            "Dealer: Let us start small. A safe bet keeps you alive, but a big bet gets you noticed.");

        ShowBetButtons();

        GameManager.Instance.SaveGame();

        UpdateStatePanel();
    }

    /// Safe betting option.
    /// Gives smaller rewards but less risk.
    public void StartSafeBet()
    {
        AudioManager.Instance.PlayButtonClick();

        // Gives player money.
        GameManager.Instance.Data.moneyAmount += 20;

        // Marks first bet as completed.
        GameManager.Instance.Data.firstBetComplete = true;

        // Unlock VIP if player now has enough money.
        if (GameManager.Instance.Data.moneyAmount >= 200)
        {
            GameManager.Instance.Data.hasVIPToken = true;

            ShowDealerDialogue(
                "Dealer: Careful will not make you rich, but it keeps you standing.\n\n" +
                "Money gained: $20\n\n" +
                "Dealer: You are ready for VIP. Take this token.");

            // Shows VIP Token visual.
            if (vipTokenObject != null)
            {
                vipTokenObject.SetActive(true);
            }

            AudioManager.Instance.PlayItemPickup();
        }
        else
        {
            ShowDealerDialogue(
                "Dealer: Careful will not make you rich, but it keeps you standing.\n\n" +
                "Money gained: $20\n\n" +
                "Dealer: Come back when you are worth something.");
        }

        GameManager.Instance.SaveGame();

        UpdateStatePanel();
        UpdateNorthButtonLock();

        HideDealerChoiceButtons();
    }

    /// Risky betting option.
    /// Gives bigger rewards and marks the player as a risk taker.
    public void StartBigBet()
    {
        AudioManager.Instance.PlayButtonClick();

        // Gives player larger money reward.
        GameManager.Instance.Data.moneyAmount += 100;

        // Marks player as someone willing to gamble.
        GameManager.Instance.Data.tookRisk = true;

        // Marks tutorial bet complete.
        GameManager.Instance.Data.firstBetComplete = true;

        // Unlock VIP if enough money was earned.
        if (GameManager.Instance.Data.moneyAmount >= 200)
        {
            GameManager.Instance.Data.hasVIPToken = true;

            ShowDealerDialogue(
                "Dealer: Now that is a gamble. The house likes courage.\n\n" +
                "Money gained: $100\n\n" +
                "Dealer: You are ready for VIP. Take this token.");

            // Shows VIP Token object.
            if (vipTokenObject != null)
            {
                vipTokenObject.SetActive(true);
            }

            AudioManager.Instance.PlayItemPickup();
        }
        else
        {
            ShowDealerDialogue(
                "Dealer: Now that is a gamble. The house likes courage.\n\n" +
                "Money gained: $100\n\n" +
                "Dealer: Come back when you are worth something.");
        }

        GameManager.Instance.SaveGame();

        UpdateStatePanel();
        UpdateNorthButtonLock();

        HideDealerChoiceButtons();
    }

    /// Closes the Dealer dialogue panel.
    public void LeaveDealer()
    {
        AudioManager.Instance.PlayButtonClick();

        dealerPanel.SetActive(false);
    }

    /// Opens Gatekeeper dialogue.
    /// Dialogue changes depending on player progress.
    public void TalkToGatekeeper()
    {
        AudioManager.Instance.PlayButtonClick();

        gatekeeperPanel.SetActive(true);
        dealerPanel.SetActive(false);

        // Player does not have VIP access yet.
        if (!GameManager.Instance.Data.hasVIPToken)
        {
            ShowGatekeeperDialogue(
                "Gatekeeper: VIPs only. No exceptions.\n\n" +
                "Earn your place. Come back when you are worth something.");
        }

        // Player has VIP access but does not know the casino secret.
        else if (!GameManager.Instance.Data.knowsSecret)
        {
            ShowGatekeeperDialogue(
                "Gatekeeper: ...You may enter.");
        }

        // Player knows the truth about the casino.
        else
        {
            ShowGatekeeperDialogue(
                "Gatekeeper: You should not be here... but the token is real. Go in, and watch your back.");
        }
    }

    /// Allows the player to request the VIP Token directly.
    /// Requires at least $200.
    public void RequestVIPToken()
    {
        AudioManager.Instance.PlayButtonClick();

        // Prevents giving another VIP Token if player already has one.
        if (GameManager.Instance.Data.hasVIPToken)
        {
            ShowGatekeeperDialogue(
                "Gatekeeper: You already have the VIP Token. Go north.");

            UpdateNorthButtonLock();

            return;
        }

        // Gives VIP access if player has enough money.
        if (GameManager.Instance.Data.moneyAmount >= 200)
        {
            GameManager.Instance.Data.hasVIPToken = true;

            ShowGatekeeperDialogue(
                "Gatekeeper: Fine. You have enough money to matter. Take this VIP Token.");

            // Shows VIP Token visual.
            if (vipTokenObject != null)
            {
                vipTokenObject.SetActive(true);
            }

            AudioManager.Instance.PlayItemPickup();

            GameManager.Instance.SaveGame();
        }
        else
        {
            ShowGatekeeperDialogue(
                "Gatekeeper: You need at least $200 before I let you near the VIP Room.");
        }

        UpdateStatePanel();
        UpdateNorthButtonLock();
    }

    /// Closes the Gatekeeper dialogue panel.
    public void LeaveGatekeeper()
    {
        AudioManager.Instance.PlayButtonClick();

        gatekeeperPanel.SetActive(false);
    }

    /// Saves the current game manually from a UI button.
    public void SaveGameButton()
    {
        AudioManager.Instance.PlayButtonClick();

        GameManager.Instance.SaveGame();
    }

    /// Displays Dealer dialogue using the typewriter effect.
    /// Falls back to normal text if typewriter is missing.
    private void ShowDealerDialogue(string message)
    {
        if (dealerTypewriter != null)
        {
            dealerTypewriter.ShowText(message);
        }
        else
        {
            dealerDialogueText.text = message;
        }
    }

    /// Displays Gatekeeper dialogue using the typewriter effect.
    /// Falls back to normal text if typewriter is missing.
    private void ShowGatekeeperDialogue(string message)
    {
        if (gatekeeperTypewriter != null)
        {
            gatekeeperTypewriter.ShowText(message);
        }
        else
        {
            gatekeeperDialogueText.text = message;
        }
    }

    /// Changes the north button color depending on
    /// whether the VIP Room is locked or unlocked.
    private void UpdateNorthButtonLock()
    {
        // Prevents errors if the button was not assigned.
        if (northButton == null)
        {
            Debug.LogWarning("North Button is not assigned.");

            return;
        }

        // Checks if the player owns the VIP Token.
        bool hasVIPToken = GameManager.Instance.Data.hasVIPToken;

        // Gets the Image component from the button.
        Image buttonImage = northButton.GetComponent<Image>();

        // Changes button color based on lock state.
        if (buttonImage != null)
        {
            buttonImage.color =
                hasVIPToken ? unlockedColor : lockedColor;
        }
    }

    /// Updates the player state panel UI.
    /// Pulls values directly from GameData.
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
    }
}
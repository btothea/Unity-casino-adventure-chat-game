using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


/// Controls the Entrance Lobby scene.
/// Handles room text, movement buttons,
/// Host dialogue, audio, and player state UI.

public class EntranceLobbyController : MonoBehaviour
{
    [Header("Room UI")]

    // Displays the room name at the top of the screen.
    [SerializeField] private TMP_Text roomTitleText;

    // Displays the room description for the player.
    [SerializeField] private TMP_Text roomDescriptionText;

    [Header("Dialogue UI")]

    // Dialogue panel that appears when talking to the Host.
    [SerializeField] private GameObject dialoguePanel;

    // Text area used for displaying dialogue.
    [SerializeField] private TMP_Text dialogueText;

    // Optional typewriter effect for animated dialogue text.
    [SerializeField] private TypewriterEffect typewriter;

    [Header("State UI")]

    // Displays the player's current money amount.
    [SerializeField] private TMP_Text moneyText;

    // Displays whether the player knows the casino secret.
    [SerializeField] private TMP_Text knowsSecretText;

    // Displays whether the player owns the VIP Token.
    [SerializeField] private TMP_Text vipTokenText;

    // Displays whether the player helped the gambler.
    [SerializeField] private TMP_Text helpedGamblerText;

    // Displays whether the player owns the Golden Card.
    [SerializeField] private TMP_Text goldenCardText;

    [Header("Scene Names")]

    // Scene loaded when moving north.
    [SerializeField] private string northScene = "MainCasinoFloor";

    // Scene loaded when moving south.
    [SerializeField] private string southScene = "MainMenu";

    // Scene loaded when moving east.
    [SerializeField] private string eastScene = "DealerTable";

    // Scene loaded when moving west.
    [SerializeField] private string westScene = "ShadyGamblerArea";

    private void Start()
    {
        // Starts the lobby background music.
        AudioManager.Instance.PlayLobbyMusic();

        // Sets the room title text.
        roomTitleText.text = "Entrance\nLobby";

        // Sets the room description text.
        roomDescriptionText.text =
            "A quiet lobby lit by chandeliers. A mysterious host welcomes you to Fate's Hand Casino and explains that fortune comes with a price.";

        // Dialogue panel starts hidden until the player talks to the Host.
        dialoguePanel.SetActive(false);

        // Updates the player state UI when the scene starts.
        UpdateStatePanel();
    }


    /// Moves the player north to the Main Casino Floor.

    public void GoNorth()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(northScene);
    }

 
    /// Moves the player south back to the Main Menu.
 
    public void GoSouth()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(southScene);
    }


    /// Moves the player east to the Dealer Table scene.

    public void GoEast()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(eastScene);
    }


    /// Moves the player west to the Shady Gambler area.

    public void GoWest()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(westScene);
    }


    /// Starts the Host introduction dialogue.

    public void TalkToHost()
    {
        AudioManager.Instance.PlayButtonClick();

        // Opens the dialogue panel.
        dialoguePanel.SetActive(true);

        ShowDialogue(
            "Host: Welcome to Fate's Hand Casino. Here, fortune favors the bold, but the house remembers every choice.");
    }


    /// Explains the main rules and progression system of the game.

    public void HostRules()
    {
        AudioManager.Instance.PlayButtonClick();

        ShowDialogue(
            "Host: Choose carefully. Your money, items, and secrets will decide which ending you reach. Some paths only open when you have the right flag or item.");
    }


    /// Explains why people come to the casino.
    /// Adds more world building and mystery to the story.

    public void HostWhyHere()
    {
        AudioManager.Instance.PlayButtonClick();

        ShowDialogue(
            "Host: Everyone comes here wanting something. Money, power, a second chance. The question is what you are willing to lose for it.");
    }

    /// Closes the Host dialogue panel.

    public void HostReady()
    {
        AudioManager.Instance.PlayButtonClick();

        dialoguePanel.SetActive(false);
    }


    /// Displays dialogue using the TypewriterEffect if one exists.
    /// Otherwise it displays the full text instantly.

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


    /// Updates the player state panel.
    /// Pulls all current values directly from the GameManager save data.

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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


/// This script controls everything that happens in the Back Office scene.
/// It handles the room text, Shady Gambler dialogue, player choices,
/// Golden Card reward, saving, music, and the state panel UI.

public class BackofficeController : MonoBehaviour
{
    [Header("Scriptable Objects")]
    // RoomData holds the name and description for this room.
    [SerializeField] private RoomData currentRoom;

    // NPCData for the Shady Gambler. This is here so the scene can reference that NPC.
    [SerializeField] private NPCData shadyGambler;

    [Header("Room UI")]
    // Text that shows the room name at the top of the scene.
    [SerializeField] private TMP_Text roomTitleText;

    // Text that shows the main room description.
    [SerializeField] private TMP_Text roomDescriptionText;

    [Header("Dialogue UI")]
    // The panel that appears when the player talks to the Shady Gambler.
    [SerializeField] private GameObject dialoguePanel;

    // Text box used for the dialogue lines.
    [SerializeField] private TMP_Text dialogueText;

    // Typewriter effect makes the dialogue appear like it is being typed out.
    [SerializeField] private TypewriterEffect typewriter;

    [Header("Choice Buttons")]
    // First dialogue choice button.
    [SerializeField] private Button choiceButton1;

    // Text shown on the first choice button.
    [SerializeField] private TMP_Text choiceButton1Text;

    // Second dialogue choice button.
    [SerializeField] private Button choiceButton2;

    // Text shown on the second choice button.
    [SerializeField] private TMP_Text choiceButton2Text;

    [Header("Item Visual")]
    // The Golden Card object in the room.
    // This turns on when the player earns or already has the Golden Card.
    [SerializeField] private GameObject goldenCardObject;

    [Header("State UI")]
    // These text fields show the player's current saved data on screen.
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text knowsSecretText;
    [SerializeField] private TMP_Text vipTokenText;
    [SerializeField] private TMP_Text helpedGamblerText;
    [SerializeField] private TMP_Text goldenCardText;

    [Header("Scene Names")]
    // Scene that loads when the player leaves the Back Office going east.
    [SerializeField] private string eastScene = "MainCasinoFloor";

    // Keeps track of what part of the dialogue tree the player is currently on.
    // 0 = start of conversation
    // 1 = gambler explains the house is rigged
    // 2 = gambler asks the player to help
    private int dialogueState = 0;

    private void Start()
    {
        // Starts the Back Office music when this scene loads.
        AudioManager.Instance.PlayBackOfficeMusic();

        // Loads the room name and description from the RoomData ScriptableObject.
        if (currentRoom != null)
        {
            roomTitleText.text = currentRoom.roomName;
            roomDescriptionText.text = currentRoom.description;

            // Saves the current room name so the game knows where the player is.
            GameManager.Instance.Data.currentRoomName = currentRoom.roomName;
        }

        // Clears old button listeners first so the buttons do not stack extra clicks.
        choiceButton1.onClick.RemoveAllListeners();
        choiceButton1.onClick.AddListener(PressChoice1);

        choiceButton2.onClick.RemoveAllListeners();
        choiceButton2.onClick.AddListener(PressChoice2);

        // If the player already has the Golden Card, show it in the room.
        if (goldenCardObject != null)
        {
            goldenCardObject.SetActive(GameManager.Instance.Data.hasGoldenCard);
        }

        // Dialogue starts hidden until the player talks to the gambler.
        dialoguePanel.SetActive(false);

        // Choice buttons also start hidden.
        HideChoices();

        // Updates the player state panel when the scene starts.
        UpdateStatePanel();
    }

    
    /// Sends the player back to the Main Casino Floor.
    
    public void GoEast()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(eastScene);
    }

    
    /// Starts the Shady Gambler conversation.
    /// This opens the dialogue panel and shows the first two choices.
    
    public void TalkToGambler()
    {
        AudioManager.Instance.PlayButtonClick();

        dialoguePanel.SetActive(true);

        // Reset dialogue back to the beginning every time the player starts talking.
        dialogueState = 0;

        ShowDialogue(
            "Shady Gambler: You look new. This place is not what you think it is.");

        choiceButton1Text.text = "What do you mean?";
        choiceButton2Text.text = "I'm not interested.";

        ShowChoices();
    }

   
    /// Handles what happens when the player presses the first dialogue choice.
    /// The result changes based on the current dialogue state.

    private void PressChoice1()
    {
        AudioManager.Instance.PlayButtonClick();

        // First branch: player asks what the gambler means.
        if (dialogueState == 0)
        {
            dialogueState = 1;

            ShowDialogue(
                "Shady Gambler: The house always wins... unless you break the rules.");

            choiceButton1Text.text = "Help me understand.";
            choiceButton2Text.text = "Sounds crazy.";
        }

        // Second branch: player listens and learns the secret.
        else if (dialogueState == 1)
        {
            dialogueState = 2;

            // This flag is saved because it can affect future scenes or endings.
            GameManager.Instance.Data.knowsSecret = true;

            ShowDialogue(
                "Shady Gambler: The games are not random. They are watching what you are willing to lose.\n\n" +
                "Shady Gambler: I can show you the way out, but I need you to trust me.");

            choiceButton1Text.text = "I'll help you.";
            choiceButton2Text.text = "I'm on my own.";

            // Save after changing important player data.
            GameManager.Instance.SaveGame();

            UpdateStatePanel();
        }

        // Third branch: player helps the gambler and earns the Golden Card.
        else if (dialogueState == 2)
        {
            GameManager.Instance.Data.helpedGambler = true;
            GameManager.Instance.Data.hasGoldenCard = true;

            ShowDialogue(
                "Shady Gambler: Good. Take this Golden Card. The VIP Room is not the reward. The High Roller Table is where the truth is hidden.");

            // Turns on the Golden Card visual in the room.
            if (goldenCardObject != null)
            {
                goldenCardObject.SetActive(true);
            }

            AudioManager.Instance.PlayItemPickup();

            // Save the reward and updated flags.
            GameManager.Instance.SaveGame();

            UpdateStatePanel();

            // Conversation is finished, so hide the choices.
            HideChoices();
        }
    }


    /// Handles what happens when the player presses the second dialogue choice.
    /// This is mostly the refusal path for the conversation.

    private void PressChoice2()
    {
        AudioManager.Instance.PlayButtonClick();

        // Player refuses to listen at the start.
        if (dialogueState == 0)
        {
            ShowDialogue(
                "Shady Gambler: That is what everyone says before the house takes everything.");

            HideChoices();
        }

        // Player rejects the gambler's warning.
        else if (dialogueState == 1)
        {
            ShowDialogue(
                "Shady Gambler: Crazy is thinking you can beat a rigged table by playing fair.");

            HideChoices();
        }

        // Player refuses to help the gambler.
        else if (dialogueState == 2)
        {
            ShowDialogue(
                "Shady Gambler: Then you will lose like the rest of us.");

            HideChoices();
        }
    }


    /// Shows both dialogue choice buttons.

    private void ShowChoices()
    {
        choiceButton1.gameObject.SetActive(true);
        choiceButton2.gameObject.SetActive(true);
    }


    /// Hides both dialogue choice buttons.
    /// This is used when the conversation reaches an ending point.

    private void HideChoices()
    {
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
    }


    /// Displays dialogue on screen.
    /// If the TypewriterEffect exists, it uses that.
    /// If not, it just shows the full text normally.

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


    /// Updates the state panel so the player can see their current saved values.
    /// This pulls directly from GameManager.Instance.Data.

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
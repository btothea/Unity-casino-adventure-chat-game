using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// Controls the VIP Room scene.
/// Lets the player inspect the room, collect the House Ledger,
/// save progress, update the state panel,
/// and return to the Main Casino Floor.
public class VIPRoomController : MonoBehaviour
{
    [Header("Scriptable Objects")]

    // Holds the room name and description.
    [SerializeField] private RoomData currentRoom;

    // ItemData for the House Ledger.
    // This lets the script pull the item name from a ScriptableObject.
    [SerializeField] private ItemData houseLedgerItem;

    [Header("Room UI")]

    // Displays the room title.
    [SerializeField] private TMP_Text roomTitleText;

    // Displays the room description.
    [SerializeField] private TMP_Text roomDescriptionText;

    [Header("Dialogue UI")]

    // Dialogue panel used for VIP Room text.
    [SerializeField] private GameObject dialoguePanel;

    // Text box where the dialogue appears.
    [SerializeField] private TMP_Text dialogueText;

    // Optional typewriter effect for dialogue.
    [SerializeField] private TypewriterEffect typewriter;

    [Header("Buttons")]

    // Button used to inspect the room.
    [SerializeField] private GameObject inspectRoomButton;

    // Button used to collect the House Ledger after inspecting.
    [SerializeField] private GameObject takeLedgerButton;

    // Button used to return to the casino floor.
    [SerializeField] private GameObject returnButton;

    [Header("Item Visual")]

    // Visual object for the key/ledger pickup in the VIP Room.
    [SerializeField] private GameObject keyObject;

    [Header("State UI")]

    // Displays player money.
    [SerializeField] private TMP_Text moneyText;

    // Displays if player knows the casino secret.
    [SerializeField] private TMP_Text knowsSecretText;

    // Displays if player has the VIP Token.
    [SerializeField] private TMP_Text vipTokenText;

    // Displays if player helped the gambler.
    [SerializeField] private TMP_Text helpedGamblerText;

    // Displays if player owns the Golden Card.
    [SerializeField] private TMP_Text goldenCardText;

    // Displays if player owns the House Ledger.
    [SerializeField] private TMP_Text houseLedgerText;

    [Header("Scene Names")]

    // Scene loaded when returning to the Main Casino Floor.
    [SerializeField] private string returnScene = "MainCasinoFloor";

    private void Start()
    {
        // Starts the VIP Room music.
        AudioManager.Instance.PlayVIPMusic();

        // Loads room info from the RoomData ScriptableObject.
        if (currentRoom != null)
        {
            roomTitleText.text = currentRoom.roomName;
            roomDescriptionText.text = currentRoom.description;

            // Saves the current room name into GameData.
            GameManager.Instance.Data.currentRoomName = currentRoom.roomName;
        }

        // Dialogue starts open so the player immediately gets room information.
        dialoguePanel.SetActive(true);

        // If the player already has the House Ledger,
        // do not let them collect it again.
        if (GameManager.Instance.Data.hasHouseLedger)
        {
            ShowDialogue(
                "The House Ledger is already in your possession. The casino's secret is no longer hidden.");

            inspectRoomButton.SetActive(false);
            takeLedgerButton.SetActive(false);

            // Shows the item visual if it exists.
            if (keyObject != null)
            {
                keyObject.SetActive(true);
            }
        }
        else
        {
            // Starting dialogue when the ledger has not been collected yet.
            ShowDialogue(
                "The VIP Room is quiet, almost too quiet. Something important is hidden here.");

            // Player must inspect the room before taking the ledger.
            inspectRoomButton.SetActive(true);
            takeLedgerButton.SetActive(false);

            // Hides the item visual until the player finds it.
            if (keyObject != null)
            {
                keyObject.SetActive(false);
            }
        }

        // Return button should always be available.
        returnButton.SetActive(true);

        // Updates player state UI.
        UpdateStatePanel();
    }

    /// Lets the player inspect the VIP Room.
    /// Reveals the House Ledger and unlocks the take button.
    public void InspectRoom()
    {
        AudioManager.Instance.PlayButtonClick();

        // Default item name in case the ScriptableObject is not assigned.
        string itemName = "House Ledger";

        // Uses the ItemData name if the item exists.
        if (houseLedgerItem != null)
        {
            itemName = houseLedgerItem.itemName;
        }

        ShowDialogue(
            "You inspect the VIP Room and find the " + itemName +
            ". It proves the casino has been controlling every game.");

        // Player already inspected, so hide inspect button.
        inspectRoomButton.SetActive(false);

        // Now the player is allowed to take the ledger.
        takeLedgerButton.SetActive(true);
    }

    /// Gives the player the House Ledger.
    /// Saves the item into GameData and updates the UI.
    public void TakeHouseLedger()
    {
        AudioManager.Instance.PlayButtonClick();
        AudioManager.Instance.PlayItemPickup();

        // Saves that the player now owns the House Ledger.
        GameManager.Instance.Data.hasHouseLedger = true;

        ShowDialogue(
            "You take the House Ledger. This evidence may help you expose the casino at the High Roller Table.");

        // Saves progress after collecting the important item.
        GameManager.Instance.SaveGame();

        // Hide the take button so the item cannot be collected twice.
        takeLedgerButton.SetActive(false);

        // Shows the item visual after collecting it.
        if (keyObject != null)
        {
            keyObject.SetActive(true);
        }

        // Refreshes the state panel.
        UpdateStatePanel();
    }

    /// Returns the player to the Main Casino Floor.
    public void ReturnToCasinoFloor()
    {
        AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(returnScene);
    }

    /// Displays dialogue using the typewriter effect.
    /// If there is no typewriter effect, it shows the full text normally.
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
    /// Pulls current values straight from GameData.
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

        // Only updates House Ledger text if it was assigned in the Inspector.
        if (houseLedgerText != null)
        {
            houseLedgerText.text =
                "House Ledger: " + GameManager.Instance.Data.hasHouseLedger;
        }
    }
}
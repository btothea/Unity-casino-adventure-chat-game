using System.Collections;
using TMPro;
using UnityEngine;


/// Creates a typewriter effect for dialogue text.
/// Displays dialogue one character at a time
/// instead of showing the full message instantly.

/// Attach this script to the same object as the dialogue UI,
/// then assign the TMP text box in the Inspector.

public class TypewriterEffect : MonoBehaviour
{
    // Text box where the dialogue will appear.
    [SerializeField] private TMP_Text textBox;

    // Controls how fast each character appears.
    // Lower values = faster typing.
    [SerializeField] private float typingSpeed = 0.02f;

    // Stores the currently running typing coroutine.
    // Used so old dialogue can be stopped before new dialogue starts.
    private Coroutine typingCoroutine;


    /// Starts displaying dialogue using the typewriter effect.

    public void ShowText(string message)
    {
        // Prevents errors if the text box reference is missing.
        if (textBox == null)
        {
            Debug.LogError("TypewriterEffect is missing its Text Box reference.");
            return;
        }

        // Stops the previous typing coroutine if one is already running.
        // Prevents overlapping dialogue text.
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Starts typing the new message.
        typingCoroutine = StartCoroutine(TypeText(message));
    }


    /// Types the message one character at a time.
    /// Uses a coroutine so the text appears gradually over time.

    private IEnumerator TypeText(string message)
    {
        // Clears the old dialogue before typing the new one.
        textBox.text = "";

        // Loops through every character in the message.
        foreach (char letter in message)
        {
            // Adds one character at a time to the text box.
            textBox.text += letter;

            // Waits before typing the next character.
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}

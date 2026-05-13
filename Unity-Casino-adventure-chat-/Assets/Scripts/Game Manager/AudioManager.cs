using UnityEngine;


/// Handles all game audio.
/// Controls background music, sound effects,
/// and keeps music playing between scenes.

public class AudioManager : MonoBehaviour
{
    // Static Instance lets other scripts access AudioManager from anywhere.
    // AudioManager.Instance.PlayButtonClick();
    public static AudioManager Instance;

    [Header("Audio Sources")]

    // AudioSource used for background music.
    [SerializeField] private AudioSource musicSource;

    // AudioSource used for sound effects.
    [SerializeField] private AudioSource sfxSource;

    [Header("SFX")]

    // Sound played when pressing buttons.
    [SerializeField] private AudioClip buttonClick;

    // Sound played when picking up important items.
    [SerializeField] private AudioClip itemPickup;

    // Sound played when the player wins.
    [SerializeField] private AudioClip winSound;

    // Sound played when the player loses.
    [SerializeField] private AudioClip loseSound;

    [Header("Music")]

    // Background music for the lobby.
    [SerializeField] private AudioClip lobbyMusic;

    // Background music for the casino floor.
    [SerializeField] private AudioClip casinoMusic;

    // Background music for the Back Office.
    [SerializeField] private AudioClip backOfficeMusic;

    // Background music for the VIP room.
    [SerializeField] private AudioClip vipMusic;

    // Background music for the High Roller room.
    [SerializeField] private AudioClip highRollerMusic;

    // Background music for ending scenes.
    [SerializeField] private AudioClip endingMusic;

    // Background music for the Main Menu.
    [SerializeField] private AudioClip menuMusic;

    private void Awake()
    {
        // Makes sure only one AudioManager exists in the game.
        // Prevents duplicates when loading scenes.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Sets this object as the global AudioManager instance.
        Instance = this;

        // Prevents the AudioManager from being destroyed between scenes.
        DontDestroyOnLoad(gameObject);
    }


    /// Plays the Main Menu music.

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }


    /// Plays background music.
    /// Also prevents restarting the same song if it is already playing.

    public void PlayMusic(AudioClip musicClip)
    {
        // Stops errors if something is missing.
        if (musicClip == null || musicSource == null)
        {
            Debug.LogWarning("Missing music clip or music source.");
            return;
        }

        // Prevents restarting the same music repeatedly.
        if (musicSource.clip == musicClip && musicSource.isPlaying)
        {
            return;
        }

        // Sets the new music clip.
        musicSource.clip = musicClip;

        // Music loops forever until changed.
        musicSource.loop = true;

        // Starts playing the music.
        musicSource.Play();

        Debug.Log("Now playing music: " + musicClip.name);
    }


    /// Plays the button click sound effect.

    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }


    /// Plays the item pickup sound effect.

    public void PlayItemPickup()
    {
        PlaySFX(itemPickup);
    }


    /// Plays the win sound effect.

    public void PlayWinSound()
    {
        PlaySFX(winSound);
    }


    /// Plays the lose sound effect.
   
    public void PlayLoseSound()
    {
        PlaySFX(loseSound);
    }


    /// Plays a sound effect using PlayOneShot.
    /// PlayOneShot allows multiple sounds to overlap without cutting each other off.

    public void PlaySFX(AudioClip clip)
    {
        // Prevents errors if the clip or source is missing.
        if (clip == null || sfxSource == null)
        {
            return;
        }

        // Plays the sound effect one time.
        sfxSource.PlayOneShot(clip);
    }


    /// Plays the Entrance Lobby music.

    public void PlayLobbyMusic()
    {
        PlayMusic(lobbyMusic);
    }


    /// Plays the Main Casino Floor music.
 
    public void PlayCasinoMusic()
    {
        PlayMusic(casinoMusic);
    }


    /// Plays the Back Office music.

    public void PlayBackOfficeMusic()
    {
        PlayMusic(backOfficeMusic);
    }

    /// Plays the VIP Room music.
 
    public void PlayVIPMusic()
    {
        PlayMusic(vipMusic);
    }

    
    /// Plays the High Roller room music.

    public void PlayHighRollerMusic()
    {
        PlayMusic(highRollerMusic);
    }


    /// Plays the Ending Scene music.
    
    public void PlayEndingMusic()
    {
        PlayMusic(endingMusic);
    }
}
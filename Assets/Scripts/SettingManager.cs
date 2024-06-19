using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
   
    public GameObject settings;
    public GameObject settingscreen;
    public GameObject controls;
    public GameObject audioSetting;
    public GameObject levelSetting;
    public GameObject settingButton;
    public GameObject returnButton;
    public GameObject backArrowButtonForControlScreen;
    public GameObject backArrowButtonForAudioScreen;
    public GameObject backArrowButtonForLevelScreen;
    public GameObject pauseMenuPanelCanvas;
    public GameObject settingMenuCanvas;
    private AudioSource audioSource;
    public Button controllerButton;
    public Button joysticButton;
    public Button swipeButton;
    public GameObject pauseButton;
 
    public Button simpleLevelButton;
    public Button wallsLevelButton;
    // Sprites for different button states (normal, pressed, etc.)
    public Sprite normalSprite;
    public Sprite pressedSprite;
    public int controllerButtonPressed;
    public int joysticButtonPressed;
    public int swipeButtonPressed;
    public int simpleLevelButtonPressed;
    public int wallsLevelButtonPressed;
    // Variable to store the current control mode
    private bool isMode;
    private bool isModeJoyStick;
    private bool isSwipeMode;
    private bool isSimple;
    private bool isWall;
    public GameObject warningText;
    public GameObject yes;
    public GameObject no;
    public GameObject restart;
    public GameObject resume;
    public GameObject quit;
    // Define events for settings changes
    public delegate void ControlSettingsChanged(bool isMode, bool isModeJoyStick, bool isSwipeMode);
    public static event ControlSettingsChanged OnControlSettingsChanged;

    public delegate void LevelSettingsChanged(bool isSimple, bool isWall);
    public static event LevelSettingsChanged OnLevelSettingsChanged;
    public GameObject pausedBg;
    public GameObject exitBg;
    public GameObject settingBg;
    

    private void SaveControlSettings(bool isMode, bool isModeJoyStick, bool isSwipeMode)
    {
    PlayerPrefs.SetInt("IsMode", isMode ? 1 : 0);
    PlayerPrefs.SetInt("IsModeJoyStick", isModeJoyStick ? 1 : 0);
    PlayerPrefs.SetInt("IsSwipeMode", isSwipeMode ? 1 : 0);
    PlayerPrefs.Save();
    }

    private void SaveLevelSettings(bool isSimple, bool isWall)
    {
        PlayerPrefs.SetInt("IsSimple", isSimple ? 1 : 0);
        PlayerPrefs.SetInt("IsWall", isWall ? 1 : 0);
        PlayerPrefs.Save();
    }
     private void LoadControlSettings()
    {
        // Load control settings from PlayerPrefs
        isMode = PlayerPrefs.GetInt("IsMode", 0) == 1;
        isModeJoyStick = PlayerPrefs.GetInt("IsModeJoyStick", 0) == 1;
        isSwipeMode = PlayerPrefs.GetInt("IsSwipeMode", 0) == 1;
        UpdateControlSettings(isMode, isModeJoyStick, isSwipeMode);
    }

    private void LoadLevelSettings()
    {
        // Load Level settings from PlayerPrefs
        isSimple = PlayerPrefs.GetInt("IsSimple", 0) == 1;
        isWall = PlayerPrefs.GetInt("IsWall",0) == 1;
        UpdateLevelSettings(isSimple, isWall);
    }
    private void UpdateControlSettings(bool isMode, bool isModeJoyStick, bool isSwipeMode) 
    {
    OnControlSettingsChanged?.Invoke(isMode, isModeJoyStick, isSwipeMode);
    SaveControlSettings(isMode, isModeJoyStick, isSwipeMode);
    }

    private void UpdateLevelSettings(bool isSimple, bool isWall)
    {
        OnLevelSettingsChanged?.Invoke(isSimple, isWall);
        SaveLevelSettings(isSimple, isWall);
    }
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Load control settings from PlayerPrefs
        //LoadControlSettings();
        // Update button sprites based on loaded control settings
        UpdateControlButtonSprites();
        UpdateLevelButtonSprites();
        //Retrieve saved preferences for button states
        int controllerButtonPressed = PlayerPrefs.GetInt("ControllerButtonPressed", 0);
        int joysticButtonPressed = PlayerPrefs.GetInt("JoystickButtonPressed", 0);
        int swipeButtonPressed = PlayerPrefs.GetInt("SwipeButtonPressed", 1);
        simpleLevelButtonPressed = PlayerPrefs.GetInt("SimpleLevelButtonPressed", 0);
        wallsLevelButtonPressed = PlayerPrefs.GetInt("WallsLevelButtonPressed",0);

        // Update button sprites based on saved preferences
        controllerButton.image.sprite = controllerButtonPressed == 1 ? pressedSprite : normalSprite;
        joysticButton.image.sprite = joysticButtonPressed == 1 ? pressedSprite : normalSprite;
        swipeButton.image.sprite = swipeButtonPressed == 1 ? pressedSprite : normalSprite;
        simpleLevelButton.image.sprite = simpleLevelButtonPressed == 1 ? pressedSprite : normalSprite;
        wallsLevelButton.image.sprite = wallsLevelButtonPressed == 1 ? pressedSprite : normalSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Method to load control settings from PlayerPrefs
   
     // Method to update button sprites based on loaded control settings
    private void UpdateControlButtonSprites()
    {
        // Set button sprites based on loaded control settings
        controllerButton.image.sprite = isMode ? pressedSprite : normalSprite;
        joysticButton.image.sprite = isModeJoyStick ? pressedSprite : normalSprite;
        swipeButton.image.sprite = isSwipeMode ? pressedSprite : normalSprite;
       
    }

    private void UpdateLevelButtonSprites()
    {
        simpleLevelButton.image.sprite = isSimple ? pressedSprite : normalSprite;
        wallsLevelButton.image.sprite = isWall ? pressedSprite : normalSprite;
    }
    

    public void SettingsButton()
    {
        PlayClickSound();
        pauseMenuPanelCanvas.gameObject.SetActive(false);
        settingMenuCanvas.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(false);
        settingscreen.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);
    }

    public void ControlButton () 
    {
        PlayClickSound();
        settingBg.gameObject.SetActive(false);
        settingscreen.gameObject.SetActive(false);
        controls.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(false);
        backArrowButtonForControlScreen.gameObject.SetActive(true);
        backArrowButtonForAudioScreen.gameObject.SetActive(false);
        backArrowButtonForLevelScreen.gameObject.SetActive(false);
    }

    public void AudioButton()
    {
        PlayClickSound();
        settingBg.gameObject.SetActive(false);
        settingscreen.gameObject.SetActive(false);
        audioSetting.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(false);
        backArrowButtonForAudioScreen.gameObject.SetActive(true);
        backArrowButtonForControlScreen.gameObject.SetActive(false);
        backArrowButtonForLevelScreen.gameObject.SetActive(false);

    }

    public void LevelSettingsButton()
    {
        PlayClickSound();
        settingBg.gameObject.SetActive(false);
        settingscreen.gameObject.SetActive(false);
        levelSetting.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(false);
        backArrowButtonForLevelScreen.gameObject.SetActive(true);
        backArrowButtonForAudioScreen.gameObject.SetActive(false);
        backArrowButtonForControlScreen.gameObject.SetActive(false);
    }

    public void MuteButton () 
    {
        PlayClickSound(); 
         
        // FindObjectOfType<AudioManager>().ToggleMute();
        // FindObjectOfType<SpriteManager1>().ToggleUiSprite(); 
          
      
    }

    public void UnMuteButton()
    {
        PlayClickSound();
    }

    public void CheckForQuitGame()
    {
        PlayClickSound();
        warningText.gameObject.SetActive(true);
        yes.gameObject.SetActive(true);
        no.gameObject.SetActive(true);
        restart.gameObject.SetActive(false);
        resume.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        settingButton.gameObject.SetActive(false);
        exitBg.SetActive(true);
        pausedBg.SetActive(false);
        
    }
    public void YesToQuit()
    {
        PlayClickSound();
        Application.Quit();
    }
    public void NoToQuit()
    {
        PlayClickSound();
        warningText.gameObject.SetActive(false);
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        restart.gameObject.SetActive(true);
        resume.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
        exitBg.SetActive(false);
        pausedBg.SetActive(true);
    }

    
    public void ReturnButtonForSettingScreen()
    {
        PlayClickSound();
        settingscreen.gameObject.SetActive(false);
        settingMenuCanvas.gameObject.SetActive(false);
        pauseMenuPanelCanvas.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(false);
        // pauseButton.gameObject.SetActive(true);
       
    }


    public void ReturnButtonForControlScreen()
    {
        PlayClickSound();
        settingBg.gameObject.SetActive(true);
        settingscreen.gameObject.SetActive(true);
        controls.gameObject.SetActive(false);
        backArrowButtonForControlScreen.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(true);
        
    }

    public void ReturnButtonForAudioScreen()
    {
        PlayClickSound();
         settingBg.gameObject.SetActive(true);
        settingscreen.gameObject.SetActive(true);
        audioSetting.gameObject.SetActive(false);
        backArrowButtonForAudioScreen.SetActive(false);
        returnButton.gameObject.SetActive(true);
    }

    public void ReturnButtonForLevelScreen()
    {
        PlayClickSound();
         settingBg.gameObject.SetActive(true);
        settingscreen.gameObject.SetActive(true);
        levelSetting.gameObject.SetActive(false);
        backArrowButtonForLevelScreen.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(true);
    }
   

   
  
     public void ControllerButton()
    {
        PlayClickSound();
        
        // PlayerPrefs.SetInt("isMode", boolToInt(true));
        // PlayerPrefs.SetInt("isModeJoyStick", boolToInt(false));
        // Update PlayerPrefs and button sprite
        isMode = true;
        isModeJoyStick = false;
        isSwipeMode = false;
        PlayerPrefs.SetInt("ControllerButtonPressed", 1);
        PlayerPrefs.SetInt("JoystickButtonPressed", 0);
        PlayerPrefs.SetInt("SwipeButtonPressed", 0);
        // Update control settings
        UpdateControlSettings(true, false, false);
        // Save control settings to PlayerPrefs
        //SaveControlSettings();
        UpdateControlButtonSprites();
        
        
    }

     public void ControllerJoyStick()
    {
        PlayClickSound();
        isMode = false;
        isModeJoyStick = true;
        isSwipeMode = false;
        // PlayerPrefs.SetInt("isMode", boolToInt(false));
        // PlayerPrefs.SetInt("isModeJoyStick",boolToInt(true));
        // Update PlayerPrefs and button sprite
        PlayerPrefs.SetInt("ControllerButtonPressed", 0);
        PlayerPrefs.SetInt("JoystickButtonPressed", 1);
        PlayerPrefs.SetInt("SwipeButtonPressed", 0);
        // Update control settings
        UpdateControlSettings(false, true, false);
        //SaveControlSettings();
        UpdateControlButtonSprites();
         
    }
   
    public void ControllerSwipe()
    {
        PlayClickSound();
        isMode = false;
        isModeJoyStick = false;
        isSwipeMode = true;

        // PlayerPrefs.SetInt("isMode", boolToInt(false));
        // PlayerPrefs.SetInt("isModeJoyStick", boolToInt(false));
        // Update PlayerPrefs and button sprite
        PlayerPrefs.SetInt("ControllerButtonPressed", 0);
        PlayerPrefs.SetInt("JoystickButtonPressed", 0);
        PlayerPrefs.SetInt("SwipeButtonPressed", 1);
        // Update control settings
        UpdateControlSettings(false, false, true);
        //SaveControlSettings();
        UpdateControlButtonSprites();
       

    }

    public void LevelSimple()
    {
        PlayClickSound();
        isSimple = true;
        isWall = false;
        // PlayerPrefs.SetInt("isLevel",boolToInt(true));
        PlayerPrefs.SetInt("SimpleLevelButtonPressed",1);
        PlayerPrefs.SetInt("WallsLevelButtonPressed",0);
        // Update Level settings
        UpdateLevelSettings(true, false);
        UpdateLevelButtonSprites();
       
    }

    public void LevelWall()
    {
        PlayClickSound();
        isSimple = false;
        isWall = true;
        // PlayerPrefs.SetInt("isLevel",boolToInt(false));
        PlayerPrefs.SetInt("SimpleLevelButtonPressed",0);
        PlayerPrefs.SetInt("WallsLevelButtonPressed",1);
        // Update Level settings
        UpdateLevelSettings(false, true);
        UpdateLevelButtonSprites();
        
    }
  

    private int boolToInt(bool booleanValue)
    {
        return booleanValue ? 1 : 0;
    }
    private void PlayClickSound()
    {
        // Check if an AudioClip is assigned to the AudioSource
        if (audioSource.clip != null)
        {
        // Play the click sound
        audioSource.PlayOneShot(audioSource.clip);
        }
    }
}

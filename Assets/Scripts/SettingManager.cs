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
    public GameObject pauseMenuPanel;
    public GameObject settingMenu;
    private AudioSource audioSource;


    public AudioClip clickAudioClip;
    
    public Button controllerButton;
    public Button joysticButton;
    public Button swipeButton;

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
    // Define events for settings changes
    public delegate void ControlSettingsChanged(bool isMode, bool isModeJoyStick, bool isSwipeMode);
    public static event ControlSettingsChanged OnControlSettingsChanged;
    

    private void SaveControlSettings(bool isMode, bool isModeJoyStick, bool isSwipeMode)
    {
    PlayerPrefs.SetInt("IsMode", isMode ? 1 : 0);
    PlayerPrefs.SetInt("IsModeJoyStick", isModeJoyStick ? 1 : 0);
    PlayerPrefs.SetInt("isSwipeMode", isSwipeMode ? 1 : 0);
    PlayerPrefs.Save();
    }
     private void LoadControlSettings()
    {
        // Load control settings from PlayerPrefs
        isMode = PlayerPrefs.GetInt("isMode", 0) == 1;
        isModeJoyStick = PlayerPrefs.GetInt("isModeJoyStick", 0) == 1;
        isSwipeMode = PlayerPrefs.GetInt("isSwipeMode", 0) == 1;
        UpdateControlSettings(isMode, isModeJoyStick, isSwipeMode);
    }
     private void UpdateControlSettings(bool isMode, bool isModeJoyStick, bool isSwipeMode) {
    OnControlSettingsChanged?.Invoke(isMode, isModeJoyStick, isSwipeMode);
    SaveControlSettings(isMode, isModeJoyStick, isSwipeMode);
}
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Load control settings from PlayerPrefs
        LoadControlSettings();

        // Update button sprites based on loaded control settings
        UpdateButtonSprites();

        //Retrieve saved preferences for button states
        int controllerButtonPressed = PlayerPrefs.GetInt("ControllerButtonPressed", 0);
        int joysticButtonPressed = PlayerPrefs.GetInt("JoystickButtonPressed", 0);
        int swipeButtonPressed = PlayerPrefs.GetInt("SwipeButtonPressed", 0);
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
    private void UpdateButtonSprites()
    {
        // Set button sprites based on loaded control settings
        controllerButton.image.sprite = isMode ? pressedSprite : normalSprite;
        joysticButton.image.sprite = isModeJoyStick ? pressedSprite : normalSprite;
        swipeButton.image.sprite = isSwipeMode ? pressedSprite : normalSprite;
    }
     // Method to save control settings to PlayerPrefs
    // private void SaveControlSettings()
    // {
    //     // Save control settings to PlayerPrefs
    //     PlayerPrefs.SetInt("isMode", isMode ? 1 : 0);
    //     PlayerPrefs.SetInt("isModeJoyStick", isModeJoyStick ? 1 : 0);
    //     PlayerPrefs.SetInt("isSwipeMode", isSwipeMode ? 1 : 0);
    // }
   
    //Method to update control settings
    // private void UpdateControlSettings(bool isMode, bool isModeJoyStick)
    // {
    //     OnControlSettingsChanged?.Invoke(isMode, isModeJoyStick);
    // }
   

    public void SettingsButton()
    {
        PlayClickSound();
        pauseMenuPanel.gameObject.SetActive(false);
        settingButton.gameObject.SetActive(false);
        settingscreen.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);
    }

    public void ControlButton () 
    {
        PlayClickSound();
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
         
        FindObjectOfType<AudioManager>().ToggleMute();
        FindObjectOfType<SpriteManager1>().ToggleUiSprite(); 
          
      
    }

    public void UnMuteButton()
    {
        PlayClickSound();
    }

    public void QuitGame()
    {
        Debug.Log("Application Quited");
        Application.Quit();
    }

    
    public void ReturnButtonForSettingScreen()
    {
        PlayClickSound();
        settingscreen.gameObject.SetActive(false);
        pauseMenuPanel.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(false);
       
    }


    public void ReturnButtonForControlScreen()
    {
        PlayClickSound();
        settingscreen.gameObject.SetActive(true);
        controls.gameObject.SetActive(false);
        backArrowButtonForControlScreen.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(true);
        
    }

    public void ReturnButtonForAudioScreen()
    {
        PlayClickSound();
        settingscreen.gameObject.SetActive(true);
        audioSetting.gameObject.SetActive(false);
        backArrowButtonForAudioScreen.SetActive(false);
        returnButton.gameObject.SetActive(true);
    }

    public void ReturnButtonForLevelScreen()
    {
        PlayClickSound();
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
        UpdateButtonSprites();
        
        
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
        UpdateButtonSprites();
         
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
        UpdateButtonSprites();
       

    }

   
    // Method to update button sprites based on saved preferences
    // private void UpdateButtonSprites()
    // {
    //     int controllerButtonPressed = PlayerPrefs.GetInt("ControllerButtonPressed", 0);
    //     int joysticButtonPressed = PlayerPrefs.GetInt("JoystickButtonPressed", 0);
    //     int swipeButtonPressed = PlayerPrefs.GetInt("SwipeButtonPressed", 0);
    //     simpleLevelButtonPressed = PlayerPrefs.GetInt("SimpleLevelButtonPressed", 0);
    //     wallsLevelButtonPressed = PlayerPrefs.GetInt("WallsLevelButtonPressed",0);

    //     controllerButton.image.sprite = controllerButtonPressed == 1 ? pressedSprite : normalSprite;
    //     joysticButton.image.sprite = joysticButtonPressed == 1 ? pressedSprite : normalSprite;
    //     swipeButton.image.sprite = swipeButtonPressed == 1 ? pressedSprite : normalSprite;
    //     simpleLevelButton.image.sprite = simpleLevelButtonPressed == 1 ? pressedSprite : normalSprite;
    //     wallsLevelButton.image.sprite = wallsLevelButtonPressed == 1 ? pressedSprite : normalSprite;
    // }
    public void LevelSimple()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("isLevel",boolToInt(true));
        PlayerPrefs.SetInt("SimpleLevelButtonPressed",1);
        PlayerPrefs.SetInt("WallsLevelButtonPressed",0);
        // Update control settings
        //UpdateControlSettings(true, false);
        UpdateButtonSprites();
       
    }

    public void LevelWall()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("isLevel",boolToInt(false));
        PlayerPrefs.SetInt("SimpleLevelButtonPressed",0);
        PlayerPrefs.SetInt("WallsLevelButtonPressed",1);
        // Update control settings
        //UpdateControlSettings(true, false);
        UpdateButtonSprites();
        
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
        audioSource.PlayOneShot(clickAudioClip);
        }
    }
}

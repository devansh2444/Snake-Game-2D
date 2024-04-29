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
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // bool hasSavedGameState = PlayerPrefs.HasKey("HasSavedGameState");
        // playButton.gameObject.SetActive(!hasSavedGameState);
        // resumeButton.gameObject.SetActive(hasSavedGameState);


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
        PlayerPrefs.SetInt("isMode", boolToInt(true));
        PlayerPrefs.SetInt("isModeJoyStick", boolToInt(false));
        // Update PlayerPrefs and button sprite
        PlayerPrefs.SetInt("ControllerButtonPressed", 1);
        PlayerPrefs.SetInt("JoystickButtonPressed", 0);
        PlayerPrefs.SetInt("SwipeButtonPressed", 0);
        UpdateButtonSprites();
        
        
    }
   
    public void ControllerSwipe()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("isMode", boolToInt(false));
        PlayerPrefs.SetInt("isModeJoyStick", boolToInt(false));
        // Update PlayerPrefs and button sprite
        PlayerPrefs.SetInt("ControllerButtonPressed", 0);
        PlayerPrefs.SetInt("JoystickButtonPressed", 0);
        PlayerPrefs.SetInt("SwipeButtonPressed", 1);
        UpdateButtonSprites();
       
        

    }

    public void ControllerJoyStick()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("isMode", boolToInt(false));
        PlayerPrefs.SetInt("isModeJoyStick",boolToInt(true));
        // Update PlayerPrefs and button sprite
        PlayerPrefs.SetInt("ControllerButtonPressed", 0);
        PlayerPrefs.SetInt("JoystickButtonPressed", 1);
        PlayerPrefs.SetInt("SwipeButtonPressed", 0);
        UpdateButtonSprites();
         
    }
    // Method to update button sprites based on saved preferences
    private void UpdateButtonSprites()
    {
        int controllerButtonPressed = PlayerPrefs.GetInt("ControllerButtonPressed", 0);
        int joysticButtonPressed = PlayerPrefs.GetInt("JoystickButtonPressed", 0);
        int swipeButtonPressed = PlayerPrefs.GetInt("SwipeButtonPressed", 0);
        simpleLevelButtonPressed = PlayerPrefs.GetInt("SimpleLevelButtonPressed", 0);
        wallsLevelButtonPressed = PlayerPrefs.GetInt("WallsLevelButtonPressed",0);

        controllerButton.image.sprite = controllerButtonPressed == 1 ? pressedSprite : normalSprite;
        joysticButton.image.sprite = joysticButtonPressed == 1 ? pressedSprite : normalSprite;
        swipeButton.image.sprite = swipeButtonPressed == 1 ? pressedSprite : normalSprite;
        simpleLevelButton.image.sprite = simpleLevelButtonPressed == 1 ? pressedSprite : normalSprite;
        wallsLevelButton.image.sprite = wallsLevelButtonPressed == 1 ? pressedSprite : normalSprite;
    }
    public void LevelSimple()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("isLevel",boolToInt(true));
        PlayerPrefs.SetInt("SimpleLevelButtonPressed",1);
        PlayerPrefs.SetInt("WallsLevelButtonPressed",0);
        UpdateButtonSprites();
       
    }

    public void LevelWall()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("isLevel",boolToInt(false));
        PlayerPrefs.SetInt("SimpleLevelButtonPressed",0);
        PlayerPrefs.SetInt("WallsLevelButtonPressed",1);
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

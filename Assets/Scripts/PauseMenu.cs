using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   [SerializeField] public GameObject PauseMenuPanelCanvas;
   public GameObject pauseButton;
   public GameObject settingButton;
   private AudioSource audioSource;
     public GameObject pausedBg;
     public GameObject score;
     public GameObject highscore;

  

    private void Start() 
    {
     
        audioSource = GetComponent<AudioSource>(); 
    }


   public void Pause () 
   {
        pauseButton.SetActive(false);
        PauseMenuPanelCanvas.SetActive(true);
        settingButton.SetActive(true);
        Time.timeScale = 0;
        PlayClickSound();
        pausedBg.SetActive(true);
        score.SetActive(false);
        highscore.SetActive(false);
   }

   public void Resume () 
   {
        pauseButton.SetActive(true);
        PauseMenuPanelCanvas.SetActive(false); 
         score.SetActive(true);
        highscore.SetActive(true);
        settingButton.SetActive(false);
        Time.timeScale = 1;
        //GameStateManager.LoadGameState(FindObjectOfType<Snake>());
        PlayClickSound();
   }

   
   public void Restart () 
   {
        
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
        PlayClickSound();
   }
   public void GoToStartMenu()
    {
          
          PlayClickSound();
          Time.timeScale = 1;  
          
          SceneManager.LoadScene("StartScene");
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

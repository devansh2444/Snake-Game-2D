using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   [SerializeField] public GameObject PauseMenuPanel;
   public GameObject pauseButton;
   
   private AudioSource audioSource;


    private void Start() 
    {
        audioSource = GetComponent<AudioSource>(); 
          
    }


   public void Pause () 
   {
        pauseButton.SetActive(false);
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0;
        PlayClickSound();

   }

   public void Resume () 
   {
        pauseButton.SetActive(true);
        PauseMenuPanel.SetActive(false); 
        Time.timeScale = 1;
        //GameStateManager.LoadGameState(FindObjectOfType<Snake>());
        PlayClickSound();
   }

   public void Restart () 
   {
        PlayClickSound();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
   }
   public void GoToStartMenu()
    {
          // GameStateManager.SaveGameState(FindObjectOfType<Snake>()); 
          // bool hasSavedGameState = PlayerPrefs.HasKey("HasSavedGameState");
          // PlayerPrefs.SetString("HasSavedGameState", "True");
          // Debug.Log(hasSavedGameState);
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

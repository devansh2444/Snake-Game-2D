using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    
    private AudioSource audioSource;
    public AudioClip clickSound;


    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void RestartGame()
    {
        PlayClickSound();
        
       
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        PlayClickSound();
        // Note: Application.Quit() might not work in the Unity Editor, consider testing in a build.
        // In the Editor, you can use UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    private void PlayClickSound()
    {
        // Check if an AudioClip is assigned to the AudioSource
        if (audioSource != null && clickSound != null)
        {
            // Play the click sound
            audioSource.PlayOneShot(clickSound);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour

{

    private AudioSource audioSource;
    public GameObject gameOverText;
    public TextMeshProUGUI score;
    public TextMeshProUGUI collectedCoins;
    public GameObject gameoverPanel;
   
    void Start()
    {
        //audioSource.Play();
        audioSource = GetComponent<AudioSource>();
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        
        collectedCoins.text = ": " + GameManager.coinCount.ToString();
        score.text = "Score:" + Snake.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        PlayClickSound();
        GameManager.ResetScore();  // Reset the score before restarting the game
        SceneManager.LoadScene("MainScene");
    }

    public void Continue()
    {
        PlayClickSound();
        GameManager.LoadGameState();  // Load the saved game state
        gameoverPanel.SetActive(false);  // Hide the game over panel

        // Find and reset the necessary game components, e.g., the snake
        Snake.Instance.ContinueGame();
    }

    

    private void PlayClickSound()
    {
        if(audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

}

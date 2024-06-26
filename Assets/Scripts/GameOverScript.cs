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
    public GameObject notEnoughCoinsDialog;  // Dialog box for not enough coins
   
    void Start()
    {
        //audioSource.Play();
        audioSource = GetComponent<AudioSource>();
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        
        collectedCoins.text = ": " + GameManager.coinCount.ToString();
        Debug.Log("Game Over Coins:" + collectedCoins.text);
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
         Debug.Log("Attempting to Continue...");
        if(GameManager.coinCount > 0)
        {
        PlayClickSound();
        GameManager.AddCoins(-1); // Deduct 1 coin
        UpdateScoreUI();  // Update UI to reflect the new coin count
         
        Debug.Log("Coin Debucted");
        Debug.Log("Updated Coins:" + GameManager.coinCount);
        //GameManager.LoadGameState();  // Load the saved game state
        gameoverPanel.SetActive(false);  // Hide the game over panel
        Snake.Instance.ContinueGame(); // Continue the game
        }
        else
        {
            ShowNotEnoughCoinsDialog();  // Show dialog if not enough coins
        }
    }
    private void ShowNotEnoughCoinsDialog()
    {
         Debug.Log("Not Enough Coins to Continue");
        notEnoughCoinsDialog.SetActive(true);
    }

    

    private void PlayClickSound()
    {
        if(audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

}

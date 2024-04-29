
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodSpawner : MonoBehaviour
{
    // public BoxCollider2D foodSpawn;
    // public float score;
    // public TextMeshProUGUI scoreText;
    // public TextMeshProUGUI highScoreText;
    // public SnakeMovement snakeMovement;

    // private bool foodBeingConsumed = false;

    // private void Start()
    // {
    //     UpdateScoreUI();
    //     UpdateHighScoreUI();
    //     SpawnFood();
        
    // }

    // private void Update()
    // {
    //     // scoreText.text = "Score:" + score;
    // }

    // private void SpawnFood()
    // {
    //     Bounds bounds = foodSpawn.bounds;

    //     float x = Random.Range(bounds.min.x, bounds.max.x);
    //     float y = Random.Range(bounds.min.y, bounds.max.y);

    //     transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);

    //     foodBeingConsumed = false;
    // }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.tag == "Player" && !foodBeingConsumed)
    //     {
    //         foodBeingConsumed = true;
    //         if(snakeMovement.powerup2XActive == true)
    //         {
    //             score += 10;
    //         }
    //         else
    //         {
    //             score += 5;
    //         }
    //         UpdateScoreUI();


    //          if (score > PlayerPrefs.GetFloat("HighScore", 0))
    //         {
    //             // New high score reached
    //             PlayerPrefs.SetFloat("HighScore", score);
    //             UpdateHighScoreUI();
    //             // Optionally, you can display a UI notification for new high score here
               
    //         }

    //         Invoke("SpawnFood",0f); // Delay the spawn to ensure the previous food is consumed
    //     }
    // }
    // private void UpdateScoreUI()
    // {
    //     // if (snakeMovement != null)
    //     // {
    //     //     int scoreIncrement = snakeMovement.powerupActive ? 10 : 5;
    //     //     score += scoreIncrement;

    //     //     scoreText.text = "Score:" + score;
    //     // }
    //     // else
    //     // {
    //     //     Debug.LogWarning("SnakeMovement script reference is null in FoodSpawner.");
    //     // }
        
    //     scoreText.text = "Score:" + score;
    // }

    // private void UpdateHighScoreUI()
    // {
    //     highScoreText.text = "High Score:" + PlayerPrefs.GetFloat("HighScore", 0);
    // }
}



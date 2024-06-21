using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int coinCount;
    // public static float score;
    private const string COIN_COUNT_KEY = "CoinCount";

   private void Awake()
{
    if (FindObjectsOfType<GameManager>().Length > 1)
    {
        Destroy(gameObject);
    }
    else
    {
        DontDestroyOnLoad(gameObject);
    }

    // Load the coin count from PlayerPrefs
    coinCount = PlayerPrefs.GetInt(COIN_COUNT_KEY, 0);
}


    public static void AddCoins(int count)
    {
        coinCount += count;
        PlayerPrefs.SetInt(COIN_COUNT_KEY, coinCount);
    }

    public static void ResetCoins()
    {
        coinCount = 0;
        PlayerPrefs.SetInt(COIN_COUNT_KEY, coinCount);
    }
    public static void ResetScore()
    {
       Snake.score = 0;
    }
    public static void SaveGameState()
    {
        
        PlayerPrefs.SetInt("SavedCoinCount", coinCount);
        // Add other states to save as needed
    }

    public static void LoadGameState()
    {
        
        coinCount = PlayerPrefs.GetInt("SavedCoinCount", 0);
        // Load other states as needed
    }


    private void OnApplicationQuit()
    {
        // Save the coin count when the application quits
        PlayerPrefs.SetInt(COIN_COUNT_KEY, coinCount);
    }
}

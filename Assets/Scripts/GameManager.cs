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
    Debug.Log("Loaded Coin Count: " + coinCount);
}


    public static void AddCoins(int count)
    {
        coinCount += count;
        // PlayerPrefs.SetInt(COIN_COUNT_KEY, coinCount);
        SaveCoinCount();
        Debug.Log("GameManager Coins after AddCoins: " + coinCount);
    }

    // public static void DeductCoin(int count)
    // {
    //     coinCount -= count;
    //     PlayerPrefs.SetInt(COIN_COUNT_KEY, coinCount);
    //     PlayerPrefs.Save();
    //     Debug.Log("Game Manager Coins:" + coinCount);
    // }

    public static void ResetCoins()
    {
        coinCount = 0;
        // PlayerPrefs.SetInt(COIN_COUNT_KEY, coinCount);
        SaveCoinCount();
        Debug.Log("GameManager Coins after ResetCoins: " + coinCount);
    }
    public static void ResetScore()
    {
       Snake.score = 0;
    }
    public static void SaveGameState()
    {
        
        PlayerPrefs.SetInt("SavedCoinCount", coinCount);
        PlayerPrefs.Save();
        Debug.Log("GameManager SaveGameState: SavedCoinCount = " + coinCount);
        // Add other states to save as needed
    }

    public static void LoadGameState()
    {
        
        coinCount = PlayerPrefs.GetInt("SavedCoinCount", 0);
        Debug.Log("GameManager LoadGameState: Loaded SavedCoinCount = " + coinCount);
        
        // Load other states as needed
    }

     private static void SaveCoinCount()
    {
        PlayerPrefs.SetInt(COIN_COUNT_KEY, coinCount);
        PlayerPrefs.Save();
    }
    private static void LoadCoinCount()
    {
        coinCount = PlayerPrefs.GetInt(COIN_COUNT_KEY, 0);
    }



    private void OnApplicationQuit()
    {
        // Save the coin count when the application quits
        SaveCoinCount();
        Debug.Log("GameManager OnApplicationQuit: Saved Coin Count = " + coinCount);
    }
}

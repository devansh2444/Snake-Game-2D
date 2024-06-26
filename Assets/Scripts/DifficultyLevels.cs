using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyLevels : MonoBehaviour
{
 
    public enum Difficulty 
    {
        Easy,
        Medium,
        Hard
    }

    private Difficulty difficulty;

    public void EasyDifficulty () {
        difficulty = Difficulty.Easy;
        PlayerPrefs.SetString ("Difficulty", difficulty.ToString());
        SceneManager.LoadScene("MainScene");
    }

    public void MediumDifficulty () {
        difficulty = Difficulty.Medium;
        PlayerPrefs.SetString ("Difficulty", difficulty.ToString());
        SceneManager.LoadScene("MainScene");
    }

    public void HardDifficulty () {
        difficulty = Difficulty.Hard;
        PlayerPrefs.SetString ("Difficulty", difficulty.ToString());
        SceneManager.LoadScene("MainScene");
    }
}

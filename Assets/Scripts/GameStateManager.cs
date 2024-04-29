using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
     private const string SnakePositionXKey = "SnakePositionX";
    private const string SnakePositionYKey = "SnakePositionY";
    private const string SnakeDirectionKey = "SnakeDirection";
    private const string ScoreKey = "Score";
    private const string HighScoreKey = "HighScore";
    private const string PowerupActiveKey = "PowerupActive";
    private const string PowerupDurationKey = "PowerupDuration";
    private const string ControlPreferenceKey = "ControlPreference";
    private const string LevelSettingsKey = "LevelSettings";

    public static void SaveGameState(Snake snake)
    {
        PlayerPrefs.SetFloat(SnakePositionXKey, snake.GetGridPosition().x);
        PlayerPrefs.SetFloat(SnakePositionYKey, snake.GetGridPosition().y);
        PlayerPrefs.SetInt(SnakeDirectionKey, (int)snake.GetDirection());
        PlayerPrefs.SetFloat(ScoreKey, snake.score);
        PlayerPrefs.SetFloat(HighScoreKey, PlayerPrefs.GetFloat("HighScore", 0)); // Save the high score
        PlayerPrefs.SetInt(PowerupActiveKey, snake.powerup2XActive ? 1 : 0);
        //PlayerPrefs.SetFloat(PowerupDurationKey, snake.currentPowerupTime);
        //PlayerPrefs.SetInt(ControlPreferenceKey, snake.GetControlPreference());
        //PlayerPrefs.SetInt(LevelSettingsKey, snake.GetLevelSettings());
        PlayerPrefs.Save();
    }
     public static void LoadGameState(Snake snake)
    {
        // Load saved values and apply them to the snake
        float snakePosX = PlayerPrefs.GetFloat(SnakePositionXKey, 0);
        float snakePosY = PlayerPrefs.GetFloat(SnakePositionYKey, 0);
        int snakeDirection = PlayerPrefs.GetInt(SnakeDirectionKey, 0);
        snake.gridPosition = new Vector3(snakePosX, snakePosY);
        snake.gridMoveDirection = (Snake.Direction)snakeDirection;
        snake.score = PlayerPrefs.GetFloat(ScoreKey, 0);
        snake.UpdateScoreUI();
        snake.UpdateHighScoreUI();
        snake.powerup2XActive = PlayerPrefs.GetInt(PowerupActiveKey, 0) == 1;
        snake.currentPowerupTime = PlayerPrefs.GetFloat(PowerupDurationKey, 0);
        snake.ActivatePowerup2X(); // Activate the power-up if it was active
        // int controlPreference = PlayerPrefs.GetInt(ControlPreferenceKey, 0);
        // snake.ApplyControlPreference(controlPreference);
        int levelSettings = PlayerPrefs.GetInt(LevelSettingsKey, 0);
        //snake.ApplyLevelSettings(levelSettings);
    }
}

/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using UnityEngine.UIElements;

public class LevelGrid {

    private Vector3 foodGridPosition;
    private GameObject foodGameObject;
    private float width;
    private float height;
    private float gridWidthForLevelGrid;
    private float gridHeightForLevelGrid;
    
    private Snake snake;
    private AudioSource audioSource;

    

    private ViewportHandler viewportHandler;

    void Awake() {
        gridWidthForLevelGrid = viewportHandler.GridWidth - 0.5f;
        gridHeightForLevelGrid = viewportHandler.GridHeight - 0.5f;
    }
    public LevelGrid(float width, float height, ViewportHandler viewportHandler) {
        this.width = width;
        this.height = height;
        this.viewportHandler = viewportHandler;
        audioSource = GameObject.Find("Snake").GetComponent<AudioSource>();
    }

    
    public void Setup(Snake snake) {
        this.snake = snake;

        SpawnFood();
    }
    

    public  void SpawnFood() {
        do {
            foodGridPosition = new Vector3(Random.Range(-gridWidthForLevelGrid+2, width-2), Random.Range(-gridHeightForLevelGrid+2, height-2));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        
    }
    //  public Vector3 GetFoodGridPosition() {
    //     return foodGridPosition;
    // }
// private void SpawnFood() {
//     Camera mainCamera = Camera.main;
//     if (mainCamera == null) {
//         Debug.LogError("Main camera not found!");
//         return;
//     }

//     float minX = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
//     float maxX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
//     float minY = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
//     float maxY = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

//     do {
//         float randomX = Random.Range(minX, maxX);
//         float randomY = Random.Range(minY, maxY);
//         foodGridPosition = new Vector2Int(Mathf.RoundToInt(randomX), Mathf.RoundToInt(randomY));
//     } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

//     foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
//     foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
//     foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
// }

  //     private void SpawnFood() {
    
//     // Get the bounds of the Box Collider attached to a GameObject named "FoodSpawnArea"
//     BoxCollider2D spawnAreaCollider = GameObject.Find("FoodSpawnArea").GetComponent<BoxCollider2D>();
    
//     // Check if the spawn area collider exists
//     if (spawnAreaCollider != null) {
//         // Get the bounds of the collider
//         Vector3 spawnAreaCenter = spawnAreaCollider.bounds.center;
//         Vector3 spawnAreaSize = spawnAreaCollider.bounds.size;

//         // Generate a random position within the spawn area
//         Vector3 randomPosition = spawnAreaCenter + new Vector3(
//             Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
//             Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
//             0f
//         );

//          // Update foodGridPosition
//         foodGridPosition = new Vector2Int((int)randomPosition.x, (int)randomPosition.y);

//         // Create the food GameObject and set its position
//         foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
//         foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
//         foodGameObject.transform.position = randomPosition;
//     } else {
//         Debug.LogError("FoodSpawnArea GameObject or BoxCollider2D component not found!");
//     }
// }
// public bool TrySnakeEatFood(Vector3 snakeGridPosition) {
//         if (snakeGridPosition == foodGridPosition) {
//             Object.Destroy(foodGameObject);
//             return true; // Food successfully eaten
//         } else {
//             return false; // Food not eaten
//         }
//     }


    // public bool TrySnakeEatFood(Vector3 snakeGridPosition) {
    //     Vector3 snakeGridPositionRound = new Vector3(Mathf.Round(snakeGridPosition.x), Mathf.Round(snakeGridPosition.y));
    //     Vector3 foodGridPositionRound = new Vector3(Mathf.Round(foodGridPosition.x), Mathf.Round(foodGridPosition.y));
    //     if (snakeGridPositionRound == foodGridPositionRound) {
    //         Object.Destroy(foodGameObject);
    //         SpawnFood();

    //         // Play the audio clip
    //         if (audioSource != null) {
    //             audioSource.Play();
    //         } else {
    //             Debug.LogError("AudioSource component not found!");
    //         }
            
    //         return true;
    //     } else {
    //         return false;
    //     }
    // }
    public bool TrySnakeEatFood(Vector3 snakeGridPosition) {
    // Define a tolerance value to allow a small variation in positions
    float tolerance = 0.7f;

    // Calculate the squared distance between snake and food positions
    float squaredDistance = (snakeGridPosition - foodGridPosition).sqrMagnitude;

    // Compare the squared distance with the square of the tolerance
    if (squaredDistance <= tolerance * tolerance) {
        Object.Destroy(foodGameObject);
        SpawnFood();
        return true;
    } else {
        return false;
    }
}
 

    public Vector3 ValidateGridPosition(Vector3 gridPosition) {
       
        gridWidthForLevelGrid = viewportHandler.GridWidth - 0.5f;
        gridHeightForLevelGrid = viewportHandler.GridHeight - 0.5f;
        
        if (gridPosition.x < -gridWidthForLevelGrid) {
            gridPosition.x = width - 1;
        }
        if (gridPosition.x > width - 1) {
            gridPosition.x = -gridWidthForLevelGrid;
        }
        if (gridPosition.y < -gridHeightForLevelGrid) {
            gridPosition.y = height - 1;
        }
        if (gridPosition.y > height - 1) {
            gridPosition.y = -gridHeightForLevelGrid;
        }
        return gridPosition;
    }
}



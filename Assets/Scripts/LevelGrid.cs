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
        gridWidthForLevelGrid = viewportHandler.GridWidth;
        gridHeightForLevelGrid = viewportHandler.GridHeight;
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
            foodGridPosition = new Vector3(Random.Range(-gridWidthForLevelGrid+2, width-4), Random.Range(-gridHeightForLevelGrid-3, height+3));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        
    }
    public bool TrySnakeEatFood(Vector3 snakeGridPosition) {
    float tolerance = 0.7f;

    float squaredDistance = (snakeGridPosition - foodGridPosition).sqrMagnitude;

    if (squaredDistance <= tolerance * tolerance) {
        Object.Destroy(foodGameObject);
        SpawnFood();
        return true;
    } else {
        return false;
    }
}
 

    public Vector3 ValidateGridPosition(Vector3 gridPosition) {
       
        gridWidthForLevelGrid = viewportHandler.GridWidth;
        gridHeightForLevelGrid = viewportHandler.GridHeight;
        
        if (gridPosition.x < -gridWidthForLevelGrid) {
            gridPosition.x = width - 1;
        }
        if (gridPosition.x > width - 1) {
            gridPosition.x = -gridWidthForLevelGrid;
        }
        if (gridPosition.y < gridHeightForLevelGrid) {
            gridPosition.y = -height -0.8f;
        }
        if (gridPosition.y > -height -0.8f) {
            gridPosition.y = gridHeightForLevelGrid;
        }
        return gridPosition;
    }
}



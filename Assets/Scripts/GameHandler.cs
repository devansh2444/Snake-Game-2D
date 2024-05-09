using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {

    [SerializeField] private Snake snake;
    private ViewportHandler viewportHandler;

    
    public LevelGrid levelGrid;

    
    private void Start() {
        Debug.Log("GameHandler.Start");
        Camera mainCamera = Camera.main;
        if (mainCamera == null) {
            Debug.LogError("Main camera not found!");
            return;
        }
        
        
         // Find the ViewportHandler component in the scene
        viewportHandler = FindObjectOfType<ViewportHandler>();
        if (viewportHandler == null) {
            Debug.LogError("ViewportHandler not found in the scene!");
            return;
        }
        
        // Access gridWidth and gridHeight
        float gridWidthForGameHandler = viewportHandler.GridWidth + 0.5f;
        float gridHeightForGameHandler = viewportHandler.GridHeight + 0.5f;
        Debug.Log("Grid Width: " + gridWidthForGameHandler + ", Grid Height: " + gridHeightForGameHandler );
        
        levelGrid = new LevelGrid(gridWidthForGameHandler, gridHeightForGameHandler,viewportHandler);
        //levelGrid = new LevelGrid(22.5f, 13f);
        
        snake.Setup(levelGrid);
       
        levelGrid.Setup(snake);

        
       
    }


}


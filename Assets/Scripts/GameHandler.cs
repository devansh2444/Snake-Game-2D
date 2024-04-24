// /* 
//     ------------------- Code Monkey -------------------

//     Thank you for downloading this package
//     I hope you find it useful in your projects
//     If you have any questions let me know
//     Cheers!

//                unitycodemonkey.com
//     --------------------------------------------------
//  */

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
        float gridWidthForGameHandler = viewportHandler.GridWidth + 0.5f ;
        float gridHeightForGameHandler = viewportHandler.GridHeight + 0.5f ;
        Debug.Log("Grid Width: " + gridWidthForGameHandler + ", Grid Height: " + gridHeightForGameHandler );
        
        levelGrid = new LevelGrid(gridWidthForGameHandler, gridHeightForGameHandler,viewportHandler);
        //levelGrid = new LevelGrid(22.5f, 13f);
        
        snake.Setup(levelGrid);
       
        levelGrid.Setup(snake);

        
       
    }
    
//     private void Start() {
//     // Debug.Log("GameHandler.Start");
//     Camera mainCamera = Camera.main;
//     if (mainCamera == null) {
//         Debug.LogError("Main camera not found!");
//         return;
//     }
    
//     // Choose a preferred aspect ratio
//     float preferredAspectRatio = 16f / 9f; // You can adjust this to your preference

//     // Calculate the aspect ratio of the screen
//     float aspectRatio = (float)Screen.width / Screen.height;

//     // Calculate the grid width based on the preferred aspect ratio
//     float gridWidth;
//     if (aspectRatio >= preferredAspectRatio) {
//         // Screen is wider, use height to calculate width
//         float orthographicSize = mainCamera.orthographicSize;
//         gridWidth = orthographicSize * 2 * aspectRatio;
//     } else {
//         // Screen is taller, use width directly
//         gridWidth = mainCamera.orthographicSize * 2 * preferredAspectRatio;
//     }

//     // Use the calculated grid width
//     float gridHeight = mainCamera.orthographicSize * 2;
//     levelGrid = new LevelGrid(Mathf.RoundToInt(gridWidth), Mathf.RoundToInt(gridHeight));
    
//     snake.Setup(levelGrid);
//     levelGrid.Setup(snake);
// }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectRatioAdjuster : MonoBehaviour
{
   [SerializeField] private float targetHeightInWorldUnits = 10.0f; // Desired height of the viewable area in world units

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        AdjustCameraSize();
    }

    private void Update()
    {
        // Adjust camera size on every frame in case of resolution changes during gameplay
        AdjustCameraSize();
    }

    private void AdjustCameraSize()
    {
        float screenRatio = Screen.width / (float)Screen.height;
        float targetWidthInWorldUnits = targetHeightInWorldUnits * screenRatio;

        mainCamera.orthographicSize = targetHeightInWorldUnits / 2.0f;
    }
}

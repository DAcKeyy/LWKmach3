using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    void Start()
    {
        SetupCamera();
    }

    private void SetupCamera()
    {
        float aspectRatio = Screen.width / (float)Screen.height;
        float verticalSize = 7 / 2f + 1;
        float horizontalSize = (5 / 2f + 1) / aspectRatio;
        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
    }
}

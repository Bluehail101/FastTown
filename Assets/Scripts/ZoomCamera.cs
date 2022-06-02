using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ZoomCamera : MonoBehaviour
{
    private PixelPerfectCamera pixelPerfectCamera;
    private int zoomLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        pixelPerfectCamera = gameObject.GetComponent<PixelPerfectCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        var scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelInput != 0)
        {
            zoomLevel += Mathf.RoundToInt(scrollWheelInput * 10);
            zoomLevel = Mathf.Clamp(zoomLevel, 1, 5);
            pixelPerfectCamera.refResolutionX = Mathf.FloorToInt(Screen.width / zoomLevel);
            pixelPerfectCamera.refResolutionY = Mathf.FloorToInt(Screen.height / zoomLevel);
        }
    }
}

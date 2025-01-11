using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    
    public Camera[] cameras; // Array of cameras to switch between
    private int currentCameraIndex = 0; // Index of the currently active camera

    private void Start()
    {
        // Ensure only the first camera is active at the start
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }
    }

    private void Update()
    {
        // Switch camera when the "C" key is pressed
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        // Deactivate the current camera
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Move to the next camera in the array
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // Activate the new camera
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}
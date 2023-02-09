using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCam : MonoBehaviour
{
    public float sensitivity;
    public bool invertX;
    public bool invertY;

    float verticalRotation;

    public Transform target;
    public Transform horizontalAxis;
    public Transform verticalAxis;

    public bool control;

    void Start() {
        sensitivity = PlayerLook.sensitivity;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        if (control) {
            UpdateDirection();
        }
        UpdatePosition();
    }

    public void ResetRotation() {
        horizontalAxis.rotation = target.rotation;
        verticalRotation = 25f;
        verticalAxis.localRotation = Quaternion.Euler(verticalRotation, 0, 0f);
    }
    
    public void UpdateDirection()
    {
        verticalRotation = Mathf.Clamp(verticalRotation + Input.GetAxis("VerticalCam") * sensitivity * (invertY ? -1 : 1) * Time.deltaTime, 0f, 85f);
        verticalAxis.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);

        horizontalAxis.Rotate(Vector3.up * Input.GetAxis("HorizontalCam") * sensitivity * (invertX ? -1 : 1) * Time.deltaTime, Space.World);
    }

    public void UpdatePosition() {
        horizontalAxis.position = target.position;
    }
}

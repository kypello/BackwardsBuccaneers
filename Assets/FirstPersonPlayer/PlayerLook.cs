using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform horizontalAxis;
    public Transform verticalAxis;
    public static float sensitivity = 500f;
    float xRotation = 0f;

    public bool control = true;
    Vector3 lockOnPoint;

    bool lookingAtPoint = false;
    bool mouseMovedSinceUnlocking;

    public bool clampY = false;
    public float xClamp = 90f;
    public float yClamp = 60f;

    void Awake() {
        #if UNITY_EDITOR
            sensitivity = 500f;
        #endif

        #if UNITY_WEBGL
            sensitivity = 40f;
        #endif

        #if UNITY_STANDALONE
            sensitivity = 300f;
        #endif
    }

    void Update()
    {
        if (control) {
            if (mouseMovedSinceUnlocking) {
                xRotation -= Input.GetAxis("VerticalCam") * sensitivity * Time.deltaTime;
                xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
                verticalAxis.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

                horizontalAxis.Rotate(Vector3.up * Input.GetAxis("HorizontalCam") * sensitivity * Time.deltaTime);
                if (clampY) {
                    horizontalAxis.localRotation = Quaternion.Euler(0f, Mathf.Clamp(Mathf.Repeat(horizontalAxis.localEulerAngles.y + 90f, 180f) - 90f, -yClamp, yClamp), 0f);
                }
            }
            else {
                if (Input.GetAxis("HorizontalCam") != 0f || Input.GetAxis("VerticalCam") != 0f) {
                    mouseMovedSinceUnlocking = true;
                }
            }
        }
        else {
            mouseMovedSinceUnlocking = false;
        }
    }

    public void ResetXRotation() {
        xRotation = 0f;
    }

    /*
    public IEnumerator LookAt(Vector3 point) {
        while (lookingAtPoint) {
            yield return null;
        }

        lookingAtPoint = true;

        Vector3 targetDir = (point - transform.position).normalized;

        float dotProduct;

        do {
            if (control) {
                break;
            }

            dotProduct = Vector3.Dot(transform.forward, targetDir);

            Vector3 delta = Vector3.RotateTowards(transform.forward, targetDir, (dotProduct * -dotProduct + 1f) * 2f * Mathf.PI * Time.deltaTime, 0f);

            transform.localRotation = Quaternion.LookRotation(delta);
            transform.localRotation = Quaternion.Euler(Vector3.right * transform.localEulerAngles.x);

            player.localRotation = Quaternion.LookRotation(delta);
            player.localRotation = Quaternion.Euler(Vector3.up * player.localEulerAngles.y);

            xRotation = Mathf.Repeat(transform.localEulerAngles.x + 90f, 180f) - 90f;

            yield return null;
        } while (dotProduct < 0.99f);

        lookingAtPoint = false;
    }
    */
}

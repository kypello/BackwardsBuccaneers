using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterShipMode : MonoBehaviour, Interactable
{
    public PlayerLook playerLook;
    public Player player;
    public PlayerInteract playerInteract;
    public Animation cameraAnim;
    public Transform cam;
    public Transform playerCamPoint;
    public Transform shipCamPoint;
    public Transform shipCamParent;
    public ShipCam shipCam;
    public ShipControl shipControl;

    public string prompt {
        get {
            return "[E] Use Wheel";
        }
    }

    void Start() {
        
    }

    public IEnumerator Interact() {
        yield return PlayerToShipMode();
    }

    public IEnumerator PlayerToShipMode() {
        player.control = false;
        playerLook.control = false;
        playerInteract.control = false;
        cameraAnim.Stop();
        shipCam.ResetRotation();

        cam.SetParent(null, true);

        float i = 0f;

        while (i < 1f) {
            yield return null;
            i += Time.deltaTime * 2.5f;
            cam.position = Vector3.Lerp(playerCamPoint.position, shipCamPoint.position, i);
            cam.rotation = Quaternion.Lerp(playerCamPoint.rotation, shipCamPoint.rotation, i);
        }

        cam.position = shipCamPoint.position;
        cam.rotation = shipCamPoint.rotation;
        cam.SetParent(shipCamParent, true);
        shipCam.control = true;
        shipControl.control = true;
    }

    public IEnumerator ShipToPlayerMode() {
        shipCam.control = false;
        shipControl.control = false;
        playerCamPoint.localRotation = Quaternion.identity;

        cam.SetParent(null, true);

        float i = 0f;

        while (i < 1f) {
            yield return null;
            i += Time.deltaTime * 2.5f;
            cam.position = Vector3.Lerp(shipCamPoint.position, playerCamPoint.position, i);
            cam.rotation = Quaternion.Lerp(shipCamPoint.rotation, playerCamPoint.rotation, i);
        }

        cam.SetParent(playerCamPoint, true);
        cam.localPosition = Vector3.zero;
        cam.rotation = playerCamPoint.rotation;
        playerLook.ResetXRotation();
        player.control = true;
        playerLook.control = true;
        playerInteract.control = true;
    }
}

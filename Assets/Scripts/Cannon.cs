using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour, Interactable
{
    public PlayerLook playerLook;
    public Player player;
    public PlayerInteract playerInteract;
    public Animation cameraAnim;
    public Transform cam;
    public Transform playerCamPoint;
    public Transform cannonCamPoint;
    public Transform cannonHorizAxis;
    public Transform cannonVertAxis;

    bool cannonActive = false;

    void Update() {
        if (cannonActive) {
            if (Input.GetKeyDown(KeyCode.E)) {
                StartCoroutine(CannonToPlayerMode());
            }
        }
    }

    public IEnumerator Interact() {
        yield return PlayerToCannonMode();
    }

    public IEnumerator PlayerToCannonMode() {
        player.control = false;
        playerLook.control = false;
        playerInteract.control = false;
        cameraAnim.Stop();
        cannonHorizAxis.localRotation = Quaternion.identity;
        cannonVertAxis.localRotation = Quaternion.identity;

        cam.SetParent(null, true);

        float i = 0f;

        while (i < 1f) {
            yield return null;
            i += Time.deltaTime * 2.5f;
            cam.position = Vector3.Lerp(playerCamPoint.position, cannonCamPoint.position, i);
            cam.rotation = Quaternion.Lerp(playerCamPoint.rotation, cannonCamPoint.rotation, i);
        }

        cam.SetParent(cannonVertAxis, true);
        cam.localPosition = cannonCamPoint.localPosition;
        cam.localRotation = Quaternion.identity;
        
        playerLook.ResetXRotation();
        playerLook.horizontalAxis = cannonHorizAxis;
        playerLook.verticalAxis = cannonVertAxis;
        playerLook.control = true;
        cannonActive = true;
    }

    public IEnumerator CannonToPlayerMode() {
        cannonActive = false;
        playerLook.control = false;
        playerCamPoint.localRotation = Quaternion.identity;

        cam.SetParent(null, true);

        float i = 0f;

        while (i < 1f) {
            yield return null;
            i += Time.deltaTime * 2.5f;
            cam.position = Vector3.Lerp(cannonCamPoint.position, playerCamPoint.position, i);
            cam.rotation = Quaternion.Lerp(cannonCamPoint.rotation, playerCamPoint.rotation, i);
        }

        cam.SetParent(playerCamPoint);
        cam.localPosition = Vector3.zero;
        cam.rotation = playerCamPoint.rotation;
        playerLook.horizontalAxis = player.transform;
        playerLook.verticalAxis = playerCamPoint;
        playerLook.ResetXRotation();
        player.control = true;
        playerLook.control = true;
        playerInteract.control = true;
    }
}

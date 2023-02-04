using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour, Interactable
{
    public PlayerLook playerLook;
    public Player player;
    public PlayerInteract playerInteract;
    public PlayerCarry playerCarry;
    public Animation cameraAnim;
    public Transform cam;
    public Transform playerCamPoint;
    public Transform cannonCamPoint;
    public Transform cannonHorizAxis;
    public Transform cannonVertAxis;
    public GameObject cannonUI;

    public Projectile[] projectilePrefabs;

    public ProjectileState loadedProjectile = ProjectileState.None;

    bool cannonActive = false;

    public string prompt {
        get {
            if (loadedProjectile == ProjectileState.None && playerCarry.carrying != ProjectileState.None) {
                return "Load Cannon";
            }
            else {
                return "Use Cannon";
            }
        }
    }

    void Update() {
        if (cannonActive) {
            if (Input.GetMouseButtonDown(0)) {
                if (loadedProjectile != ProjectileState.None) {
                    Fire(projectilePrefabs[(int)loadedProjectile]);
                    loadedProjectile = ProjectileState.None;
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                StartCoroutine(CannonToPlayerMode());
            }
        }
    }

    void Fire(Projectile projectilePrefab) {
        Projectile projectile = Instantiate(projectilePrefab, cannonCamPoint.position, Quaternion.identity);
        projectile.velocity = cannonCamPoint.forward * 60f;
    }

    public IEnumerator Interact() {
        if (loadedProjectile == ProjectileState.None && playerCarry.carrying != ProjectileState.None) {
            loadedProjectile = playerCarry.carrying;
            playerCarry.PickUp(ProjectileState.None);
        }
        else {
            yield return PlayerToCannonMode();
        }
        yield break;
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
        playerLook.clampY = true;
        playerLook.xClamp = 45f;
        playerLook.control = true;
        cannonUI.SetActive(true);
        cannonActive = true;
    }

    public IEnumerator CannonToPlayerMode() {
        cannonActive = false;
        playerLook.control = false;
        playerCamPoint.localRotation = Quaternion.identity;
        cannonUI.SetActive(false);

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
        playerLook.clampY = false;
        playerLook.xClamp = 90f;
        player.control = true;
        playerLook.control = true;
        playerInteract.control = true;
    }
}

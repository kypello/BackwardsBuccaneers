using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public AudioSource cannonSound;
    public Collider shipTarget;
    public ShipControl shipControl;
    public bool reverseShipTilt;

    public Projectile[] projectilePrefabs;

    public ProjectileState loadedProjectile = ProjectileState.None;

    public Image projectileIcon;
    public Sprite[] projectileIconSprites;
    public TMP_Text clickInstructionText;
    public Color grayedOutTextColor;
    public Material[] floatingIconMaterials;
    public Renderer floatingIcon;

    bool cannonActive = false;

    public string prompt {
        get {
            if (loadedProjectile == ProjectileState.None && playerCarry.carrying != ProjectileState.None) {
                return "[E] Load Cannon";
            }
            else {
                return "[E] Use Cannon";
            }
        }
    }

    void Update() {
        if (cannonActive) {
            if (Input.GetMouseButtonDown(0)) {
                if (loadedProjectile != ProjectileState.None) {
                    Fire(projectilePrefabs[(int)loadedProjectile]);
                    loadedProjectile = ProjectileState.None;

                    projectileIcon.sprite = projectileIconSprites[2];
                    projectileIcon.color = grayedOutTextColor;
                    clickInstructionText.color = grayedOutTextColor;
                    floatingIcon.enabled = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                StartCoroutine(CannonToPlayerMode());
            }
        }
    }

    void Fire(Projectile projectilePrefab) {
        shipControl.FireCannonTilt(reverseShipTilt);
        cannonSound.Play();
        Projectile projectile = Instantiate(projectilePrefab, cannonCamPoint.position, Quaternion.identity);
        projectile.velocity = cannonCamPoint.forward * 60f;
        projectile.originalShip = shipTarget;
    }

    public IEnumerator Interact() {
        if (loadedProjectile == ProjectileState.None && playerCarry.carrying != ProjectileState.None) {
            loadedProjectile = playerCarry.carrying;
            playerCarry.PickUp(ProjectileState.None);

            floatingIcon.enabled = true;
            floatingIcon.sharedMaterial = floatingIconMaterials[(int)loadedProjectile];
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

        projectileIcon.sprite = projectileIconSprites[(int)loadedProjectile];
        if (loadedProjectile == ProjectileState.None) {
            projectileIcon.color = grayedOutTextColor;
            clickInstructionText.color = grayedOutTextColor;
        }
        else {
            projectileIcon.color = Color.white;
            clickInstructionText.color = Color.white;
        }
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

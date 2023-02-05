using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public bool crosshairEnabled = false;
    public RectTransform crosshair;
    public Image crosshairImage;
    public TMP_Text crosshairText;
    public float interactRange = 4f;
    public bool control = true;
    public LayerMask interactLayer;
    public GameObject currentlyLookingAt = null;

    void Update() {
        if (crosshairEnabled) {
            crosshairImage.enabled = control;
        }
        
        RaycastHit hit;

        if (control && Physics.Raycast(transform.position, transform.forward, out hit, interactRange, interactLayer)) {
            if (crosshairEnabled && currentlyLookingAt != hit.collider.gameObject) {
                crosshair.sizeDelta = Vector2.one * 12;
                crosshairText.text = hit.collider.GetComponent<Interactable>().prompt;
                currentlyLookingAt = hit.collider.gameObject;
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                StartCoroutine(hit.collider.GetComponent<Interactable>().Interact());
                currentlyLookingAt = null;
            }
        }
        else {
            if (crosshairEnabled) {
                crosshair.sizeDelta = Vector2.one * 4;
                crosshairText.text = "";
                currentlyLookingAt = null;
            }
        }
    }
}

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

    void Update() {
        if (crosshairEnabled) {
            crosshairImage.enabled = control;
        }
        
        RaycastHit hit;

        if (control && Physics.Raycast(transform.position, transform.forward, out hit, interactRange, interactLayer)) {
            if (crosshairEnabled) {
                crosshair.sizeDelta = Vector2.one * 12;
                crosshairText.text = hit.collider.GetComponent<Interactable>().prompt;
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                StartCoroutine(hit.collider.GetComponent<Interactable>().Interact());
            }
        }
        else {
            if (crosshairEnabled) {
                crosshair.sizeDelta = Vector2.one * 4;
                crosshairText.text = "";
            }
        }
    }
}

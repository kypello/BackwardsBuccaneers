using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public bool crosshairEnabled = false;
    public RectTransform crosshair;
    public Image crosshairImage;
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
            }

            if (Input.GetMouseButtonDown(0)) {
                StartCoroutine(hit.collider.GetComponent<Interactable>().Interact());
            }
        }
        else {
            if (crosshairEnabled) {
                crosshair.sizeDelta = Vector2.one * 4;
            }
        }
    }
}

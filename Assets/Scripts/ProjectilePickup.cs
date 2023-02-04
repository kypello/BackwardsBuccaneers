using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePickup : MonoBehaviour, Interactable
{
    public ProjectileState pickupType;
    public PlayerCarry playerCarry;

    public string p;
    public string prompt {
        get {
            return p;
        }
    }

    public IEnumerator Interact() {
        playerCarry.PickUp(pickupType);
        yield break;
    }
}

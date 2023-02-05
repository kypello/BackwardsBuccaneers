using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePickup : MonoBehaviour, Interactable
{
    public ProjectileState pickupType;
    public PlayerCarry playerCarry;

    public string[] treasureWords;

    public string prompt {
        get {
            if (pickupType == ProjectileState.Cannonball) {
                return "[E] Take Cannonball";
            }
            else {
                return "[E] Take " + treasureWords[Random.Range(0, treasureWords.Length)];
            }
        }
    }

    public IEnumerator Interact() {
        playerCarry.PickUp(pickupType);
        yield break;
    }
}

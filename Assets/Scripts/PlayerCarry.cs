using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileState {
    Cannonball = 0,
    Gold = 1,
    None = 2
}
    
public class PlayerCarry : MonoBehaviour
{
    public ProjectileState carrying = ProjectileState.None;
    public GameObject[] itemHolds;
    public Animation pickupAnim;

    public void PickUp(ProjectileState pickup) {
        carrying = pickup;

        for (int i = 0; i < itemHolds.Length; i++) {
            itemHolds[i].SetActive(i == (int)carrying);
        }

        pickupAnim.Play();
    }
}

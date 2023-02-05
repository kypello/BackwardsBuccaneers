using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingChest : MonoBehaviour
{
    public GameObject goldParticles;
    Score score;

    void Awake() {
        score = GameObject.FindWithTag("Score").GetComponent<Score>();
    }

    public void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "ShipTarget" && col.GetComponent<ShipTarget>().shipControl.gameObject.tag == "PlayerShip") {
            score.ChangeScore(-1);
            Instantiate(goldParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

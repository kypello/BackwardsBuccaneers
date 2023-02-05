using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControlMerchant : ShipControl
{
    float timer = 0f;

    void Awake() {
        timer = Random.Range(5f, 20f);
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
    }

    protected override void TakeInput() {
        movementInput = 1;

        timer -= Time.deltaTime;

        if (timer <= 0f) {
            if (turnInput == 0) {
                turnInput = 1 + Random.Range(0, 2) * -2;
                timer = Random.Range(1f, 4f);
            }
            else {
                turnInput = 0;
                timer = Random.Range(5f, 20f);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControlMerchant : ShipControl
{
    float movementTimer = 0f;

    public Material whiteSail;
    public Material sadSail;
    public Material sadFlag;
    public Material greenSail;
    public Material happySail;
    public Material happyFlag;

    public Renderer backSail;
    public Renderer bigSail;
    public Renderer topSail;
    public Renderer flag;

    ShipControl playerShip;
    public AudioSource cashSound;
    Score score;

    bool satisfied = false;
    float satisfactionTimer = 0f;

    void Awake() {
        playerShip = GameObject.FindWithTag("PlayerShip").GetComponent<ShipControl>();
        score = GameObject.FindWithTag("Score").GetComponent<Score>();
        movementTimer = Random.Range(5f, 20f);
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        SetSadFlag();
    }

    protected override void TakeInput() {
        movementInput = 1;

        movementTimer -= Time.deltaTime;

        if (movementTimer <= 0f) {
            if (turnInput == 0) {
                turnInput = 1 + Random.Range(0, 2) * -2;
                movementTimer = Random.Range(1f, 4f);
            }
            else {
                turnInput = 0;
                movementTimer = Random.Range(5f, 20f);
            }
        }

        if (satisfied) {
            satisfactionTimer -= Time.deltaTime;
            if (satisfactionTimer <= 0f) {
                satisfied = false;
                SetSadFlag();
            }
        }
    }

    void SetSadFlag() {
        backSail.sharedMaterial = whiteSail;
        topSail.sharedMaterial = whiteSail;
        bigSail.sharedMaterial = sadSail;
        flag.sharedMaterial = sadFlag;
    }

    void SetHappyFlag() {
        backSail.sharedMaterial = greenSail;
        topSail.sharedMaterial = greenSail;
        bigSail.sharedMaterial = happySail;
        flag.sharedMaterial = happyFlag;
    }

    protected override void GoldHit(Collider originalShip) {
        if (!satisfied) {
            cashSound.Play();
            anim.Play("ShipJiggle");
            SetHappyFlag();
            score.ChangeScore(1);
            satisfied = true;
            satisfactionTimer = Random.Range(30f, 120f);

            if (originalShip.GetComponent<ShipTarget>().shipControl != playerShip) {
                (playerShip as ShipControlPlayer).UnlockGoldFlag();
            }
        }
    }
}

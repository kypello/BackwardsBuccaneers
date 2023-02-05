using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public float maxSpeed;
    public float speed;
    public float acceleration;
    public float maxAngularVelocity;
    public float angularVelocity;
    public float angularAcceleration;
    public float decollideForce;

    List<Transform> collidingShips = new List<Transform>();

    public Transform shipTilt;
    public Transform wheel;

    float wheelNoiseX;
    float wheelNoiseY;
    float wheelNoiseTime = 0f;

    float bobNoiseX;
    float bobNoiseY;
    float bobNoiseTime = 0f;
    public Animation anim;
    bool animating = false;

    protected int movementInput;
    protected int turnInput;

    public Collider detectorCollider;
    public Collider targetCollider;

    void Start() {
        wheelNoiseX = Random.Range(-100000f, 100000f);
        wheelNoiseY = Random.Range(-100000f, 100000f);
    }

    protected virtual void TakeInput() {

    }

    protected virtual void PostMovement() {

    }

    void Update() {
        TakeInput();
        Movement();
        PostMovement();
    }

    void Movement() {
        if (movementInput == 1) {
            speed = Mathf.Min(speed + acceleration * Time.deltaTime, maxSpeed);
        }
        else if (movementInput == -1) {
            speed = Mathf.Max(speed - acceleration * Time.deltaTime, -maxSpeed);
        }
        else {
            if (speed > 0f) {
                speed = Mathf.Max(speed - acceleration * Time.deltaTime, 0f);
            }
            else if (speed < 0f) {
                speed = Mathf.Min(speed + acceleration * Time.deltaTime, 0f);
            }
        }

        if (turnInput == 1) {
            angularVelocity = Mathf.Min(angularVelocity + angularAcceleration * Time.deltaTime, maxAngularVelocity);
        }
        else if (turnInput == -1) {
            angularVelocity = Mathf.Max(angularVelocity - angularAcceleration * Time.deltaTime, -maxAngularVelocity);
        }
        else {
            if (angularVelocity > 0f) {
                angularVelocity = Mathf.Max(angularVelocity - angularAcceleration * Time.deltaTime, 0f);
            }
            else if (angularVelocity < 0f) {
                angularVelocity = Mathf.Min(angularVelocity + angularAcceleration * Time.deltaTime, 0f);
            }
        }

        if (!animating) {
            shipTilt.localRotation = Quaternion.Euler(Vector3.forward * angularVelocity * -0.2f);
        }

        wheel.Rotate(Vector3.forward * angularVelocity * 5f * Time.deltaTime);
        
        wheelNoiseTime += Time.deltaTime;
        float noiseValue = Mathf.PerlinNoise((wheelNoiseX + wheelNoiseTime) / 10f, wheelNoiseY / 10f);
        wheel.Rotate(Vector3.forward * (noiseValue * 30f - 15f) * Time.deltaTime);

        transform.Rotate(Vector3.up * angularVelocity * Time.deltaTime);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        foreach (Transform ship in collidingShips) {
            transform.Translate((transform.position - ship.position).normalized * decollideForce * Mathf.Pow(Vector3.Distance(transform.position, ship.position) / -28f + 2f, 2f) * Time.deltaTime, Space.World);
        }
    }

    public void ShipAlert(Transform ship) {
        collidingShips.Add(ship);
    }

    public void ShipLeaveAlert(Transform ship) {
        collidingShips.Remove(ship);
    }

    public void ProjectileHit(ProjectileState projectileState) {
        Debug.Log("Hit by " + projectileState);
        if (projectileState == ProjectileState.Cannonball) {
            CannonballHit();
        }
        else if (projectileState == ProjectileState.Gold) {
            GoldHit();
        }
    }

    protected virtual void CannonballHit() {
        detectorCollider.enabled = false;
        targetCollider.enabled = false;

        anim.Play(new string[]{"Sink1", "Sink2", "Sink3"}[Random.Range(0, 3)]);

        animating = true;
        Destroy(gameObject, 4f);
    }

    protected virtual void GoldHit() {

    }
}

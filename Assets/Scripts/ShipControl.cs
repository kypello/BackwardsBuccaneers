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

    bool cannonFiring;
    float cannonTiltVelocity = 0f;
    public float cannonTiltFireVelocity = 60f;
    public float minCannonTiltVelocity = -15f;
    public float cannonTiltAcceleration = 120f;
    float cannonTiltAngle = 0f;
    float cannonTiltDir = 1;

    public ShipSpawner spawner;

    public void FireCannonTilt(bool reverse) {
        cannonTiltVelocity = cannonTiltFireVelocity;
        cannonFiring = true;
        if (reverse) {
            cannonTiltDir = -1f;
        }
        else {
            cannonTiltDir = 1f;
        }
    }

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
            if (cannonFiring) {
                cannonTiltVelocity = Mathf.Max(cannonTiltVelocity - cannonTiltAcceleration * Time.deltaTime, minCannonTiltVelocity);
                cannonTiltAngle = Mathf.Max(cannonTiltAngle + cannonTiltVelocity * Time.deltaTime, 0f);

                if (cannonTiltAngle <= 0f) {
                    cannonFiring = false;
                }
            }

            shipTilt.localRotation = Quaternion.Euler(Vector3.forward * (angularVelocity * -0.2f + cannonTiltAngle * cannonTiltDir));
        }

        wheel.Rotate(Vector3.forward * angularVelocity * 5f * Time.deltaTime);
        
        wheelNoiseTime += Time.deltaTime;
        float noiseValue = Mathf.PerlinNoise((wheelNoiseX + wheelNoiseTime) / 10f, wheelNoiseY / 10f);
        wheel.Rotate(Vector3.forward * (noiseValue * 30f - 15f) * Time.deltaTime);

        transform.Rotate(Vector3.up * angularVelocity * Time.deltaTime);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        foreach (Transform ship in collidingShips) {
            if (ship == null) {
                continue;
            }
            transform.Translate((transform.position - ship.position).normalized * decollideForce * Mathf.Pow(Vector3.Distance(transform.position, ship.position) / -28f + 2f, 2f) * Time.deltaTime, Space.World);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -360f, 360f), 5f, Mathf.Clamp(transform.position.z, -360f, 360f));
    }

    public void ShipAlert(Transform ship) {
        collidingShips.Add(ship);
    }

    public void ShipLeaveAlert(Transform ship) {
        collidingShips.Remove(ship);
    }

    public void ProjectileHit(ProjectileState projectileState, Collider originalShip) {
        Debug.Log("Hit by " + projectileState);
        if (projectileState == ProjectileState.Cannonball) {
            CannonballHit();
        }
        else if (projectileState == ProjectileState.Gold) {
            GoldHit(originalShip);
        }
    }

    protected virtual void CannonballHit() {
        detectorCollider.enabled = false;
        targetCollider.enabled = false;

        anim.Play(new string[]{"Sink1", "Sink2", "Sink3"}[Random.Range(0, 3)]);

        if (this is ShipControlEnemy) {
            spawner.ReportDeadEnemy();
        }
        else if (this is ShipControlMerchant) {
            spawner.ReportDeadMerchant();
        }

        animating = true;
        Destroy(gameObject, 4f);
    }

    protected virtual void GoldHit(Collider originalShip) {

    }
}

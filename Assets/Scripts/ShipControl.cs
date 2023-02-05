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

    public Transform shipTilt;
    public Transform wheel;

    float wheelNoiseX;
    float wheelNoiseY;
    float wheelNoiseTime = 0f;

    float bobNoiseX;
    float bobNoiseY;
    float bobNoiseTime = 0f;

    protected int movementInput;
    protected int turnInput;

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

        shipTilt.localRotation = Quaternion.Euler(Vector3.forward * angularVelocity * -0.2f);

        wheel.Rotate(Vector3.forward * angularVelocity * 5f * Time.deltaTime);
        
        wheelNoiseTime += Time.deltaTime;
        float noiseValue = Mathf.PerlinNoise((wheelNoiseX + wheelNoiseTime) / 10f, wheelNoiseY / 10f);
        wheel.Rotate(Vector3.forward * (noiseValue * 30f - 15f) * Time.deltaTime);

        transform.Rotate(Vector3.up * angularVelocity * Time.deltaTime);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
}

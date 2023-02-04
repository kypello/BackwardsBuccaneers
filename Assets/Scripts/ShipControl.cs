using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public bool control;

    public float maxSpeed;
    public float speed;
    public float acceleration;
    public float maxAngularVelocity;
    public float angularVelocity;
    public float angularAcceleration;

    public Transform shipModel;
    public CharacterController player;

    public EnterShipMode shipMode;

    void Update() {
        if (control && Input.GetAxis("Vertical") > 0f) {
            speed = Mathf.Min(speed + acceleration * Time.deltaTime, maxSpeed);
        }
        else if (control && Input.GetAxis("Vertical") < 0f) {
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

        if (control && Input.GetAxis("Horizontal") > 0f) {
            angularVelocity = Mathf.Min(angularVelocity + angularAcceleration * Time.deltaTime, maxAngularVelocity);
        }
        else if (control && Input.GetAxis("Horizontal") < 0f) {
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

        if (control && Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine(shipMode.ShipToPlayerMode());
        }

        shipModel.localRotation = Quaternion.Euler(Vector3.forward * angularVelocity * -0.2f);

        transform.Rotate(Vector3.up * angularVelocity * Time.deltaTime);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        if (speed != 0f) {
            player.Move(transform.forward * speed * Time.deltaTime);
        }
        if (angularVelocity != 0f) {
            float dist = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(player.transform.position.x, 0f, player.transform.position.z));

            Vector3 dirToPlayer = new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z).normalized;

            player.Move(Vector3.Cross(Vector3.up, dirToPlayer) * dist * angularVelocity * Mathf.Deg2Rad * Time.deltaTime);
            player.transform.Rotate(Vector3.up * angularVelocity * Time.deltaTime);
        }
    }
}

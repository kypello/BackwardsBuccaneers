using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControlPlayer : ShipControl
{
    public bool control;
    public CharacterController player;
    public EnterShipMode shipMode;

    void Awake() {
        Physics.IgnoreCollision(player, GetComponent<Collider>());
    }

    protected override void TakeInput() {
        if (control && Input.GetAxis("Vertical") > 0f) {
            movementInput = 1;
        }
        else if (control && Input.GetAxis("Vertical") < 0f) {
            movementInput = -1;
        }
        else {
            movementInput = 0;
        }

        if (control && Input.GetAxis("Horizontal") > 0f) {
            turnInput = 1;
        }
        else if (control && Input.GetAxis("Horizontal") < 0f) {
            turnInput = -1;
        }
        else {
            turnInput = 0;
        }

        if (control && Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine(shipMode.ShipToPlayerMode());
        }
    }

    protected override void PostMovement() {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 velocity;
    public float airResistance = 10f;
    public float gravity = -40f;

    void Start() {
        
    }

    void Update() {
        velocity += -velocity.normalized * airResistance * Time.deltaTime;
        velocity += Vector3.up * gravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime, Space.World);

        if (transform.position.y < 0) {
            Destroy(gameObject);
        }
    }
}

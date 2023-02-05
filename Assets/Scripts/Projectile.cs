using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 velocity;
    public float airResistance = 10f;
    public float gravity = -40f;
    public ProjectileState projectileState;
    public GameObject hitParticles;
    public GameObject splashParticles;
    public Collider originalShip;
    bool destroying = false;
    public ParticleSystem trailParticles;
    public Renderer rend;
    public FloatingChest floatingChestPrefab;

    void Update() {
        velocity += -velocity.normalized * airResistance * Time.deltaTime;
        velocity += Vector3.up * gravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime, Space.World);

        if (transform.position.y < 0 && !destroying) {
            destroying = true;
            Instantiate(splashParticles, Vector3.right * transform.position.x + Vector3.forward * transform.position.z, Quaternion.identity);
            if (projectileState == ProjectileState.Gold) {
                GameObject.FindWithTag("Spawner").GetComponent<ShipSpawner>().ChestSpawnedIndependently();
                Instantiate(floatingChestPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject, 4f);
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "ShipTarget" && col != originalShip && !destroying) {
            destroying = true;
            rend.enabled = false;
            if (trailParticles != null) {
                trailParticles.Stop();
            }
            col.GetComponent<ShipTarget>().shipControl.ProjectileHit(projectileState, originalShip);
            Instantiate(hitParticles, transform.position, Quaternion.identity);
            Destroy(gameObject, 4f);
        }
    }
}

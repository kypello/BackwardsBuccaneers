using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControlEnemy : ShipControl
{
    public Transform target;
    public float playerChaseRange;
    public float playerTargetRange;

    float cannonCooldown = -1f;

    float roamTimer = 0f;

    public Transform backRightCannon;
    public Transform frontRightCannon;
    public Transform backLeftCannon;
    public Transform frontLeftCannon;
    public Projectile goldPrefab;

    public ParticleSystem backRightParticles;
    public ParticleSystem frontRightParticles;
    public ParticleSystem backLeftParticles;
    public ParticleSystem frontLeftParticles;

    public AudioSource cannonSound;
    
    void Awake() {
        roamTimer = Random.Range(5f, 20f);
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
    }

    protected override void TakeInput() {
        if (cannonCooldown > 0f) {
            cannonCooldown -= Time.deltaTime;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < playerTargetRange) {
            Attack();
        }
        else if (distanceToTarget < playerChaseRange) {
            Chase();
        }
        else {
            Roam();
        }
    }

    void Roam() {
        movementInput = 1;

        roamTimer -= Time.deltaTime;

        if (roamTimer <= 0f) {
            if (turnInput == 0) {
                turnInput = 1 + Random.Range(0, 2) * -2;
                roamTimer = Random.Range(1f, 4f);
            }
            else {
                turnInput = 0;
                roamTimer = Random.Range(5f, 20f);
            }
        }
    }

    void Chase() {
        Vector3 playerDir = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, playerDir);

        if (dot < 0.75f || dot < 0.9f && Mathf.Abs(angularVelocity) < 10f) {
            if (Vector3.Dot(transform.right, playerDir) > Vector3.Dot(-transform.right, playerDir)) {
                turnInput = 1;
            }
            else {
                turnInput = -1;
            }
        }
        else {
            turnInput = 0;
        }

        movementInput = 1;
    }

    void Attack() {
        Vector3 playerDir = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, playerDir);

        if (Mathf.Abs(dot) < 0.25f && Mathf.Abs(angularVelocity) > 10f) {
            turnInput = 0;

            if (Mathf.Abs(dot) < 0.1f && cannonCooldown <= 0f) {
                bool back = dot < 0f;
                bool right = Vector3.Dot(transform.right, playerDir) > dot;
                FireCannon(back, right);
            }
        }
        else {
            if (Vector3.Dot(transform.right, playerDir) > Vector3.Dot(-transform.right, playerDir)) {
                turnInput = -1;
            }
            else {
                turnInput = 1;
            }
        }

        movementInput = 1;
    }

    void FireCannon(bool back, bool right) {
        Debug.Log("Fire!!!");

        cannonSound.Play();

        Projectile projectile;
        if (back && right) {
            backRightParticles.Play();
            projectile = Instantiate(goldPrefab, backRightCannon.position, Quaternion.identity);
            projectile.velocity = backRightCannon.forward * 60f;
        }
        else if (!back && right) {
            frontRightParticles.Play();
            projectile = Instantiate(goldPrefab, frontRightCannon.position, Quaternion.identity);
            projectile.velocity = frontRightCannon.forward * 60f;
        }
        if (back && !right) {
            backLeftParticles.Play();
            projectile = Instantiate(goldPrefab, backLeftCannon.position, Quaternion.identity);
            projectile.velocity = backLeftCannon.forward * 60f;
        }
        if (!back && !right) {
            frontLeftParticles.Play();
            projectile = Instantiate(goldPrefab, frontLeftCannon.position, Quaternion.identity);
            projectile.velocity = frontLeftCannon.forward * 60f;
        }

        FireCannonTilt(!right);

        cannonCooldown = 4f;
    }
}

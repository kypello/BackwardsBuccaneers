using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingChest : MonoBehaviour
{
    public GameObject goldParticles;
    public Animation anim;
    Score score;

    float lifetime;
    bool sinking = false;

    ShipSpawner spawner;

    void Awake() {
        score = GameObject.FindWithTag("Score").GetComponent<Score>();
        spawner = GameObject.FindWithTag("Spawner").GetComponent<ShipSpawner>();
        lifetime = Random.Range(15f, 45f);
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        anim.Play("ChestFloatToSurface");
        StartCoroutine(BobAnimDelay());
    }

    IEnumerator BobAnimDelay() {
        yield return new WaitForSeconds(1f);
        anim.Play("ChestBob");
    }

    void Update() {
        if (lifetime > 0f) {
            lifetime -= Time.deltaTime;
        }
        else if (!sinking) {
            StartCoroutine(Sink());
        }
    }

    IEnumerator Sink() {
        sinking = true;
        anim.Play("ChestSink");
        yield return new WaitForSeconds(1f);
        spawner.ReportDead(SpawnableType.Chest);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "ShipTarget" && col.GetComponent<ShipTarget>().shipControl.gameObject.tag == "PlayerShip") {
            score.ChangeScore(-1);
            Instantiate(goldParticles, transform.position, Quaternion.identity);
            spawner.ReportDead(SpawnableType.Chest);
            Destroy(gameObject);
        }
    }
}

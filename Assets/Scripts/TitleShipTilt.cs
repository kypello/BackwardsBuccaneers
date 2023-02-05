using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleShipTilt : MonoBehaviour
{
    public Transform ship;

    float tiltNoiseX;
    float tiltNoiseY;
    float tiltNoiseT;

    float tiltNoiseBX;
    float tiltNoiseBY;

    float bobNoiseX;
    float bobNoiseY;

    void Awake() {
        tiltNoiseX = Random.Range(-100000f, 100000f);
        tiltNoiseY = Random.Range(-100000f, 100000f);

        tiltNoiseBX = Random.Range(-100000f, 100000f);
        tiltNoiseBY = Random.Range(-100000f, 100000f);

        bobNoiseX = Random.Range(-100000f, 100000f);
        bobNoiseY = Random.Range(-100000f, 100000f);
    }

    void Update() {
        tiltNoiseT += Time.deltaTime;

        float tiltSample = Mathf.PerlinNoise((tiltNoiseX + tiltNoiseT) / 4f, tiltNoiseY / 4f);
        float tiltSampleB = Mathf.PerlinNoise((tiltNoiseBX + tiltNoiseT) / 4f, tiltNoiseBY / 4f);
        ship.localRotation = Quaternion.Euler(Vector3.right * (tiltSample * 14f - 7f) + Vector3.forward * (tiltSampleB * 14f - 7f));

        float bobSample = Mathf.PerlinNoise((bobNoiseX + tiltNoiseT) / 4f, bobNoiseY / 4f);
        ship.localPosition = Vector3.up * bobSample * 4f;
    }
}

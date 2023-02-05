using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagFlap : MonoBehaviour
{
    static float noiseX;
    static float noiseY;
    static float time;
    float[] noiseValues;

    public static Quaternion windDirection = Quaternion.LookRotation(Vector3.forward);

    // Start is called before the first frame update
    void Start()
    {
        noiseX = Random.Range(-100000f, 100000f);
        noiseY = Random.Range(-100000f, 100000f);
        time = 0f;
        noiseValues = new float[36];
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * 10f;

        for (int y = 0; y < 6; y++) {
            for (int x = 0; x < 6; x++) {
                float noiseValue;
                if (x == 0) {
                    noiseValue = 0f;
                }
                else {
                    noiseValue = Mathf.PerlinNoise((noiseX + time + (float)x) / 5f, (noiseY + (float)y) / 5f) * 5f - 2.5f;
                }
                noiseValues[y * 6 + x] = noiseValue;
            }
        }

        Shader.SetGlobalFloatArray("_FlagNoiseValues", noiseValues);
    }
}

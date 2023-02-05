using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    void Update() {
        transform.rotation = FlagFlap.windDirection;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDetector : MonoBehaviour
{
    public ShipControl shipControl;

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Ship") {
            shipControl.ShipAlert(col.transform);
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Ship") {
            shipControl.ShipLeaveAlert(col.transform);
        }
    }
}

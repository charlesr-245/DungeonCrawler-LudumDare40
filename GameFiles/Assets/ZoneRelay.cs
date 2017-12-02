using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneRelay : MonoBehaviour {

    private BoxCollider zone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.name == "Level")
        {
            zone = other.GetComponent<BoxCollider>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        zone = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (zone == null)
        {
            if (other.transform.parent.name == "Level")
            {
                zone = other.GetComponent<BoxCollider>();
            }
        }
    }

    public BoxCollider GetZone()
    {
        return zone;
    }

}

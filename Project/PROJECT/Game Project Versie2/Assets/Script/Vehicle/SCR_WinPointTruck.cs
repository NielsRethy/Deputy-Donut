using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WinPointTruck : MonoBehaviour {

	// Use this for initialization
    private bool _active = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "DonutTruck" && _active)
        {
            GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<SCR_RoundObjectSpawner>().deleteOutArray(gameObject);
            _active = false;
            Destroy(gameObject);
        }

    }
}

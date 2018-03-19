using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SpikeDamage : MonoBehaviour {
   private GameObject _Truck;

    // Use this for initialization
    void Start () {
        _Truck = GameObject.FindWithTag("DonutTruck");
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DonutTruck")
        {
            other.GetComponent<SCR_TruckDestructionManager>().TakeDamage(1);
            gameObject.active = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Items : MonoBehaviour
{

    // Use this for initialization
    GameObject[] PolicePowerUps;
    GameObject[] TruckPowerups;

    bool policeCollided = false;
    bool truckCollided = false;

    GameObject Truck;
    GameObject Police;

    void Start()
    {
        PolicePowerUps = GameObject.FindGameObjectsWithTag("Police_Pickup");
        TruckPowerups = GameObject.FindGameObjectsWithTag("Truck_Pickup");

        Truck = GameObject.FindGameObjectWithTag("DonutTruck");
        Police = GameObject.FindGameObjectWithTag("PoliceCar");
    }

    // Update is called once per frame
    void Update()
    {
        if (policeCollided)
        {
            int random = Random.Range(0, PolicePowerUps.Length);
            policeCollided = false;
        }

        if (truckCollided)
        {
            int random = Random.Range(0, TruckPowerups.Length);
            truckCollided = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other == Truck)
        {
            policeCollided = true;
        }
        else if(other == Police)
        {
            truckCollided = true;
        }
    }
}

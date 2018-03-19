using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FlameThrower : MonoBehaviour {
    public float DamagerPerTick = 10;
    public float TickPerSecond = 2;

    private bool _TruckInRange = false;

    private float _TimeSinceLastTick;
    private GameObject _Truck;
    private void Start()
    {
        _Truck = GameObject.FindGameObjectWithTag("DonutTruck");
    }
    private void Update()
    {
     if(_TruckInRange)
        {
            _TimeSinceLastTick += Time.deltaTime;
            if(_TimeSinceLastTick > 1/TickPerSecond)
            {
                _Truck.GetComponent<SCR_TruckDestructionManager>().TakeDamage(DamagerPerTick);
                _TimeSinceLastTick = 0;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "DonutTruck")
        {
            _TruckInRange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DonutTruck")
        {
            _TruckInRange = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DonutTruck")
        {
            _TruckInRange = true;
        }
    }
}

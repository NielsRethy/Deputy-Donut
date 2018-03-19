using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Molotov : SCR_AbilityBase
{
    public float DamagePerSecond = 5;
    public float Radius = 1;
    public float AliveTime = 10;
    private GameObject Truck;
    private bool _Lock = false;
    private float _TimeAlive = 0;
	void Start ()
    {
        Truck = GameObject.FindGameObjectWithTag("DonutTruck");
        gameObject.transform.localScale = new Vector3(Radius,0.1f,Radius);
        Physics.IgnoreLayerCollision(10, 12);
        Physics.IgnoreLayerCollision(11, 12);


    }

    void Update ()
    {

        Vector3 truckPos = Truck.transform.position;
        Vector3 molotovPos = gameObject.transform.position;
        Vector3 distance = truckPos - molotovPos;
        float distanceFloat = Mathf.Sqrt(Mathf.Pow(distance.x, 2)+ Mathf.Pow(distance.z, 2));
        _TimeAlive += Time.deltaTime;
        if (_TimeAlive > AliveTime)
        {
            _TimeAlive = 0;
            gameObject.SetActive(false);
        }
        if (Mathf.Abs(distanceFloat)<Radius/2 + 0.75)
        {
            Truck.GetComponent<SCR_TruckDestructionManager>().TakeDamage(DamagePerSecond * Time.deltaTime);

        }
    }

}

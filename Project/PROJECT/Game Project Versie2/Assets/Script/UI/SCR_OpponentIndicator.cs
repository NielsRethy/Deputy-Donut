using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_OpponentIndicator : MonoBehaviour {

    GameObject Police;
    GameObject Truck;

    public string PoliceTag = "PoliceCar";
    public string TruckTag = "DonutTruck";

	// Use this for initialization
	void Start ()
    {
        Police = GameObject.FindGameObjectWithTag(PoliceTag);
        Truck = GameObject.FindGameObjectWithTag(TruckTag);
	}
	
	// Update is called once per frame
	void Update ()
    {
        float angle = Vector3.Angle(Police.transform.position.normalized, Truck.transform.position.normalized);
        Debug.Log(angle);
	}
}

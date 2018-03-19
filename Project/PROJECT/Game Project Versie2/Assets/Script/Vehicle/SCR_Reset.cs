using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Reset : MonoBehaviour {

    // Use this for initialization
    public Transform[] safePoints;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (transform.CompareTag("PoliceCar"))
	    {
	        if (SCR_ButtonMaster.Player1 == "Police")
	        {
                if (Input.GetButtonDown(SCR_ButtonMaster.Master.Player1Reset))
                {
                    // put this in a different function for general cleanliness
                    ResetCar();
                }

            }
            else if (SCR_ButtonMaster.Player2 == "Police")
            {
                if (Input.GetButtonDown(SCR_ButtonMaster.Master.Player2Reset))
                {
                    // put this in a different function for general cleanliness
                    ResetCar();
                }

            }
        }
        else if (transform.CompareTag("DonutTruck"))
        {
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                if (Input.GetButtonDown(SCR_ButtonMaster.Master.Player1Reset))
                {
                    // put this in a different function for general cleanliness
                    ResetCar();
                }

            }
            else if (SCR_ButtonMaster.Player2 == "Truck")
            {
                if (Input.GetButtonDown(SCR_ButtonMaster.Master.Player2Reset))
                {
                    // put this in a different function for general cleanliness
                    ResetCar();
                }

            }
        }
    }
    void ResetCar()
    {
        // first, find the closest safe place
        Transform closestTransform = transform;
        float closestDistance = 9999999999;
        Vector3 currentPos = transform.position;
        // This goes through every possible safe place and picks the best one
        foreach (Transform trans in safePoints)
        {
            float currentDistance = Vector3.Distance(currentPos, trans.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestTransform = trans;
            }
        }

        // Now we reset the car!
        transform.position = closestTransform.position;
        transform.rotation = closestTransform.rotation;

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}

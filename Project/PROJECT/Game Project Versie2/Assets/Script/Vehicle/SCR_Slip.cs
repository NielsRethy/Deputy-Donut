using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Slip : MonoBehaviour {

	// Use this for initialization
    private GameObject _car;
	void Start () {
        _car = GameObject.FindWithTag("PoliceCar");
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PoliceCar")
        {
            other.GetComponent<SCR_Vehicle>().Slip = true;
        }
    }

   
}

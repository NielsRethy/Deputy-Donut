using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TurnArrow : MonoBehaviour {

    // Use this for initialization
    private GameObject target;
    private Vector3 targetPoint;
    private Quaternion targetRotation;
    void Start () {
        target = GameObject.FindWithTag("PoliceCar");
    }
	
	// Update is called once per frame
	void Update ()
	{
        var v3 = transform.eulerAngles;
	    v3.y = target.transform.eulerAngles.y -90;

        transform.eulerAngles = v3;
	}
}

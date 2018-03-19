using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TextMovement : MonoBehaviour {
    public float AliveTime = 2;
    public float speed = 1;
	void Start ()
    {
		
	}
	
	void Update ()
    {
        AliveTime -= Time.deltaTime;
        if(AliveTime<0)
        {
            Destroy (this.gameObject);
        }

        this.gameObject.transform.Translate(0, speed * Time.deltaTime, 0);
	}
}

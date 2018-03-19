using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CameraPos : MonoBehaviour {

    public Transform LookAtTransform;
    public float TimeToLerp;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, LookAtTransform.position);
        Gizmos.DrawSphere(LookAtTransform.position, 0.3f);
    }
}

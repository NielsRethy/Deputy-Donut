using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MoveUpAndDown : MonoBehaviour {

    public float Speed = 3f;
    public float MaxHeight = 0.015f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Move up and down
        Vector3 position = transform.position;
        float newHeight = MaxHeight * Mathf.Sin(Speed * Time.time) + position.y;
        transform.position = new Vector3(position.x, newHeight, position.z);
	}
}

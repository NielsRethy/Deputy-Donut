﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FlipOver : MonoBehaviour {

    public float velocityMinimum = 2.0f;

    public float TriggerTime = 2.0f;
    public List<WheelCollider> WheelCollidersList = new List<WheelCollider>();
    public Transform RoofPosition;

    private float _timeFlippedOver = 0.0f;
    private Rigidbody _rb;


	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
    void Update()
    {


        //Check if all 4 wheels are not grounded
        int wheelsGroundedCount = 0;
        foreach (WheelCollider wheelColl in WheelCollidersList)
        {
            if (wheelColl.isGrounded)
                ++wheelsGroundedCount;
        }

        // Check if car is flipped on roof
        if (wheelsGroundedCount == 0)
        {
            // Check if car is on flipped over
            RaycastHit hit;
            Vector3 rayDirection = transform.up * 0.2f;

            if (Physics.Raycast(RoofPosition.position, rayDirection, out hit, 100.0f))
            {
                _timeFlippedOver += Time.deltaTime;
                Debug.Log("On roof");
            }
            else _timeFlippedOver = 0.0f; // If not on roof


            if (_timeFlippedOver > TriggerTime)
            {
                FlipOver();
                _timeFlippedOver = 0.0f;
            }
        }
        else _timeFlippedOver = 0.0f; // If any wheel is grounded

    }

    private void FlipOver()
    {
        Debug.Log("Flip over");
        float explosionForce = 10000.0f;
        _rb.AddForceAtPosition(-transform.up * explosionForce, RoofPosition.position, ForceMode.Impulse);
    }
}

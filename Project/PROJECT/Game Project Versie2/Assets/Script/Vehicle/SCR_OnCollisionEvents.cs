using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;       // Action<,>


[RequireComponent(typeof(Rigidbody))]

public class SCR_OnCollisionEvents : MonoBehaviour {

    public event Action<Collision> onCollisionEnter = delegate { };
    public event Action<Collision> onCollisionExit = delegate { };
    public event Action<Collision> onCollisionStay = delegate { };

    public event Action onCollisionWithTruck = delegate { };

    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter(collision);
        if (collision.gameObject.CompareTag("DonutTruck"))
        {
            onCollisionWithTruck();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        onCollisionExit(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        onCollisionStay(collision);
    }
}

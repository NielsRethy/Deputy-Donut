using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DamagePart : MonoBehaviour
{
    public Mesh MeshWheels;
    public bool Swap = false;

    public float WheelVelocity = 10;
    private int _Counter = 0;
	// Use this for initialization
	void Start ()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="PoliceCar")
        {
            Swap = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == "DamWheel")
        { 
            if (collision.relativeVelocity.magnitude > WheelVelocity)
            {

                collision.collider.GetComponent<MeshFilter>().mesh = MeshWheels;
                collision.collider.GetComponent<Transform>().localScale = new Vector3(0.5f,0.5f,0.5f);
            }
        }
    }
}

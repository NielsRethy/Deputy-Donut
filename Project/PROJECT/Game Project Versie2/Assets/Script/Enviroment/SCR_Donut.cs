using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Donut : MonoBehaviour {

    public float TimeToDestroy = 2;

    private float _BounceTime = 0;
    private bool _InTruck = false;
    private bool _OnGround = false;
	// Update is called once per frame
	void Update () {
		if(_OnGround)
        {
            _BounceTime += Time.deltaTime;
            if(_BounceTime>TimeToDestroy)
            {
                _BounceTime = 0;
                _InTruck = false;
                _OnGround = false;
                this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                this.gameObject.GetComponent<BoxCollider>().isTrigger = false;

                this.gameObject.SetActive(false);
                this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

        }
        
	}
    public bool IsInTruck()
    {
        return _InTruck;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            _InTruck = false;
            _OnGround = true;
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            
        }
    }
}

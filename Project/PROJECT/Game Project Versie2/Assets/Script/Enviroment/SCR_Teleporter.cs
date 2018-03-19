using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Teleporter : MonoBehaviour {
    public SCR_Teleporter Target;
    private Transform _TeleportTarget;
    private Transform _TeleportLocation;
    private Vector3 _Rotation;
    private Transform _OwnTarget;
	// Use this for initialization
	void Start () {
        _TeleportTarget = Target.transform.GetChild(0);
        _TeleportLocation = Target.transform.GetChild(1);
        _OwnTarget = transform.GetChild(0);





    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        Vector3 distance = _OwnTarget.position - other.transform.position;
        other.transform.position = _TeleportLocation.position - distance;
        Vector3 speed = other.GetComponent<Rigidbody>().velocity;
        
        Vector3 rotation = Target.transform.rotation.eulerAngles - transform.rotation.eulerAngles;


        other.transform.Rotate(rotation.x, rotation.y, rotation.z);
        other.GetComponent<Rigidbody>().velocity = Quaternion.Euler(rotation.x, rotation.y, rotation.z)* speed;
    }
}

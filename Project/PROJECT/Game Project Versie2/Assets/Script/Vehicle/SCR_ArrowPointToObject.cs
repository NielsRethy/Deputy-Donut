using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ArrowPointToObject : MonoBehaviour {

	// Use this for initialization
    private GameObject _followObject = null;
    public float RotateSpeed = 1000.0f;

    public GameObject FollowObject
    {
        get { return _followObject; }
        set { _followObject = value; }
    }

    void Start () {
        if (gameObject.GetComponentInParent<SRC_CarControllerInput>().Police)
        {
            if (SCR_ButtonMaster.Player1 == "Police")
            {
                _followObject = GameObject.FindWithTag("DonutTruck");
            }
            else if (SCR_ButtonMaster.Player2 == "Police")
            {
                _followObject = GameObject.FindWithTag("DonutTruck");
            }
        }
        //else
        //{
        //    if (SCR_ButtonMaster.Player1 == "Truck")
        //    {
        //        _followObject = GameObject.FindWithTag("PoliceCar");
        //    }
        //    else if (SCR_ButtonMaster.Player2 == "Truck")
        //    {
        //        _followObject = GameObject.FindWithTag("PoliceCar");
        //    }
        //}
    }
	
	// Update is called once per frame
	void Update () {
        //transform.RotateAround(GetComponentInParent<Rigidbody>().transform.position,new Vector3(0,1,0),Time.deltaTime*100);
        //transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 100);

	    if (_followObject != null)
	    {
            var lookPos = _followObject.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            rotation *= Quaternion.Euler(0, 90, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotateSpeed * Time.deltaTime);
        }
        else
        {
            var rot = gameObject.transform.rotation;
            rot.z = 180;
            gameObject.transform.rotation = rot;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_LookAt : MonoBehaviour {
    public GameObject Target;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		if(this.gameObject!=null)
        {
            if (Target != null)
            {
                this.gameObject.transform.LookAt(Target.transform.position);
                this.gameObject.transform.Rotate(new Vector3(0, 180, 0));
            }
        }
	}
}

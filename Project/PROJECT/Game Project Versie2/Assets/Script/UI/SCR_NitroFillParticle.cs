using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_NitroFillParticle : MonoBehaviour {

    public GameObject NitroBar;
    public GameObject HitObject;

    Camera camera;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 HitObjectPosition = camera.WorldToScreenPoint(HitObject.transform.position);
        Vector3 NitroBarPosition = camera.WorldToScreenPoint(NitroBar.transform.position);

        Debug.Log(HitObjectPosition + " " + NitroBarPosition);
    }
}

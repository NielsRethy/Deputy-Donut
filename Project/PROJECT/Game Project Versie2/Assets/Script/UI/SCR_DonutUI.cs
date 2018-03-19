using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_DonutUI : MonoBehaviour {

    SCR_DonutSpawner _donutScript;
    GameObject _donutCounterUI;

	// Use this for initialization
	void Start ()
    {
       // _donutScript = GameObject.FindGameObjectWithTag("DonutMaster").GetComponent<SCR_DonutSpawner>();
        //_donutCounterUI = GameObject.Find("DonutText");
	}

    // Update is called once per frame
    void Update()
    {
        //if (_donutCounterUI != null && _donutScript != null)
       // {
       //     _donutCounterUI.GetComponent<Text>().text = _donutScript.GetDonuts().ToString();
       // }
    }
}
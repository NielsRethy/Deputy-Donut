using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_AddFlaggPoints : MonoBehaviour {

	// Use this for initialization
    private Text _pointsCaptureTheFlagg;

    void Start () {
        _pointsCaptureTheFlagg = GameObject.Find("DonutText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {

        if (_pointsCaptureTheFlagg != null)
        {
            _pointsCaptureTheFlagg.text = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().Points.ToString();
           
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_NitroUI : MonoBehaviour {

    GameObject PoliceCar;
    GameObject DonutTruck;

    public string PoliceCarTag = "PoliceCar";
    public string DonutTruckTag = "DonutTruck";

    public GameObject PoliceCarNitroUI;
    public GameObject DonutTruckNitroUI;

	// Use this for initialization
	void Start ()
    {
        PoliceCar = GameObject.FindGameObjectWithTag(PoliceCarTag);
        DonutTruck = GameObject.FindGameObjectWithTag(DonutTruckTag);
    }
	
	// Update is called once per frame
	void Update ()
    {
        PoliceCarNitroUI.GetComponent<Image>().fillAmount = PoliceCar.GetComponent<SCR_Nitro>().GetNitroAmount() / PoliceCar.GetComponent<SCR_Nitro>().GetMaxNitro();
        DonutTruckNitroUI.GetComponent<Image>().fillAmount = DonutTruck.GetComponent<SCR_Nitro>().GetNitroAmount() / DonutTruck.GetComponent<SCR_Nitro>().GetMaxNitro();
    }
}

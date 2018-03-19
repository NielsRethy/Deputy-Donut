using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_TruckHP : MonoBehaviour {

    GameObject DonutTruck;
    public string DonutTruckTag = "DonutTruck";
    public GameObject DonutTruckHealthUI;
    private bool frame = true;
    float _truckHP = 0;

    // Use this for initialization
    void Start () {
        DonutTruck = GameObject.FindGameObjectWithTag(DonutTruckTag);
      
    }

    // Update is called once per frame
    void Update()
    {
        if (frame)
        {
            DonutTruckHealthUI.GetComponent<Slider>().maxValue = DonutTruck.GetComponent<SCR_TruckDestructionManager>().GetMaxHealth();
            frame = false;
        }

        _truckHP = DonutTruck.GetComponent<SCR_TruckDestructionManager>().GetDamage();
        DonutTruckHealthUI.GetComponent<Slider>().value = _truckHP;
    }
}

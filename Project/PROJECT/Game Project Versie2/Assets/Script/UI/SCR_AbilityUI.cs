using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SCR_AbilityUI : MonoBehaviour {

    public GameObject _policeDisplay;
    public GameObject _truckDisplay;
    public GameObject _policeDisplay2;
    public GameObject _truckDisplay2;
    public bool _doOnce = false;
    public Sprite UIEmpty;

    void Start ()
    {
       // policeDisplay.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
       // truckDisplay.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

    }
    void Update ()
    {

    }

    public void UpdatePickupUI()
    {
        if (!SCR_AbilityManager.PoliceAbilityActive)
        {
            _policeDisplay.GetComponent<Image>().sprite = UIEmpty;
        }
        else
        {
            _policeDisplay.GetComponent<Image>().sprite = SCR_AbilityManager.ActiveAbilityPolice.UiSprite;

        }
        if (!SCR_AbilityManager.TruckAbilityActive)
        {
            _truckDisplay.GetComponent<Image>().sprite = UIEmpty;
        }
        else
        {
            _truckDisplay.GetComponent<Image>().sprite = SCR_AbilityManager.ActiveAbilityTruck.UiSprite;
        }
        if (!SCR_AbilityManager.PoliceAbilityActive2)
        {
            _policeDisplay2.GetComponent<Image>().sprite = UIEmpty;
        }
        else
        {
            _policeDisplay2.GetComponent<Image>().sprite = SCR_AbilityManager.ActiveAbilityPolice2.UiSprite;

        }
        if (!SCR_AbilityManager.TruckAbilityActive2)
        {
            _truckDisplay2.GetComponent<Image>().sprite = UIEmpty;
        }
        else
        {
            _truckDisplay2.GetComponent<Image>().sprite = SCR_AbilityManager.ActiveAbilityTruck2.UiSprite;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FlaggPickUp : MonoBehaviour
{
    private SCR_NotificationQueue _policeNotification;

    // Use this for initialization
    void Update()
    {

        //this.transform.position = GameObject.FindWithTag("FlaggLocation").transform.position;
       // this.transform.rotation = GameObject.FindWithTag("FlaggLocation").transform.rotation;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "DonutTruck" && GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().FlaggActive == false)
        {
            GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().FlaggActive = true;
           // _policeNotification.DisplayText("The donut truck has captured the flag!");
            SCR_NotificationQueue.NotificationPolice.DisplayText("The donut truck has captured the flag!");
            SCR_NotificationQueue.NotificationTruck.DisplayText("I've got the flag!");
        }


    }
}

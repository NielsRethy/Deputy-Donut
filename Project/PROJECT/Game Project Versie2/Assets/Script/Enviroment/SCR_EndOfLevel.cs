using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EndOfLevel : MonoBehaviour
{
    public GameObject DonutCar;
    bool _endReached;

    void OnTriggerEnter(Collider other)
    {
        if(other.name == DonutCar.name && !_endReached )
        {
            _endReached = true;
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                SCR_GameManager.Manager.EndRound(true);
            }
            else
            {
                SCR_GameManager.Manager.EndRound(false);
            }
            
        }
    }

    public bool isTriggered()
    {
        return false;
    }
}

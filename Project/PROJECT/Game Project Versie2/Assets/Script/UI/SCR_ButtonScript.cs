using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_ButtonScript : MonoBehaviour {

    // Use this for initialization
    public void NewRound()
    {
        if (SCR_ButtonMaster.Player1 == "Police")
        {
            SCR_ButtonMaster.Player1 = "Truck";
            SCR_ButtonMaster.Player2 = "Police";
        }
        else
        {
            SCR_ButtonMaster.Player2 = "Truck";
            SCR_ButtonMaster.Player1 = "Police";
        }

        SCR_GameManager._firstRound = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
    }
    public void BackToMenu()
    {

        if (SCR_ButtonMaster.Player1 == "Police")
        {
            SCR_ButtonMaster.Player1 = "Truck";
            SCR_ButtonMaster.Player2 = "Police";
        }
        else
        {
            SCR_ButtonMaster.Player2 = "Truck";
            SCR_ButtonMaster.Player1 = "Police";
        }
        SceneManager.LoadScene(0);
    }
}

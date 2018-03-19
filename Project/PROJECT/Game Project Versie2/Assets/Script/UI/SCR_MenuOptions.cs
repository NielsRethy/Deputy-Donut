using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MenuOptions : MonoBehaviour {

	// Use this for initialization
    public List<GameObject> ControlScreens;
    public int activeGameobject;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeControlsRight()
    {
        ControlScreens[activeGameobject].SetActive(false);
        activeGameobject++;
        if (activeGameobject == ControlScreens.Count)
        {
            activeGameobject = 0;
        }
        ControlScreens[activeGameobject].SetActive(true);

    }
    public void ChangeControlsLeft()
    {
        ControlScreens[activeGameobject].SetActive(false);
        activeGameobject--;
        if (activeGameobject < 0)
        {
            activeGameobject = ControlScreens.Count - 1;
        }
        ControlScreens[activeGameobject].SetActive(true);

    }

    public void UseQwerty()
    {
        SCR_ButtonMaster.Master.IsKeyboardAzerty = false;
    }
    public void UseAzerty()
    {
        SCR_ButtonMaster.Master.IsKeyboardAzerty = true;
    }
}

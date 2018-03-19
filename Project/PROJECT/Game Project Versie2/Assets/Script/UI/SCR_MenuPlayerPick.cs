using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_MenuPlayerPick : MonoBehaviour {

    // Use this for initialization

    bool isButtonP1Pressed = false;
    bool isButtonP2Pressed = false;

    public GameObject[] ReadyIndicator;
    public GameObject LightCop;
    public GameObject LightTruck;
    public GameObject RotateCar;
    public GameObject RotateTruck;
    public float RotateSpeed = 10.0f;
    void Start () {
        foreach (GameObject curr in ReadyIndicator)
        {
            curr.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (isButtonP2Pressed && isButtonP1Pressed)
        {
            StartCoroutine(StartnewScene("SCN_buildscene"));
        }

        if (Input.GetAxis("Player1Nitro") != 0)
        {
            CarChosen(0);
        }

        if (Input.GetAxis("Player2Nitro") != 0)
        {
            CarChosen(1);
        }

        if (RotateCar != null)
        {
            RotateCar.transform.Rotate(0.0f, Time.deltaTime * RotateSpeed, 0.0f);
        }
        if (RotateTruck != null)
        {
            RotateTruck.transform.Rotate(0.0f, Time.deltaTime * RotateSpeed, 0.0f);
        }
    }
    IEnumerator StartnewScene(string sceneName)
    {

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(sceneName);

    }

    public void CarChosen(int playerNumber)
    {
        switch (playerNumber)
        {
            case 0:
                isButtonP1Pressed = true;
                ReadyIndicator[playerNumber].SetActive(true);
                LightCop.SetActive(true);
                break;
            case 1:
                isButtonP2Pressed = true;
                ReadyIndicator[playerNumber].SetActive(true);
                LightTruck.SetActive(true);
                break;
        }
    }
}

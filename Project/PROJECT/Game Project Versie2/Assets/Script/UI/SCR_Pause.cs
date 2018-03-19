using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Pause : MonoBehaviour
{
    public GameObject PauseElement;
    bool _pauseGame = false;
    public string _pauseInput;
    private GameObject myEventSystem;
    public GameObject SetActiveButton;

    void Start()
    {
        PauseElement.SetActive(false);
        myEventSystem = GameObject.Find("EventSystem");
    }
    void Update()
    {
        PauseScreen();
    }

    void PauseScreen()
    {
        //check for input on string to pause(inputmanager)
        //if (Input.GetAxisRaw(_pauseInput) > 0)

        if ((Input.GetButtonDown(SCR_ButtonMaster.Master.Player1Pauze) || Input.GetButtonDown(SCR_ButtonMaster.Master.Player2Pauze)) && _pauseGame)
        {
            Time.timeScale = 1;
            PauseElement.SetActive(false);
            _pauseGame = false;
        }
        else if (Input.GetButtonDown(SCR_ButtonMaster.Master.Player1Pauze) || Input.GetButtonDown(SCR_ButtonMaster.Master.Player2Pauze) && !_pauseGame)
        {
            _pauseGame = true;
           
            PauseElement.SetActive(true);
            Time.timeScale = 0;
            do
            {
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(PauseElement.GetComponentsInChildren<Button>()[PauseElement.GetComponentsInChildren<Button>().Length - 1].gameObject);

            } while (myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject !=
                    PauseElement.GetComponentsInChildren<Button>()[
                        PauseElement.GetComponentsInChildren<Button>().Length - 1].gameObject);
        }

       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCR_Menu : MonoBehaviour
{
  
    public GameObject OptionMenu = null;
    public GameObject StartMenu = null;

    private GameObject myEventSystem;

    // Use this for initialization
    void Start()
    {
      
        myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(StartMenu.GetComponentInChildren<Button>().gameObject);
    }

    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {
      
    }

    public void ExitGame()
    {
        Application.Quit();
    }
 

    public void OpenOptionMenu()
    {
        if (OptionMenu!= null)
        {
            OptionMenu.SetActive(true);
            StartMenu.SetActive(false);

            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(OptionMenu.GetComponentsInChildren<Button>()[OptionMenu.GetComponentsInChildren<Button>().Length - 1].gameObject);
        }
    }
    public void BackToMenu()
    {
        if (OptionMenu != null)
        {
            OptionMenu.SetActive(false);
            StartMenu.SetActive(true);
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(StartMenu.GetComponentInChildren<Button>().gameObject);
        }
    }
}

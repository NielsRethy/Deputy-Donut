using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Timer : MonoBehaviour
{
    GameObject _carObject;
    public GameObject SoundManager;

    public float _levelClearWait;
    float currentTimer = 0f;
    bool _endReached = false;
    bool beginTimer = false;

    float endTime = 0f;
    float tempTimer = 0;
    private bool _player1Police;

    public GameObject playerTime;
    Text playerTimeText;

    public GameObject countDownPlayer1;
    public GameObject countDownPlayer2;
    public int FontSizeOverRide = 100;
    public int minimumFontSize = 20;
    public int timeScale = 200;
    public AudioClip countDownClip;

    public float CurrentTimer
    {
        get { return currentTimer; }
        set { currentTimer = value; }
    }

    void Start()
    {
        playerTimeText = playerTime.GetComponent<Text>();
        _carObject = GameObject.FindGameObjectWithTag("DonutTruck");

        countDownPlayer1.transform.position = new Vector3(Screen.width / 4, Screen.height / 2, 0);
        countDownPlayer2.transform.position = new Vector3(Screen.width - (Screen.width / 4), Screen.height / 2, 0);
        GetComponent<AudioSource>().clip = countDownClip;
        GetComponent<AudioSource>().Play();
        SoundManager.GetComponent<SCR_AudioManager>().PlayBGM(countDownClip);
    }

    public bool getTimer()
    {
        return beginTimer;
    }

    void Update()
    {
        GetComponent<AudioSource>().volume = SoundManager.GetComponent<SCR_AudioManager>().GetSFXVolume();
        //Set currentTime to countdown maximum and start the countdown
        if (!beginTimer)
        {
            currentTimer -= Time.deltaTime;
            if(currentTimer <= 1)
            {
                beginTimer = true;
                currentTimer = 0;
            }
        }

        if (!_endReached && beginTimer)
        {
            currentTimer += Time.deltaTime;
        }

        if(_endReached)
        {
            tempTimer += Time.deltaTime;
            if (tempTimer >= _levelClearWait)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if(beginTimer)
        {
            int minutes = Mathf.FloorToInt(currentTimer / 60F);
            int seconds = Mathf.FloorToInt(currentTimer - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            playerTimeText.text = niceTime;
            if(seconds > 0)
            {
                countDownPlayer1.GetComponent<Text>().text = "";
                countDownPlayer2.GetComponent<Text>().text = "";
            }
        }
        else
        {
            playerTimeText.text = "0:00";
        }
    }
    void OnGUI()
    {
        int minutes = Mathf.FloorToInt(currentTimer / 60F);
        int seconds = Mathf.FloorToInt(currentTimer - minutes * 60);

        if (currentTimer == 0)
        {
            FontSizeOverRide *= 2;
        }

        if (countDownPlayer1.GetComponent<Text>().text != "")
        {
            int size = (int)Mathf.PingPong(Time.time * timeScale, FontSizeOverRide);
            if (size < minimumFontSize) size = minimumFontSize;
            countDownPlayer1.GetComponent<Text>().fontSize = countDownPlayer2.GetComponent<Text>().fontSize = size;
        }

        if (!beginTimer)
        {
            countDownPlayer1.GetComponent<Text>().text = countDownPlayer2.GetComponent<Text>().text = seconds.ToString();
        }
        else
        {
            if (currentTimer <= 1f)
            {
                // Nitro is disabled while counting down
                SCR_Nitro policeNitroScript = GameObject.FindGameObjectWithTag("PoliceCar").GetComponent<SCR_Nitro>();
                SCR_Nitro truckNitroScript = GameObject.FindGameObjectWithTag("DonutTruck").GetComponent<SCR_Nitro>();
                // Nitro is enabled here
                policeNitroScript.SetCountDownIsOver(true);
                truckNitroScript.SetCountDownIsOver(true);

                if (SCR_ButtonMaster.Player1 == "Police")
                {
                    countDownPlayer1.GetComponent<Text>().text = "Fight";
                    countDownPlayer2.GetComponent<Text>().text = "Survive";

                }
                else
                {
                    countDownPlayer2.GetComponent<Text>().text = "Survive";
                    countDownPlayer1.GetComponent<Text>().text = "Fight";
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCR_GameManager : MonoBehaviour {

	// Use this for initialization
    public static SCR_GameManager Manager;
    public static bool _firstRound = true;
    private float _scoreFirstRoundPlayer1;
    private float _scoreFirstRoundPlayer2;
    private float _scoreSecondRoundPlayer1;
    private float _scoreSecondRoundPlayer2;
    private GameObject _winnerscreen;
    private GameObject _endscreen;
    private GameObject _endscreenPlayer1;
    private GameObject _endscreenPlayer2;
    private bool _showWinScreen = false;
    private bool _doOnce = false;
    private GameObject myEventSystem;

    public List<string> JokeTextPlayer1Win;
    public List<string> JokeTextPlayer2Win;

    public AudioSource BackgroundMusic;

    //delay
    public float RoundDelay = 2.0f;
    private float _CurrentDelay = 0;
    private bool _EnableDelay = false;
    private bool _Player1Wins = false;
    public bool FirstRound
    {
        get { return _firstRound; }
        set { _firstRound = value; }
    }


    void Start()
    {
        myEventSystem = GameObject.Find("EventSystem");
        if (GameObject.FindGameObjectWithTag("WinnerScreen") != null)
        {
            _winnerscreen = GameObject.FindGameObjectWithTag("WinnerScreen");
            _winnerscreen.SetActive(false);
            _endscreen = GameObject.FindGameObjectWithTag("EndScreen");
            _endscreen.SetActive(false);
            BackgroundMusic.loop = true;
            BackgroundMusic.volume = 0.01f;
            BackgroundMusic.Play();
        }
    }

    public void EndRound(bool PlayerOneWins)
    {
        _Player1Wins = PlayerOneWins;
        _EnableDelay = true;
        GameObject.FindGameObjectWithTag("PoliceCar").GetComponent<SRC_CarControllerInput>().SetDisabled(true);
        GameObject.FindGameObjectWithTag("DonutTruck").GetComponent<SRC_CarControllerInput>().SetDisabled(true);
       // _firstRound = !_firstRound;
        RoundDelay = 0.5f;


    }
    void Awake()
    {
        if (Manager == null)
        {
            DontDestroyOnLoad(gameObject);
            Manager = this;
        }
        else if (Manager != this)
        {
            
            // Manager.Start();
            Destroy(gameObject);
        }

       
    }

    void Update()
    {



        if (_winnerscreen == null && GameObject.FindGameObjectWithTag("WinnerScreen") != null)
        {
            _winnerscreen = GameObject.FindGameObjectWithTag("WinnerScreen");
            _winnerscreen.SetActive(false);

            _endscreen = GameObject.FindGameObjectWithTag("EndScreen");
            _endscreen.SetActive(false);

            BackgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();
            BackgroundMusic.loop = true;
            BackgroundMusic.volume = 0.01f;
            BackgroundMusic.Play();

        }
        
        if (_EnableDelay)
        {
            Time.timeScale = 0.2f;
            _CurrentDelay += Time.deltaTime;
            if (_CurrentDelay > RoundDelay)
            {
                Time.timeScale = 1.0f;

                _CurrentDelay = 0;
                _EnableDelay = false;
                newRound(_Player1Wins);
            }
        }
        //if (!Manager.FirstRound && !_doOnce)
            //{ 
            //    SwitchSides();
            //    _doOnce = true;
            //}
        
    }

    //public void SwitchSides()
    //{
    //    var l = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().rect;
    //    var r = GameObject.FindWithTag("SecondCam").GetComponent<Camera>().rect;

    //    GameObject.FindWithTag("MainCamera").GetComponent<Camera>().rect = r;
    //    GameObject.FindWithTag("SecondCam").GetComponent<Camera>().rect = l;

    //    l = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().rect;
    //    r = GameObject.FindWithTag("SecondCam").GetComponent<Camera>().rect;
    //}
    void newRound(bool player1Wins)
    {
        var time = GameObject.FindGameObjectWithTag("HUD").GetComponent<SCR_Timer>().CurrentTimer;
        if (_firstRound)
        {
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                _scoreFirstRoundPlayer1 = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().Points + GameObject.FindGameObjectWithTag("HUD").GetComponent<SCR_Timer>().CurrentTimer + GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().BonusPoints;
                _scoreFirstRoundPlayer2 = 0.0f;
            }
            else if (SCR_ButtonMaster.Player2 == "Truck")
            {
                _scoreFirstRoundPlayer1 = 0.0f;
                _scoreFirstRoundPlayer2 =
                    GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().Points +
                    GameObject.FindGameObjectWithTag("HUD").GetComponent<SCR_Timer>().CurrentTimer +
                    GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().BonusPoints;
            }
        }
        else
        {
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                _scoreSecondRoundPlayer1 = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().Points +
                    GameObject.FindGameObjectWithTag("HUD").GetComponent<SCR_Timer>().CurrentTimer +
                    GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().BonusPoints;
                _scoreSecondRoundPlayer2 = 0.0f;
            }
            else if (SCR_ButtonMaster.Player2 == "Truck")
            {
                _scoreSecondRoundPlayer1 = 0.0f;
                _scoreSecondRoundPlayer2 = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().Points +
                    GameObject.FindGameObjectWithTag("HUD").GetComponent<SCR_Timer>().CurrentTimer +
                    GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().BonusPoints;
            }
        }
        if (_firstRound)
        {
            if (_winnerscreen == null)
            {
                _winnerscreen = GameObject.FindGameObjectWithTag("WinnerScreen");
                _winnerscreen.SetActive(false);

                _endscreen = GameObject.FindGameObjectWithTag("EndScreen");
                _endscreen.SetActive(false);
            }
            _winnerscreen.SetActive(true);


           
            if (player1Wins)
            {
                _winnerscreen.GetComponentsInChildren<Text>()[2].text = _scoreFirstRoundPlayer1.ToString("f0");
                _winnerscreen.GetComponentsInChildren<Text>()[3].text = _scoreFirstRoundPlayer2.ToString("f0");
                _winnerscreen.GetComponentsInChildren<Text>()[4].text = GameObject.FindGameObjectWithTag("HUD").GetComponent<SCR_Timer>().CurrentTimer.ToString();
                _winnerscreen.GetComponentsInChildren<Text>()[5].text = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().Points.ToString();
                _winnerscreen.GetComponentsInChildren<Text>()[6].text = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().BonusPoints.ToString();
            }
            else
            {
                _winnerscreen.GetComponentsInChildren<Text>()[2].text = _scoreFirstRoundPlayer1.ToString();
                _winnerscreen.GetComponentsInChildren<Text>()[3].text = _scoreFirstRoundPlayer2.ToString();
                _winnerscreen.GetComponentsInChildren<Text>()[4].text = GameObject.FindGameObjectWithTag("HUD").GetComponent<SCR_Timer>().CurrentTimer.ToString();
                _winnerscreen.GetComponentsInChildren<Text>()[5].text = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().Points.ToString();
                _winnerscreen.GetComponentsInChildren<Text>()[6].text = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().BonusPoints.ToString();
            }
            _firstRound = false;
            //myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_winnerscreen.GetComponentsInChildren<Button>()[_winnerscreen.GetComponentsInChildren<Button>().Length - 1].gameObject);

            //var t = _winnerscreen.GetComponentsInChildren<Button>();

            //var te = 0;
        }
        else
        {
            _endscreen.SetActive(true);
            var endScorePlayer1 = _scoreFirstRoundPlayer1 + _scoreSecondRoundPlayer1;
            var endScorePlayer2 = _scoreFirstRoundPlayer2 + _scoreSecondRoundPlayer2;
            if (endScorePlayer1 > endScorePlayer2)
            {
                GameObject.Find("WinScreenPlayer2").SetActive(false);
                GameObject.Find("WinScreenPlayer1").SetActive(true);
                _endscreen.GetComponentsInChildren<Text>()[2].text = endScorePlayer1.ToString("f0");
                _endscreen.GetComponentsInChildren<Text>()[3].text = endScorePlayer2.ToString("f0");
                _endscreen.GetComponentsInChildren<Text>()[4].text = GameObject.FindGameObjectWithTag("HUD").GetComponent<SCR_Timer>().CurrentTimer.ToString();
                _endscreen.GetComponentsInChildren<Text>()[5].text = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().Points.ToString();
                _endscreen.GetComponentsInChildren<Text>()[6].text = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().BonusPoints.ToString();

                var randNumber = Random.Range(0, JokeTextPlayer1Win.Count);
                _endscreen.GetComponentsInChildren<Text>()[7].text = JokeTextPlayer1Win[randNumber];
                _endscreen.GetComponentsInChildren<Text>()[8].text = JokeTextPlayer1Win[randNumber];
                _endscreen.GetComponentsInChildren<Text>()[9].text = JokeTextPlayer1Win[randNumber];
            }
            else
            {
                GameObject.Find("WinScreenPlayer2").SetActive(true);
                GameObject.Find("WinScreenPlayer1").SetActive(false);
                _endscreen.GetComponentsInChildren<Text>()[2].text = endScorePlayer1.ToString("f0");
                _endscreen.GetComponentsInChildren<Text>()[3].text = endScorePlayer2.ToString("f0");
                _endscreen.GetComponentsInChildren<Text>()[4].text = GameObject.FindGameObjectWithTag("HUD").GetComponent<SCR_Timer>().CurrentTimer.ToString();
                _endscreen.GetComponentsInChildren<Text>()[5].text = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().Points.ToString();
                _endscreen.GetComponentsInChildren<Text>()[6].text = GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().BonusPoints.ToString();

                var randNumber = Random.Range(0, JokeTextPlayer2Win.Count);
                _endscreen.GetComponentsInChildren<Text>()[7].text = JokeTextPlayer2Win[randNumber];
                _endscreen.GetComponentsInChildren<Text>()[8].text = JokeTextPlayer2Win[randNumber];
                _endscreen.GetComponentsInChildren<Text>()[9].text = JokeTextPlayer2Win[randNumber];
            }
            _firstRound = true;
            //myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_endscreen.GetComponentsInChildren<Button>()[_endscreen.GetComponentsInChildren<Button>().Length - 1].gameObject);
  

        }
    }
}

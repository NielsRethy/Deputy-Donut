using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_CaptureTheFlagg : MonoBehaviour
{

    public List<Transform> SpawnPoints;
    public GameObject Flagg;
    private bool _flaggActive;
    private float _points;
    private float _bonusPoints;
    private float _bonusPointsView;
    private float _pointsTotal;
    private Vector3 _flaggScale;
    private GameObject _textActive;
    private Text _extraPoints;
    private bool _activateBonusPoints;
    private double _timeLeft;
    private ParticleSystem _idleParticles;
    private bool _addExtraPoints;
    private Text _extraPointsCaptureTheFlagg;
    private double _timeExtraPoints;
    private float _bonusPointsAdd;
    private Rigidbody _flagRb;
    private float _lostFlagTimeStamp;
    private float _flagPickupCooldown = 1f;
    public bool FlaggActive
    {
        get { return _flaggActive; }
        set
        {
           
            if (!value)
            {
               //if (_flaggActive)
               //{
               //    //var r = Random.Range(0, SpawnPoints.Count);
               //    //Flagg.transform.position = SpawnPoints[r].position;
               //    //Flagg.transform.rotation = new Quaternion(0,0,0,0);
               //    //Flagg.transform.parent = null;
               //    //Flagg.transform.localScale = _flaggScale;
               //    Flagg.GetComponent<Rigidbody>().isKinematic = false;
               //    GameObject.FindWithTag("DonutTruck").GetComponentInChildren<SCR_ArrowPointToObject>().FollowObject = GameObject.FindWithTag("Flagg");
               //    //GameObject.FindWithTag("DonutTruck").GetComponentInChildren<SCR_ArrowPointToObject>().FollowObject = SpawnPoints[r].gameObject;
               //    _idleParticles.Play();
               //}
            }
            else
            {
                if (_lostFlagTimeStamp + _flagPickupCooldown < Time.time)
                {
                    Flagg.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                    Flagg.transform.position = GameObject.FindWithTag("FlaggLocation").transform.position;
                    Flagg.transform.rotation = GameObject.FindWithTag("FlaggLocation").transform.rotation;
                    Flagg.transform.parent = GameObject.FindWithTag("DonutTruck").transform;
                    Flagg.GetComponent<Rigidbody>().isKinematic = true;
                    GameObject.FindWithTag("DonutTruck").GetComponentInChildren<SCR_ArrowPointToObject>().FollowObject = null;
                    _idleParticles.Stop();
                    _flaggActive = value;
                }
            }
            
        }
    }

    public float Points
    {
        get { return _points; }
        set { _points = value; }
    }

    public bool ActivateBonusPoints
    {
        get { return _activateBonusPoints; }
        set
        {
            if (_flaggActive)
            {
                _activateBonusPoints = value;
                _textActive.SetActive(true);
                _timeLeft = 1;
            }

        }
    }

    public float BonusPoints
    {
        get { return _bonusPoints; }
        set { _bonusPoints = value; }
    }


    // Use this for initialization
	void Start ()
	{
        Flagg = GameObject.FindGameObjectWithTag("Flagg");
        _flagRb = Flagg.GetComponent<Rigidbody>();
        var r = Random.Range(0, SpawnPoints.Count);
	    Flagg.transform.position = SpawnPoints[r].position;
        GameObject.FindWithTag("DonutTruck").GetComponentInChildren<SCR_ArrowPointToObject>().FollowObject = SpawnPoints[r].gameObject;
	    _flaggScale = GameObject.FindWithTag("Flagg").transform.localScale;
	    _textActive = GameObject.FindWithTag("ExtraPoints");
	    _extraPoints = _textActive.GetComponentsInChildren<Text>()[1];
        _textActive.SetActive(false);
        _idleParticles = Flagg.transform.Find("PS_Idle").GetComponent<ParticleSystem>();
        Debug.Log("After");
        
        _extraPointsCaptureTheFlagg = GameObject.Find("ExtraDonutText").GetComponent<Text>();
        
    }
	
	// Update is called once per frame
	void Update () {


	    if (_flaggActive)
	    {
	        _points += 1;

	    }

        if (_activateBonusPoints)
        {
            _timeLeft -= Time.deltaTime;
           // _textActive.SetActive(true);
            _bonusPoints += 10;
            _bonusPointsView += 10;
            _extraPoints.text = _bonusPointsView.ToString();

            if (_timeLeft < 0)
            {
                _addExtraPoints = true;
                _timeExtraPoints = 0.7f;
                _bonusPointsAdd = _bonusPointsView;
                _activateBonusPoints = false;
                _textActive.SetActive(false);
                _bonusPointsView = 0;
            }

        }

        if (_addExtraPoints)
        {
            _timeExtraPoints -= Time.deltaTime;
            _extraPointsCaptureTheFlagg.text = "+" + _bonusPointsAdd;

            if (_timeExtraPoints < 0)
            {
                _points += _bonusPointsAdd;
                _extraPointsCaptureTheFlagg.text = "";
                _addExtraPoints = false;
                _bonusPointsAdd = 0;
            }
        }
    }

    public void DropFlag(Vector3 launchDir, float collisionStrength)
    {
        // launch flag in hit direction
        if (_flaggActive)
        {
            _flaggActive = false;
            _flagRb.isKinematic = false;
            _lostFlagTimeStamp = Time.time;

            GameObject truck = GameObject.FindGameObjectWithTag("DonutTruck");

            Flagg.transform.position = truck.transform.position + Vector3.up * 2f;
            Flagg.transform.rotation = new Quaternion(0, 0, 0, 0);
            Flagg.transform.parent = null;
            Flagg.transform.localScale = _flaggScale;

            if (launchDir.magnitude < 0.8f)
            {
                launchDir = launchDir.normalized * 0.8f;
            }

            launchDir = launchDir.normalized * (-1f) + new Vector3(0, 0.8f, 0);
            Debug.Log(launchDir.magnitude);

            _flagRb.AddForce(launchDir * 300f );
            truck.GetComponentInChildren<SCR_ArrowPointToObject>().FollowObject = GameObject.FindWithTag("Flagg");
            _idleParticles.Play();

            SCR_NotificationQueue.NotificationPolice.DisplayText("He dropped the flag.");
            SCR_NotificationQueue.NotificationTruck.DisplayText("I dropped the flag!");
      

        }

    }
 
}

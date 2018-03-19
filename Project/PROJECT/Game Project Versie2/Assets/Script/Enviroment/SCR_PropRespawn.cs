using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PropRespawn : MonoBehaviour {

    // Use this for initialization
    public float RespawnTime =30.0f;
    public float AliveTime = 20.0f;
    public float ShrinkTime = 2.0f;
    public float GrowTime = 2.0f;
    

    private float _TimeSinceHit = 0.0f;
    private bool _Hit = false;
    private float _ScaleFactor = 1.0f;
    private Vector3 _Scale;
    private Vector3 _Position;
    private Quaternion _Rotation;
    enum state
    {
        WAITING,
        SHRINKING,
        GROWING,
        UNHIT,
        HIT
    };
    private state _Status = state.UNHIT;
	void Start () {
        _Scale = transform.localScale;
        _Position = transform.localPosition;
         _Rotation= transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_Status == state.HIT)
        {
            _TimeSinceHit += Time.deltaTime;
            if (_TimeSinceHit > AliveTime - ShrinkTime)
            {
                _Status = state.SHRINKING;
            }
        }
        if (_Status == state.SHRINKING)
        {
            _TimeSinceHit += Time.deltaTime;
            _ScaleFactor -= (Time.deltaTime) / ShrinkTime;
            transform.localScale = _Scale * (_ScaleFactor);
            if (_ScaleFactor <= 0.01f)
            {
                _ScaleFactor = 0.0f;
                _Status = state.WAITING;

            }
        }
        if (_Status == state.WAITING)
        {
            _TimeSinceHit += Time.deltaTime;
            if (_TimeSinceHit > RespawnTime)
            {
                _Status = state.GROWING;
                GetComponent<SCR_GiveNitro>()._IsHit = false;
                transform.localPosition = _Position;
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

                transform.rotation = _Rotation;

            }
        }
        if (_Status == state.GROWING)
        {
            _ScaleFactor += (Time.deltaTime) / GrowTime;
            transform.localScale = _Scale * (_ScaleFactor);
            if (_ScaleFactor >= 1)
            {
                _ScaleFactor = 1;
                _Status = state.UNHIT;
            }
        }
    }





       
        
	
    public void IsHit()
    {
        _Status = state.HIT;
        _TimeSinceHit = 0.0f;


    }

}

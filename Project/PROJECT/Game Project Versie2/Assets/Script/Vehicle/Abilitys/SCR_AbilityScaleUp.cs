using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AbilityScaleUp : SCR_AbilityBase
{
    public float ScaleFactor;
    public float TimeStop;
    public float ShrinkOver = 0.5f;

    public Vector3 CentreMass;
    private Vector3 _normalScale;
    private Vector3 _normalCentreMass;
    private float _normalDistance;
    private float _normalHeight;
    private bool _Grow = false;
    private bool _Shrink = false;
    private bool _Changed = false;
    private float _TimePassed = 0;
    private GameObject _Cop;
    private GameObject _Second;
    private float _TimeChanging = 0;

    private float volume;
    // Use this for initialization
    void Start ()
    {
        _normalCentreMass = GameObject.FindWithTag("PoliceCar").GetComponent<SCR_Vehicle>().MCentreOfMassOffset;

        _Cop = GameObject.FindWithTag("PoliceCar");
        _normalScale = _Cop.transform.localScale;

        _Second = GameObject.FindWithTag("SecondCam");
        _normalDistance = _Second.GetComponent<SCR_CameraFollow>().Distance;
        _normalHeight = _Second.GetComponent<SCR_CameraFollow>().Height;
        _abilityIsActive = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if (_abilityIsActive)
	    {
            volume = _Cop.GetComponent<AudioSource>().volume;
            _Cop.GetComponent<AudioSource>().volume = volume * 2;

            if (SCR_ButtonMaster.Player1 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                { 
                    _Grow = true;

                    EnableActivateParticles(true);

                    _abilityIsActive = false;

                    //_SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[7], _Cop);
                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[12], _Cop);
                }
            }

            else if (SCR_ButtonMaster.Player2 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {
                    _Grow = true;


                    EnableActivateParticles(true);

                    _abilityIsActive = false;

                    // _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[7], _Cop);
                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[12], _Cop);
                }
            }
          
	    }
        if (_Changed)
        {
            _TimePassed += Time.deltaTime;
            if (_TimePassed > TimeStop)
            {
                _TimePassed = 0;
                _Changed = false;
                _Shrink = true;
            }
        }
        if (_Grow)
        {
            var gb = _Cop;

            var n = gb.transform.localScale;

            float factor = ScaleFactor;
            if (_TimeChanging <= ShrinkOver)
            {
                _TimeChanging += Time.deltaTime;
                factor = Mathf.Lerp(1.0f, ScaleFactor, _TimeChanging / ShrinkOver);

            }
            if (_TimeChanging > ShrinkOver)
            {
                _TimeChanging = 0;
                _Grow = false;
                _Changed = true;

            }


             n =_normalScale;
            n.x =_normalScale.x * factor;
            n.y = _normalScale.x * factor;
            n.z = _normalScale.x * factor;
            gb.transform.localScale = n;
            gb.GetComponent<SCR_Vehicle>().MCentreOfMassOffset = CentreMass;

            _Second.GetComponent<SCR_CameraFollow>().Distance =_normalDistance * factor;
            _Second.GetComponent<SCR_CameraFollow>().Height =_normalHeight* factor;



        }
        if (_Shrink)
        {
            var gb = _Cop;

            var n = gb.transform.localScale;

            float factor = ScaleFactor;
            if (_TimeChanging <= ShrinkOver)
            {
                _TimeChanging += Time.deltaTime;
                factor = Mathf.Lerp(ScaleFactor, 1.0f, _TimeChanging / ShrinkOver);

            }
            if (_TimeChanging > ShrinkOver)
            {
                _Cop.GetComponent<AudioSource>().volume = volume;

                _TimeChanging = 0;
                _Shrink = false;

            }



            n = _normalScale;
            n.x = _normalScale.x * factor;
            n.y = _normalScale.x * factor;
            n.z = _normalScale.x * factor;
            gb.transform.localScale = n;

            _Cop.GetComponent<SCR_Vehicle>().MCentreOfMassOffset = _normalCentreMass;
            _Second.GetComponent<SCR_CameraFollow>().Distance = _normalDistance * factor    ;
            _Second.GetComponent<SCR_CameraFollow>().Height = _normalHeight* factor;
            if (_abilityOne)
            {
                EnableIdleParticles(true);

                SCR_AbilityManager.PoliceAbilityActive = false;
                SetVisualModelActive(false);
            }
            else
            {
                EnableIdleParticles(true);

                SCR_AbilityManager.PoliceAbilityActive2 = false;
                SetVisualModelActive(false);

            }

            }
    }
 
}

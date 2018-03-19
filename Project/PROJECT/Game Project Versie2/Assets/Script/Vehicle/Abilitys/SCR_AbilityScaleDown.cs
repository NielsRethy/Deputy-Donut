using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AbilityScaleDown : SCR_AbilityBase
{

    public float ScaleFactor;
    public float TimeStop;
    public float ShrinkOver = 0.5f;
    public Transform CentreMass ;
    private Vector3 _normalScale;
    private Vector3 _normalCentreMass;

    private float ShrinkFactor = 0;
    private Transform _truck;
    private bool _Shrink = false;
    private bool _Grow = false;
    private float _TimePassed =0;
    private float _TimeChanging = 0;
    private bool _Changed = false;

    private float volume;
    // Use this for initialization
    void Start()
    {
        _normalCentreMass = GameObject.FindWithTag("DonutTruck").GetComponent<SCR_Vehicle>().MCentreOfMassOffset;
        _truck = GameObject.FindWithTag("DonutTruck").GetComponent<Transform>();
        _normalScale = _truck.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_abilityIsActive)
        {
            volume = GameObject.FindWithTag("DonutTruck").GetComponent<AudioSource>().volume;
            GameObject.FindWithTag("DonutTruck").GetComponent<AudioSource>().volume = volume * 2;
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                {
                    _Shrink = true;
                    _abilityIsActive = false;
                    EnableActivateParticles(true);

                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[6], GameObject.FindWithTag("DonutTruck"));
                }
            }

            else if (SCR_ButtonMaster.Player2 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {

                    _Shrink = true;
                    EnableActivateParticles(true);
                    _abilityIsActive = false;

                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[6], GameObject.FindWithTag("DonutTruck"));

                }
            }

        }
        if(_Changed)
        {
            _TimePassed += Time.deltaTime;
            if (_TimePassed > TimeStop) 
            {
                _TimePassed = 0;
                _Changed = false;
                _Grow = true;
            }
        }
        if(_Shrink)
        {
            var n = _normalScale;
            float factor = ScaleFactor;
            if(_TimeChanging <= ShrinkOver)
            {
                _TimeChanging += Time.deltaTime;
                factor = Mathf.Lerp(1.0f, ScaleFactor, _TimeChanging / ShrinkOver);

            }
            if(_TimeChanging> ShrinkOver)
            {
                _TimeChanging = 0;
                _Shrink = false;
                _Changed = true;

            }

            n.x = 1.0f/factor;
            n.y = 1.0f/factor;
            n.z = 1.0f/factor;
            _truck.transform.localScale = n;
            _truck.GetComponent<SCR_Vehicle>().MCentreOfMassOffset = CentreMass.position;
        }
        if (_Grow)
        {
            GameObject.FindWithTag("DonutTruck").GetComponent<AudioSource>().volume = volume;
            var n = _normalScale;
            float factor = ScaleFactor;

            if (_TimeChanging <= ShrinkOver)
            {
                _TimeChanging += Time.deltaTime;
                factor = Mathf.Lerp(ScaleFactor, 1.0f, _TimeChanging / ShrinkOver);

            }
            if (_TimeChanging > ShrinkOver)
            {
                _TimeChanging = 0;
                _Grow = false;
                GameObject.FindWithTag("DonutTruck").GetComponent<AudioSource>().volume = volume;
                if (_abilityOne)
                {
                    EnableIdleParticles(true);
                    SCR_AbilityManager.TruckAbilityActive = false;
                    SetVisualModelActive(false);
                }
                else
                {
                    EnableIdleParticles(true);
                    SCR_AbilityManager.TruckAbilityActive2 = false;
                    SetVisualModelActive(false);

                }

            }
            n.x = 1.0f / factor;
                n.y = 1.0f / factor;
                n.z = 1.0f / factor;
                _truck.transform.localScale = n;
                _truck.GetComponent<SCR_Vehicle>().MCentreOfMassOffset = _normalCentreMass;
            
        }

    }
  
   
}

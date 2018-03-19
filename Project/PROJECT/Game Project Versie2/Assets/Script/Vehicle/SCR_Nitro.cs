using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Nitro : SCR_AbilityBase {

    public int NitroForce = 50;
    public int MaxAmountOfNitro = 50;
    public float NitroPerSec = 10;
    public List<ParticleSystem> NitroParticles;
    public int PropNitro = 8;
    public float ParticlesOverDistAmount = 15.0f;
    private Rigidbody _RigidBody;
    public float _CurrentNitro;      // Amount of nitro in car ~ better name
    private bool _IsPolice = false;
    private bool _IsDonut = false;
    private string ActivateNitroAxis;
    private bool _bEmitting = false;
    private bool _soundPlaying = false;
    private bool _bcountDownIsOver = false;

    public float NitroFadeOutTimer = 0.2f;
    float timeOutDelay = 0;
    public float maxTimeOut = 0.2f;

	// Use this for initialization
	new void Awake () {
        _RigidBody = GetComponent<Rigidbody>();
        //NitroParticles = GetComponentInChildren<ParticleSystem>();

        EnableParticles(false);

        _CurrentNitro = 10;

        if (transform.CompareTag("PoliceCar"))
        {
            if (SCR_ButtonMaster.Player1 == "Police")
            {
                _IsPolice = true;
                _IsDonut = false;
                ActivateNitroAxis = SCR_ButtonMaster.Master.Player1Nitro;     //Press E       
            }
            else if ((SCR_ButtonMaster.Player2 == "Police"))
            {
                _IsPolice = true;
                _IsDonut = false;
                ActivateNitroAxis = SCR_ButtonMaster.Master.Player2Nitro;
            }

            timeOutDelay = maxTimeOut;       
        }
        else if (transform.CompareTag("DonutTruck"))
        {

            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                _IsPolice = false;
                _IsDonut = true;
                ActivateNitroAxis = SCR_ButtonMaster.Master.Player1Nitro;
            }
            else if ((SCR_ButtonMaster.Player2 == "Truck"))
            {
                _IsPolice = false;
                _IsDonut = true;
                ActivateNitroAxis = SCR_ButtonMaster.Master.Player2Nitro;
            }
        }

	}
	
	// Update is called once per frame
	void Update () {

        // Activate Nitro policar
	    if (ActivateNitroAxis != null)
	    {
            if (_bcountDownIsOver)
            {
                if (Input.GetAxisRaw(ActivateNitroAxis) > 0)
                {
                    ActivateNitro();
                }
                ManageEffects();
            }
        }
	}

    void ActivateNitro()
    {
        if (_CurrentNitro > 0f)
        {
            // Calculate nitro left
            _CurrentNitro -= NitroPerSec * Time.deltaTime;
            _CurrentNitro = Mathf.Clamp(_CurrentNitro, 0, MaxAmountOfNitro);    // Clam nitro in vehicle between [0, MaxCapacity]

            // Nitro push car
            Vector3 localForward = transform.forward;
            _RigidBody.AddForce(NitroForce * localForward,ForceMode.Acceleration);
        }
    }

    void ManageEffects()
    {
        if (_CurrentNitro > 0)
        {
            if (Input.GetButtonUp(ActivateNitroAxis) && _bEmitting == true)
            {
                EnableParticles(false);
                if (_SoundHolder!= null)
                {
                    _SoundHolder.GetComponent<SCR_AudioManager>().StopSound(NitroFadeOutTimer, gameObject);
                }
                
            }

            if(!Input.GetButtonDown(ActivateNitroAxis))
            {
                timeOutDelay += Time.deltaTime;
            }

            if (Input.GetButtonDown(ActivateNitroAxis) && _bEmitting == false)
            {
                EnableParticles(true);

                if (timeOutDelay >= maxTimeOut)
                {
                    if (_SoundHolder != null)
                    {
                        _SoundHolder.GetComponent<SCR_AudioManager>()
                            .PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[0], gameObject);
                    }
                    timeOutDelay = 0;
                }
            }
        }
        else if (_bEmitting == true)
        {
            EnableParticles(false);
            if (_SoundHolder != null)
            {
                _SoundHolder.GetComponent<SCR_AudioManager>().StopSound(NitroFadeOutTimer, gameObject);
            }
        }
    }

    public void AddNitro(float nitroReward)
    {
        _CurrentNitro += nitroReward;
        _CurrentNitro = Mathf.Clamp(_CurrentNitro, 0, MaxAmountOfNitro);    // Clam nitro in vehicle between [0, MaxCapacity]
    }
    public void AddPropNitro()
    {
        _CurrentNitro += PropNitro;
        _CurrentNitro = Mathf.Clamp(_CurrentNitro, 0, MaxAmountOfNitro);    // Clam nitro in vehicle between [0, MaxCapacity]

    }
    public float GetNitroAmount()
    {
        return _CurrentNitro;
    }

    public float GetMaxNitro()
    {
        return MaxAmountOfNitro;
    }

    private void EnableParticles(bool enableParticles)
    {
        Debug.Log("Called" + _IsDonut);

        _bEmitting = enableParticles;

        foreach (ParticleSystem ps in NitroParticles)
        {
            //var emission = ps.emission;

            if (enableParticles)
            {
                if (ps != null)
                {
                    ps.Play();
                }
                //emission.rateOverDistanceMultiplier = 15.0f;
                //emission.rateOverDistance = ParticlesOverDistAmount;
            }
            else
            {
                Debug.Log("hit");
                if (ps!= null)
                {
                    ps.Stop();
                }
                

                //emission.rateOverDistanceMultiplier = 0.0f;
                //emission.rateOverDistance = 0.0f;
                //ps.emission.rateOverDistanceMultiplier.Equals(0);
            }
        }
    }

    public void SetCountDownIsOver(bool isOver)
    {
        _bcountDownIsOver = isOver;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PickUp : MonoBehaviour
{
    private SCR_AbilityManager _abilityScript;
    public float respawnTime = 20;
    public float GrowTime = 1;

    [Header("Particles")]
    public List<ParticleSystem> IdleParticles;
    public List<ParticleSystem> OnPickUpParticles;


    private float _TimeSincePickedUp = 0;
    private bool _isActive = true;
    private PickupState _State = PickupState.SPAWNED;
    private Vector3 _InitScale;
    private float _GrowFactor = 1;
    private 
    enum PickupState
    {
        SPAWNED,
        GROWING,
        SHRINKING,
        PICKED

    };

    // Use this for initialization
    void Start()
    {
        _abilityScript = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<SCR_AbilityManager>();
        gameObject.transform.GetChild(0).gameObject.active = true;
        _InitScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        switch(_State)
        {
            case PickupState.GROWING:
                transform.localScale = _InitScale * _GrowFactor;
                _GrowFactor += Time.deltaTime / GrowTime;
                if (_GrowFactor >= 1)
                {
                    _GrowFactor = 1;

                    EnableParticles(IdleParticles);
                    _State = PickupState.SPAWNED;
                    transform.Rotate(0, 0, 20 * Time.deltaTime);
                    _isActive = true;

                }
                break;
            case PickupState.SHRINKING:
                transform.localScale = _InitScale * _GrowFactor;
                _GrowFactor -= Time.deltaTime / GrowTime;
                if(_GrowFactor<=0)
                {
                    _GrowFactor = 0;
                    _State = PickupState.PICKED;
                    transform.Rotate(0, 0, 20 * Time.deltaTime);

                }
                break;
            case PickupState.PICKED:
                _TimeSincePickedUp += Time.deltaTime;
                if(_TimeSincePickedUp>respawnTime)
                {
                    _TimeSincePickedUp = 0;
                    _State = PickupState.GROWING;
                    DisableParticles(IdleParticles);

                }
                break;
            case PickupState.SPAWNED:
                transform.Rotate(0, 0, 20 * Time.deltaTime);
                
                break;
                
        };
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isActive)
        {
            if (other.tag == "DonutTruck")
            {
                if (!SCR_AbilityManager.TruckAbilityActive)
                {
                    _abilityScript.ActivateAbility(false);
                    _State = PickupState.SHRINKING;
                    _isActive = false;
                }
                else if (!SCR_AbilityManager.TruckAbilityActive2)
                {
                    _abilityScript.ActivateAbility(false);
                    _isActive = false;
                    _State = PickupState.SHRINKING;
                }
                OnPickupParticles();
            }
            if (other.tag == "PoliceCar")
            {
                if (!SCR_AbilityManager.PoliceAbilityActive)
                {
                    _abilityScript.ActivateAbility(true);
                    _State = PickupState.SHRINKING;
                    _isActive = false;
                }
                else if (!SCR_AbilityManager.PoliceAbilityActive2)
                {
                    _abilityScript.ActivateAbility(true);
                    _State = PickupState.SHRINKING;
                    _isActive = false;
                }

                OnPickupParticles();
            }
        }
    }

    private void OnPickupParticles()
    {
        EnableParticles(OnPickUpParticles);
        DisableParticles(IdleParticles);
    }

    private void EnableParticles(List<ParticleSystem> lParticles)
    {
        foreach(ParticleSystem ps in lParticles)
        {
            ps.Play();
        }
    }

    private void DisableParticles(List<ParticleSystem> lParticles)
    {
        foreach (ParticleSystem ps in lParticles)
        {
            ps.Stop();
        }
    }

}
    


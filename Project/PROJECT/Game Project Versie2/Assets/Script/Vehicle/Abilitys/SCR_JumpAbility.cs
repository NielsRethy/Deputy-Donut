using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_JumpAbility : SCR_AbilityBase
{

    // Use this for initialization

    public float ForceStrength = 20000.0f;

    new private void Awake()
    {
        EnableActivateParticles(false);
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (_abilityIsActive)
	    {
	        if (SCR_ButtonMaster.Player1 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                {
                    EnableActivateParticles(true);
                    GameObject.FindWithTag("DonutTruck").GetComponent<Rigidbody>().AddForce(transform.up * ForceStrength, ForceMode.Impulse);
                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[8], GameObject.FindWithTag("DonutTruck"));
                    DisableAbilityTruck();
                }

            }
            else if (SCR_ButtonMaster.Player2 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {
                    EnableActivateParticles(true);
                    GameObject.FindWithTag("DonutTruck").GetComponent<Rigidbody>().AddForce(transform.up * ForceStrength, ForceMode.Impulse);
                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[8], GameObject.FindWithTag("DonutTruck"));
                    DisableAbilityTruck();
                }

            }




        }
	}
}

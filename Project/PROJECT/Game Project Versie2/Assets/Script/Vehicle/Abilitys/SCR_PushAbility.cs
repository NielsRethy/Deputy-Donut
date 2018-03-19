using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PushAbility : SCR_AbilityBase
{

    // Use this for initialization
    public float radius = 5.0F;
    public float power = 10.0F;

    void Start () {
        if (_AbilityIndicator != null)
        {
            _AbilityIndicator.transform.localScale = new Vector3(radius, 1, radius);
        }		
	}

    public override void OnPickUp(bool abilityOne)
    {
        _abilityIsActive = true;

        if (VisualModel != null)
        {
            SetVisualModelActive(true);
        }
        _abilityOne = abilityOne;

        EnableIndicator(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (_abilityIsActive)
        {
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                {
                    Vector3 explosionPos = transform.position;
                    Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
                    foreach (Collider hit in colliders)
                    {
                        Rigidbody rb = hit.GetComponent<Rigidbody>();


                        if (rb != null)
                        {
                            if (rb.tag == "PoliceCar")
                                rb.AddExplosionForce(power * 100, explosionPos, radius, 4.0F);
                            else
                                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                            
                        }
                    }
                    EnableActivateParticles(true);
                    EnableIndicator(false);
                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[8], GameObject.FindWithTag("DonutTruck"));
                    DisableAbilityTruck();
                }
            }
            else if (SCR_ButtonMaster.Player2 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {
                    Vector3 explosionPos = transform.position;
                    Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
                    foreach (Collider hit in colliders)
                    {
                        Rigidbody rb = hit.GetComponent<Rigidbody>();


                        if (rb != null)
                        {
                            if (rb.tag == "PoliceCar")
                                rb.AddExplosionForce(power * 100, explosionPos, radius, 4.0F);
                            else
                                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                        }
                        
                    }
                    EnableActivateParticles(true);
                    EnableIndicator(false);
                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[8], GameObject.FindWithTag("DonutTruck"));
                    DisableAbilityTruck();
                }
                
            }
        }
    }
}

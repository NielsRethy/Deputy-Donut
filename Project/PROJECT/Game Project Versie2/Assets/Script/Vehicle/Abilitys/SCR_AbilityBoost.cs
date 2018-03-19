using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AbilityBoost : SCR_AbilityBase
{

    // Use this for initialization
    public float Power = 10.0F;
    public ParticleSystem Particle;
    private GameObject _toPush;

    private bool soundIsPlaying = false;

    private void Start()
    {
        _toPush = transform.parent.gameObject;

        SetVisualModelActive(false);

        EnableActivateParticles(false);
        EnableIdleParticles(false);

    }

    public override void OnPickUp(bool abilityOne)
    {
        _abilityIsActive = true;
        SetVisualModelActive(true);
        _abilityOne = abilityOne;
        EnableIndicator(true);
        EnableIdleParticles(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (_abilityIsActive)
        {
            if (SCR_ButtonMaster.Player1 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                {
                    SlingShot();
                    if(!soundIsPlaying)
                    {
                        _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[8], GameObject.FindWithTag("PoliceCar"));
                        soundIsPlaying = true;
                    }
                }
            }

            else if (SCR_ButtonMaster.Player2 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {
                    SlingShot();
                    if (!soundIsPlaying)
                    {
                        _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[8], GameObject.FindWithTag("PoliceCar"));
                        soundIsPlaying = true;
                    }
                }
            }
        }
    }

    private void SlingShot()
    {
        EnableActivateParticles(true);
        EnableIdleParticles(false);
        EnableIndicator(false);

        Rigidbody Rb = _toPush.GetComponent<Rigidbody>();
        Vector3 velocity = Rb.velocity;

        velocity.Normalize();
        velocity *= Power;
        Rb.AddForce(velocity, ForceMode.Impulse);
        
        StartCoroutine("DestroyThruster");
    }

    IEnumerator DestroyThruster()
    {
        yield return new WaitForSeconds(1); // ~ 2 should be particleSystem.StartLifeTime

        EnableActivateParticles(false);
        soundIsPlaying = false;
        DisableAbilityPolice();
    }
}

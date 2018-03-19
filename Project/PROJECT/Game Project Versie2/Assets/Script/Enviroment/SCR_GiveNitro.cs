using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GiveNitro : MonoBehaviour {

    public bool _IsHit = false; // Is object already hit?
    SCR_PropRespawn _Respawn ;

    // Use this for initialization

    public AudioSource HitSound;
    public AudioClip HitClip;
    void Start()
    {
        _Respawn = GetComponent<SCR_PropRespawn>();
        if (HitSound!=null)
        {
            HitSound.clip = HitClip;
            HitSound.loop = false;
            HitSound.volume = 0.1f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_IsHit == false)    // Not hit yet
        {
            if (collision.transform.CompareTag("PoliceCar"))
            {
                SCR_Nitro policeNitroScript = collision.transform.GetComponent<SCR_Nitro>();
                policeNitroScript.AddPropNitro();
                Debug.Log("Gave reward nitro  nitro.");
                SCR_FlashUI.FlashNitroUIPolice.FlashImage();
                if (_Respawn != null)
                {
                    _Respawn.IsHit();

                }
                _IsHit = true;
                ParticleEffect();
                if (HitSound != null)
                {
                    HitSound.clip = HitClip;
                    HitSound.Play();
                }


            }
            else if (collision.transform.CompareTag("DonutTruck"))
            {
                SCR_Nitro DonutTruckNitroScript = collision.transform.GetComponent<SCR_Nitro>();
                DonutTruckNitroScript.AddPropNitro();
                SCR_FlashUI.FlashNitroUITruck.FlashImage();
                if (GetComponent<SCR_PropRespawn>()!= null)
                {
                    GetComponent<SCR_PropRespawn>().IsHit();
                }

                if (_Respawn != null)
                {
                    _Respawn.IsHit();

                }
                _IsHit = true;

                ParticleEffect();
                GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().ActivateBonusPoints = true;
                if (HitSound != null)
                {
                    HitSound.clip = HitClip;
                    HitSound.Play();
                }
            }
        }

        
    }

    void ParticleEffect()
    {
        // Make Particles
        // Change object color (for feedback to player)
        if (this.GetComponentInChildren<ParticleSystem>() != null)
        {
            this.GetComponentInChildren<ParticleSystem>().Play();
        }

    }
    public void ResetHit()
    {
        _IsHit = false;
    }
}

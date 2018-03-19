using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_TruckDestructionManager : MonoBehaviour {

	// Use this for initialization
    public List<GameObject> DestructableParts;
    public SCR_HitNumber NumberSpawner;
    public float Life = 100;
    public GameObject PoliceWin;
    //public Material MatTruck;

    public float DeathExplosionForce = 1000f;
    public Transform DeathExplosionForcePos;
    private float healthMax;
    private bool _death = false;

    [Header("Particles")]
    public float PercentStage1 = 75f;
    public float PercentStage2 = 50f;
    public float PercentStage3 = 25f;
    public List<ParticleSystem> ParticlesStage1 = new List<ParticleSystem>();
    public List<ParticleSystem> ParticlesStage2 = new List<ParticleSystem>();
    public List<ParticleSystem> ParticlesStage3 = new List<ParticleSystem>();
    public List<ParticleSystem> ParticlesDeath = new List<ParticleSystem>();
    private bool _stage1IsActive = false;
    private bool _stage2IsActive = false;
    private bool _stage3IsActive = false;
    private float _startStage1Value;
    private float _startStage2Value;
    private float _startStage3Value;



    void Start ()
    {
        healthMax = Life;
       // MatTruck.color = Color.yellow;

        float onePercent = healthMax / 100f;
        _startStage1Value = PercentStage1 * onePercent;
        _startStage2Value = PercentStage2 * onePercent;
        _startStage3Value = PercentStage3 * onePercent;
    }
	
	// Update is called once per frame
	void Update () {

	    if (Life < 0 &&!_death)
	    {
            if (SCR_ButtonMaster.Player1 == "Police")
            {
                SCR_GameManager.Manager.EndRound(true);
            }
            else
            {
                SCR_GameManager.Manager.EndRound(false);
            }
            SCR_NotificationQueue.NotificationTruck.DisplayText("My donuts...!");
            SCR_NotificationQueue.NotificationPolice.DisplayText("No more illegal donuts in my town.");

            _death = true;
            EnableParticles(ParticlesDeath);
            ExplosionApplyForce();

        }

        //Particle check health stages
        if (Life < _startStage1Value && !_stage1IsActive )
        {
            _stage1IsActive = true;
            EnableParticles(ParticlesStage1);
        }
        if (Life < _startStage2Value && !_stage2IsActive)
        {
            _stage2IsActive = true;
            EnableParticles(ParticlesStage2);
            SCR_NotificationQueue.NotificationPolice.DisplayText("Donut truck is at 50% health!");
        }
        if (Life < _startStage3Value && !_stage3IsActive)
        {
            _stage3IsActive = true;
            EnableParticles(ParticlesStage3);
            SCR_NotificationQueue.NotificationTruck.DisplayText("The truck is about to explode!");
        }

    }

    public void TakeDamage( float healt)
    {
        if(healt<0)
        {
            healt = 1;
        }
        Life -= healt;

        float c = 1.0f / healthMax * Life;
        //Color colors = MatTruck.color;
        //colors.g = c;
        //MatTruck.color = colors;

        NumberSpawner.DamageNumber(Mathf.FloorToInt(healt));

        //GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().FlaggActive = false;

    }

    private void ExplosionApplyForce()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForceAtPosition(transform.up * DeathExplosionForce, DeathExplosionForcePos.position);
        }
    }


    public float GetDamage()
    {
        return Life;
    }

    public float GetMaxHealth()
    {
        return healthMax;
    }

    private void EnableParticles(List<ParticleSystem> psList)
    {
        foreach (ParticleSystem ps in psList)
        {
            ps.Play();
        }
    }
}

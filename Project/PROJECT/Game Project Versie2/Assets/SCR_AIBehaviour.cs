using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AIBehaviour : MonoBehaviour {

    private bool _bFollowPath = false;
    public float TimeBeforeExploding = 5f;
    public float Speed;
    public List<ParticleSystem> FlameParticles = new List<ParticleSystem>();
    public List<ParticleSystem> ExplosionParticles = new List<ParticleSystem>();

    public float DissapearTime = 10f;
    public float ShrinkTime = 0.5f;

    private float _timeElapsed = 0f;
    private bool _bHasExploded = false;

    private bool _bIsLerping = false;
    private float _startLerpTime = 0.0f;
    private Vector3 _startScale;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (!_bFollowPath)
        {
            if (_timeElapsed > TimeBeforeExploding)
            {
                _bHasExploded = true;
                PlayParticles(ExplosionParticles);
                _timeElapsed = 0f;
                // change texture to black
            }
            else
            {
                _timeElapsed += Time.deltaTime;
                transform.Translate(transform.forward * Speed);

            }

            if (_bIsLerping)
            {
                LerpSize();
            }

            if (_bHasExploded)
            {
                _timeElapsed += Time.deltaTime;
                if (_timeElapsed > DissapearTime && !_bIsLerping)
                {
                    StartLerp();
                }
            }
        }
    }

    private void StartLerp()
    {
        _bIsLerping = true;
        _startScale = transform.localScale;
        _startLerpTime = Time.time;
    }

    private void LerpSize()
    {
        Vector3 scale = transform.localScale;

        float timeSinceStarted = Time.time - _startLerpTime;
        float percentageCompleted = timeSinceStarted / ShrinkTime;

        scale = Vector3.Lerp(_startScale, Vector3.zero, percentageCompleted);

        if (percentageCompleted >= 1)
        {
            _bIsLerping = false;
            Destroy(this);
        }

        transform.localScale = scale;
    }


    private void OnCollisionEnter(Collision collision)
    {
        //trein, andere auto, police, truck
        if ((collision.gameObject.CompareTag("Police") || collision.gameObject.CompareTag("DonutTruck")
            || collision.gameObject.CompareTag("AICar") || collision.gameObject.CompareTag("AITrain")) && _bFollowPath)
        {
            _bFollowPath = false;
            // stop following path -> script
            PlayParticles( FlameParticles );
        }
    }

    private void PlayParticles( List<ParticleSystem> particlesList)
    {
        foreach(ParticleSystem ps in particlesList)
        {
            ps.Play();
        }
        
    }

    private void StopParticles(List<ParticleSystem> particlesList)
    {
        foreach (ParticleSystem ps in particlesList)
        {
            ps.Stop();
        }
    }
}

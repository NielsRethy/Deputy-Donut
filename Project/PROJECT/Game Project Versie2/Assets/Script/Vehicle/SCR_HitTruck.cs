using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_HitTruck : MonoBehaviour {

    // Use this for initialization
    public SCR_TruckDestructionManager DestructionManager;

    [Header("Damage")]
    public float BaseHit = 5.0f;
    public float MaxDamage = 1.0f;          // damage = maxDamage * cos(angle) -> e.g. angle = 90 deg => Max damage / angle = 0 deg => 0 damage
    public float MinTimeBetweenHits = 0.5f;

    [Header("Particles")]
    public ParticleSystem Sparks;
    public float ParticleSpeedMultiplier = 20.0f;

    private float _timeSinceHit = 0;

    private void Start()
    {
        transform.GetComponent<SCR_OnCollisionEvents>().onCollisionEnter += DealDamageToTruck;  // script is on same object -> use transform.GetComponent<>()...
    }
    private void Update()
    {
        if(_timeSinceHit < MinTimeBetweenHits)
        {
            _timeSinceHit += Time.deltaTime;
        }
    }

    private void HitTruckParticles(float collisionStrength, Collision collision, Vector3 reflectionVec)
    {
        if (Sparks == null)
            return;

        ParticleSystem.MainModule psSettings = Sparks.main;
        // Start speed dependent on collision
        psSettings.startSpeed = ParticleSpeedMultiplier * collision.relativeVelocity.magnitude;
        Debug.Log("Particle speed: " + psSettings.startSpeed.ToString());
        // Correct position and rotation
        Sparks.transform.position = collision.contacts[0].point;
        Sparks.transform.rotation = Quaternion.FromToRotation(Vector3.forward, reflectionVec);

        Sparks.Play();
    }

    private void DealDamageToTruck(Collision collision)
    {
        if (_timeSinceHit > MinTimeBetweenHits)
        {
            
            Debug.Log(collision.gameObject.name);
            if (collision.transform.CompareTag("DonutTruck") || collision.gameObject.CompareTag("AICar"))
            {
                ContactPoint contact = collision.contacts[0];
                Vector3 inNorm = contact.normal;
                Vector3 reflectDir = Vector3.Reflect(transform.forward, inNorm);

                //Angle between collison and collision plane normal
                float angleDeg = Vector3.Angle(inNorm, collision.relativeVelocity);
                float angleRad = angleDeg * Mathf.PI / 180;     // in radians

                float collisionStrength = Mathf.Cos(angleRad); // 0 - 1 range -> multiply this value by Some Force

                // Emit particles
                HitTruckParticles(collisionStrength, collision, reflectDir);

                // Drop flag
                GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().DropFlag(collision.relativeVelocity, collisionStrength);

                // Deal damage
                int damage = Mathf.RoundToInt(collisionStrength * collision.relativeVelocity.magnitude );
                DestructionManager.TakeDamage(damage);
                _timeSinceHit = 0;


            }
        }
    }


}

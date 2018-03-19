using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CollisionParticles : MonoBehaviour {

    public float TimeBetweenCollisions = 0.2f;
    private float _timer;
    public ParticleSystem Sparks;

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("DonutTruck"))
        {
            if (_timer > TimeBetweenCollisions)
            {
                if (Sparks != null)
                {
                    ContactPoint contact = collision.contacts[0];

                    Sparks.transform.position = contact.point;
                    Vector3 inDir = transform.TransformDirection(Vector3.forward);
                    Vector3 inNorm = contact.normal;
                    Vector3 reflectDir = Vector3.Reflect(collision.relativeVelocity, inNorm);
              
                    float angle = Vector3.Angle(inNorm, collision.relativeVelocity);
                    angle = angle * Mathf.PI / 180;     // in radians

                    float relativeForce = Mathf.Cos(angle);

                    Sparks.transform.rotation = Quaternion.FromToRotation(Vector3.forward, reflectDir);

                    Sparks.Play();
                    _timer = 0.0f;
                }

            }
        }
    }


}

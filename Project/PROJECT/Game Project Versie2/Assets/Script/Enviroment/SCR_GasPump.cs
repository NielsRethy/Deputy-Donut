using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GasPump : MonoBehaviour {

    public GameObject CleanMesh;          // make child = private
    public GameObject DestroyedMesh;      // make child = private

    public float ExplosionRadius = 5.0f;
    public float ExplosionStrength = 20000.0f;
    public float PlayerExplosionMultiplier = 50.0f;

    public List<ParticleSystem> ListExplosionParticles = new List<ParticleSystem>();

    private bool _bIsDestroyed;

	// Use this for initialization
	void Start () {
        // set Boxcollider
       

       CleanMesh.SetActive(!_bIsDestroyed);
       DestroyedMesh.SetActive(_bIsDestroyed);
	}

    private void SetMeshAndCollider()
    {
        if (_bIsDestroyed)
        {
            BoxCollider boxColl = GetComponent<BoxCollider>();
            boxColl.size = DestroyedMesh.transform.localScale;
            boxColl.center = DestroyedMesh.transform.position;
        }
        else
        {
            BoxCollider boxColl = GetComponent<BoxCollider>();
            boxColl.size = CleanMesh.transform.localScale;
            boxColl.center = CleanMesh.transform.position;
        }

        CleanMesh.SetActive(!_bIsDestroyed);
        DestroyedMesh.SetActive(_bIsDestroyed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_bIsDestroyed)
        {
            if (collision.transform.CompareTag("DonutTruck") || collision.transform.CompareTag("PoliceCar"))
            {
                _bIsDestroyed = true;
                SetMeshAndCollider();
                // play explosion
                PlayParticles();
                ExplosionKnockBack();
                // tell gasstation it has been destroyed
                GameObject gasStation = transform.parent.gameObject;
                gasStation.GetComponent<SCR_GasStationDestruction>().NotifyGasPumpDestroyed();
            }
        }
    }

    private void PlayParticles()
    {
        foreach(ParticleSystem ps in ListExplosionParticles)
        {
            ps.Play();
        }
    }


    private void ExplosionKnockBack()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);
        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (rb.CompareTag("DonutTruck") || rb.CompareTag("PoliceCar"))
                {
                    rb.AddExplosionForce(ExplosionStrength * PlayerExplosionMultiplier, explosionPos, ExplosionRadius);
                } 
            }
        }
    }
}

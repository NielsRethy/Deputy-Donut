using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GasStationDestruction : MonoBehaviour
{

    private int _gasPumpsDestoyedTrigger = 2;
    public GameObject CleanMesh;
    public GameObject DestoyedMesh;
    [Header("Explosion")]
    public float ExplosionStrength = 40000f;
    public float ExplosionRadius = 40f;
    public float PlayerExplosionMultiplier = 150f;
    public List<ParticleSystem> ListExplosionParticles = new List<ParticleSystem>();

    private List<GameObject> _listGasPumps = new List<GameObject>();
    private bool _bIsDestoyed = false;
    private int _pumpsExploded = 0;


    // Use this for initialization
    void Start()
    {
        // get list of gaspumps
        foreach (Transform child in transform)
        {
            if (child.CompareTag("GasPump"))
            {
                _listGasPumps.Add(child.gameObject);
            }
        }

        _gasPumpsDestoyedTrigger = _listGasPumps.Count;

        CleanMesh.SetActive(!_bIsDestoyed);
        DestoyedMesh.SetActive(_bIsDestoyed);
    }

    private void ExplodeGasStation()
    {
        _bIsDestoyed = true;
        ExplosionKnockBack();
        StartCoroutine("DelayChangeMeshes");
        PlayParticles();
    }

    private void PlayParticles()
    {
        foreach (ParticleSystem ps in ListExplosionParticles)
        {
            ps.Play();
        }
    }


    IEnumerator DelayChangeMeshes()
    {
        yield return new WaitForSeconds(0.15f);
        CleanMesh.SetActive(!_bIsDestoyed);
        DestoyedMesh.SetActive(_bIsDestoyed);
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

    public void NotifyGasPumpDestroyed()
    {
        ++_pumpsExploded;
        if (_pumpsExploded >= _gasPumpsDestoyedTrigger)
        {
            ExplodeGasStation();
        }
    }


}

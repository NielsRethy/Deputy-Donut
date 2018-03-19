using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Mine : MonoBehaviour {

    public float DamageTake = 10;
    public float ForceStrength = 20000.0f;
    private GameObject police;
    private GameObject truck;
    public bool Mine = false;
    public List<ParticleSystem> OnActivatePS;
    public List<ParticleSystem> IdlePs;

    private void Awake()
    {
        EnableExplosionPS(false);
        EnableIdlePS(true);
    }

    // Use this for initialization
    void Start () {
        police = GameObject.FindGameObjectWithTag("PoliceCar");
        truck = GameObject.FindGameObjectWithTag("DonutTruck");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (Mine)
        {
            if (other.tag == "DonutTruck")
            {
                EnableExplosionPS(true);

                other.gameObject.GetComponent<SCR_TruckDestructionManager>().TakeDamage(DamageTake);

                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(ForceStrength, gameObject.transform.position, 1.5f, 4.0F);

                other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * ForceStrength, ForceMode.Impulse);

                GameObject mineMesh = transform.GetChild(0).gameObject;
                mineMesh.SetActive(false);
                StartCoroutine("SelfDestruct");
            }
            if (other.tag == "Ground")
            {
                GetComponentInParent<Rigidbody>().isKinematic = true;
            }
        }
        else
        {
            if (other.tag == "PoliceCar")
            {
                EnableExplosionPS(true);
                //  truck.gameObject.GetComponent<Rigidbody>().AddExplosionForce(ForceStrength, truck.transform.position, 1.5f, 4.0F);
                Vector3 direction = police.transform.position - truck.transform.position;
                direction = direction.normalized;
                direction.y += 0.2f;
                other.gameObject.GetComponent<Rigidbody>().AddForce(direction * ForceStrength, ForceMode.Impulse);
            }
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2);
        EnableExplosionPS(false);
        EnableIdlePS(true);
        gameObject.SetActive(false);
    }

    private void EnableExplosionPS(bool enable)
    {
        foreach(ParticleSystem ps in OnActivatePS)
        {
            if (enable)
                ps.Play();
            else ps.Stop();
        }
    }

    private void EnableIdlePS(bool enable)
    {
        foreach (ParticleSystem ps in IdlePs)
        {
            if (enable)
                ps.Play();
            else ps.Stop();
        }
    }
}

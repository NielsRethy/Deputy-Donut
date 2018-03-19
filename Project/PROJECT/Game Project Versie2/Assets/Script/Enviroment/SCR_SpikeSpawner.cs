using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SpikeSpawner : MonoBehaviour {

    public Transform SpawnLocation;
    public bool ActivateFlow = false;
    public GameObject[] SpikeObjArr;
    private GameObject[] _SpikeArr;
    public int MaxSpike = 100;
    public Vector3 SpawnDirection = new Vector3(1, 0, 0);
    public float EjectForce = 1.0f;
    public float EjectAngle = 45;
    public float TimeOnRoad = 5;
    //spawn paras
    public float Width = 1.0f;
    public float Height = 1.0f;
    public float SpikePerSecond = 2;

    //timed
    private float _TimeSinceLastSpike;
    private int _CurrSpike = 0;
    // Use this for initialization
    //rescource
    public float SpikeRescource = 100;
    public float SpikesPerSecondConsumed = 50;

    void Start ()
    {
        if (SpikeObjArr.Length > 0)
        {
            int index;
            _SpikeArr = new GameObject[MaxSpike];
            for (int i = 0; i < MaxSpike; i++)
            {
                index = Random.Range(0, SpikeObjArr.Length);


                _SpikeArr[i] = Instantiate(SpikeObjArr[index]);
                _SpikeArr[i].GetComponent<SCR_Donut>().TimeToDestroy = TimeOnRoad;
                _SpikeArr[i].SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {




        if (SCR_ButtonMaster.Player1 == "Police")
        {
            if (Input.GetAxisRaw(SCR_ButtonMaster.Master.Player1Ability2) > 0)
            {
                ActivateFlow = true;

            }
            else
            {
                ActivateFlow = false;

            }
        }
        if (SCR_ButtonMaster.Player2 == "Police")
        {
            if (Input.GetAxisRaw(SCR_ButtonMaster.Master.Player2Ability2) > 0)
            {
                ActivateFlow = true;

            }
            else
            {
                ActivateFlow = false;

            }
        }





        if (SpikeObjArr.Length > 0)
        {
            if ((ActivateFlow) && SpikeRescource > 0)
            {
                SpikeRescource -= SpikesPerSecondConsumed * Time.deltaTime;
                _TimeSinceLastSpike += Time.deltaTime;
                if (_TimeSinceLastSpike > 1 / SpikePerSecond)
                {
                    _TimeSinceLastSpike = 0;
                    _CurrSpike++;

                    Vector3 posOffset;
                    posOffset.x = Random.Range(-Width, Width);
                    posOffset.y = Random.Range(-Height, Height);
                    posOffset.z = 0;
                    posOffset = SpawnLocation.rotation * posOffset;
                    for (int i = 0; i < _SpikeArr.Length; i++)
                    {
                        if (!_SpikeArr[i].activeSelf)
                        {

                            _SpikeArr[i].transform.position = SpawnLocation.position + posOffset;

                            float randAngle = Random.Range(0, EjectAngle);
                            randAngle -= EjectAngle / 2;
                            Vector3 ejectan = new Vector3(randAngle, 0, 0);
                            Vector3 force = SpawnLocation.rotation * ejectan;
                            force += SpawnLocation.rotation * new Vector3(0, 0, -EjectForce);
                            _SpikeArr[i].GetComponent<Rigidbody>().velocity = force;
                            _SpikeArr[i].SetActive(true);
                            break;
                        }
                    }
                }
            }

        }
    }
    public int GetSpikes()
    {
        return (int)SpikeRescource;
    }
}

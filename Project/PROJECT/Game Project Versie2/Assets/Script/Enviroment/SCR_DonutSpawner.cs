using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DonutSpawner : MonoBehaviour
{
    public Transform SpawnLocation;
    public bool ActivateFlow = false;
    public GameObject[] DonutsObjectArr;
    private GameObject[] _DonutArr;
    public int MaxDonuts = 100;
    public Vector3 SpawnDirection =new Vector3 (1,0,0 );
    public float EjectForce = 1.0f;
    public float EjectAngle = 45;
    public float TimeOnRoad = 5;
    //spawn paras
    public float Width = 1.0f;
    public float Height = 1.0f;
    public float DonutsPerSeconds = 2;


     //timed
    private float _TimeSinceLastDonut;
    private int _CurrDonuts = 0;
    // Use this for initialization
    //rescource
    public float DonutsRescource = 100;
    public float DonutsPerSecondConsumed = 50;
    

    void Start()
    {
        if (DonutsObjectArr.Length>0)
        {
            int index;
            _DonutArr = new GameObject[MaxDonuts];
            for (int i = 0; i < MaxDonuts; i++)
            {
                index = Random.Range(0, DonutsObjectArr.Length);


                _DonutArr[i] = Instantiate(DonutsObjectArr[index]);
                _DonutArr[i].GetComponent<SCR_Donut>().TimeToDestroy = TimeOnRoad;
                _DonutArr[i].SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (SCR_ButtonMaster.Player1 == "Truck")
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
        if (SCR_ButtonMaster.Player2 == "Truck")
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
     
        if (DonutsObjectArr.Length > 0)
        {
            if ((ActivateFlow) && DonutsRescource>0)
            {
                DonutsRescource -= DonutsPerSecondConsumed * Time.deltaTime;
                _TimeSinceLastDonut += Time.deltaTime;
                if (_TimeSinceLastDonut > 1 / DonutsPerSeconds)
                {
                    _TimeSinceLastDonut = 0;
                    _CurrDonuts++;

                    Vector3 posOffset;
                    posOffset.x = Random.Range(-Width, Width);
                    posOffset.y = Random.Range(-Height, Height);
                    posOffset.z = 0;
                    posOffset = SpawnLocation.rotation * posOffset;
                    for (int i = 0; i < _DonutArr.Length; i++)
                    {
                        if (!_DonutArr[i].activeSelf)
                        {
                               
                            _DonutArr[i].transform.position = SpawnLocation.position + posOffset;

                            float randAngle = Random.Range(0, EjectAngle);
                            randAngle -= EjectAngle / 2;
                            Vector3 ejectan = new Vector3(randAngle, 0, 0);
                            Vector3 force = SpawnLocation.rotation * ejectan ;
                            force += SpawnLocation.rotation * new Vector3(0,0,-EjectForce);
                            _DonutArr[i].GetComponent<Rigidbody>().velocity = force;
                            _DonutArr[i].SetActive(true);
                            break;
                        }
                    }
                }
            }

        }
    }
        
    public int GetDonuts()
    {
        return (int) DonutsRescource;
    }
}

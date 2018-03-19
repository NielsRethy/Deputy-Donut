using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_MasterDonutCounter : MonoBehaviour {

    public GameObject[] PossibleDonutsArr;
    public int MaxDonutsInGame = 1000;

    private GameObject[] _DonutsArr;
    private int _DonutsInTruck = 0;
    // Use this for initialization
    void Start()
    {
        int index;
        _DonutsArr = new GameObject[MaxDonutsInGame];
        for (int i = 0; i < _DonutsArr.Length; i++)
        {
            index = Random.Range(0, PossibleDonutsArr.Length);

            _DonutsArr[i] = Instantiate(PossibleDonutsArr[index]);
            _DonutsArr[i].SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        _DonutsInTruck = 0;
        for (int i = 0; i < _DonutsArr.Length; i++)
        {
            if (_DonutsArr[i].GetComponent<SCR_Donut>().IsInTruck())
            {
                _DonutsInTruck++;
            }
        }
    }
    public void AddDonut(Vector3 position)
    {
        for (int i = 0; i < _DonutsArr.Length; i++)
        {
           if( !_DonutsArr[i].activeSelf)
            {
                _DonutsArr[i].transform.position=position;
                _DonutsArr[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                _DonutsArr[i].SetActive(true);
                break;
            }
        }
    }
    public bool RemoveFromTruckDonuts(int numberToRemove)
    {
        if (numberToRemove < 0)
            numberToRemove = 0;
        if(_DonutsInTruck<numberToRemove)
        {
            return false;
        }
        int counter = 0;
        for (int i = 0; i < _DonutsArr.Length; i++)
        {
            if (_DonutsArr[i].GetComponent<SCR_Donut>().IsInTruck())
            {
                _DonutsArr[i].SetActive(false);
                counter++;
                if (counter <= numberToRemove)
                    break;
            }
        }
        return true;
    } 
    public int GetDonutsInTruck()
    {
        return _DonutsInTruck;
    }
}

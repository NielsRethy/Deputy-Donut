using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_RoundObjectSpawner : MonoBehaviour {

	// Use this for initialization
    public List<GameObject> Positions;
    private int _count;

	void Start () {
        var n = Random.Range(0, Positions.Count);
        Positions[n].SetActive(true);
	    GameObject.FindWithTag("DonutTruck").GetComponentInChildren<SCR_ArrowPointToObject>().FollowObject = Positions[n];
	    foreach (var t in GameObject.FindWithTag("HUD").GetComponentsInChildren<Text>())
	    {
	        if (t.name == "CollectTextMax")
	        {
	            t.text = Positions.Count.ToString();

            }
	    }

    }
	
	// Update is called once per frame
    public void deleteOutArray(GameObject g)
    {
        Positions.Remove(g);
        if (Positions.Count > 0)
        {
            _count++;
            var n = Random.Range(0, Positions.Count);
            Positions[n].SetActive(true);
            GameObject.FindWithTag("DonutTruck").GetComponentInChildren<SCR_ArrowPointToObject>().FollowObject = Positions[n];

            foreach (var t in GameObject.FindWithTag("HUD").GetComponentsInChildren<Text>())
            {
                if (t.name == "CollectTextBehaald")
                {
                    t.text = _count.ToString();

                }
            }
        }
        else
        {
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                SCR_GameManager.Manager.EndRound(true);
            }
            else
            {
                SCR_GameManager.Manager.EndRound(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TRSortingLayer : MonoBehaviour {


	// Use this for initialization
	void Start () {
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.sortingLayerName = "TrailRenderer";
	}
}

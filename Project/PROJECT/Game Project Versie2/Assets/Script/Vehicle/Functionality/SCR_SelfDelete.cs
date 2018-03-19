using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SelfDelete : MonoBehaviour {

    public float TimeBeforeDelete = 5;
    private float _timer;

	// Use this for initialization
	void Start () {
        _timer = TimeBeforeDelete;
	}
	
	// Update is called once per frame
	void Update () {
        //Count down
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            Destroy(this);
        }
	}

}

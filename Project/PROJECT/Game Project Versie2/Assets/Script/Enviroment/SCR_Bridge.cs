using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Bridge : MonoBehaviour {

    public float OpenTime = 10;
    public float ClosedTime = 10;
    private Animator anim;
    private float _Time;

    enum BridgeState
    {
        Open,
        Close
    };
    private BridgeState State = BridgeState.Open;
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetBool("OpenTrigger", true);

    }

    // Update is called once per frame
    void Update () {
        _Time += Time.deltaTime;
        switch(State)
        {
            case BridgeState.Open:
                if(_Time>OpenTime)
                {
                    _Time = 0;
                    State = BridgeState.Close;
                    anim.SetTrigger("OpenTrigger");


                }
                break;
            case BridgeState.Close:
                if (_Time > ClosedTime)
                {
                    _Time = 0;

                    anim.SetTrigger("OpenTrigger");

                    State = BridgeState.Open;

                }
                break;
        }
		
	}
}

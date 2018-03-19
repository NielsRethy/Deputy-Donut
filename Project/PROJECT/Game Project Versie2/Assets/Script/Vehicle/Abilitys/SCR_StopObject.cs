using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_StopObject : MonoBehaviour {
    public float SlowTime = 2;
    private Animator anim;
    private GameObject Cop;
    private float _TimeSlowed =0;
    public float Delay = 0.1f;
    private bool _ActivateDelay = false;
    private bool _Activated = false;
    private float _Delay = 0;
    private 
    // Use this for initialization
    void Start () {
        Cop = GameObject.FindGameObjectWithTag("PoliceCar");
        Physics.IgnoreLayerCollision(10, 12);
        Physics.IgnoreLayerCollision(11, 12);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
		if(_ActivateDelay)
        {
            _Delay += Time.deltaTime;
            if (_Delay > Delay)
            {
                anim.SetBool("ClampTrigger", true);

                _ActivateDelay = false;
                _Activated = true;
                _Delay = 0;
                _TimeSlowed = Time.deltaTime;
                Cop.GetComponent<Rigidbody>().isKinematic = true;
                Cop.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                Cop.GetComponent<SRC_CarControllerInput>().SetDisabled(true);
            }
        }
      
        if(_Activated)
        {
            _TimeSlowed += Time.deltaTime;
            if (_TimeSlowed > SlowTime)
            {
                _TimeSlowed = 0;
                _Activated = false;
                Cop.GetComponent<Rigidbody>().isKinematic = false;

                Cop.GetComponent<SRC_CarControllerInput>().SetDisabled(false);
                anim.SetBool("Reset", true);

                gameObject.active = false;
            }
        }
      
      
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PoliceCar")
        {

            _ActivateDelay = true;
           
        }
    }

}

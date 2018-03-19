using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AbilityShield :  SCR_AbilityBase
{ 
    public float RotationSpeed = 90;
    public float Duration=5;
    public int NumberOfSHields = 2;
    public float Distance = 2.7f;

    public GameObject Parent;
    public GameObject Shield;

    public bool _Spin = false;
    private float _TimePassed;
    private GameObject[] _ShieldsArr;
  
    void Start ()
    {
        
        Physics.IgnoreLayerCollision(16,15 , true);
        Physics.IgnoreLayerCollision(16, 14, true);
        Physics.IgnoreLayerCollision(16, 13, true);

        _ShieldsArr = new GameObject[NumberOfSHields];
        for(int i=0; i<NumberOfSHields; i++)
        {
            _ShieldsArr[i] = Instantiate(Shield, Parent.transform);

            Vector3 pos;
            float angle = 360 *i/ NumberOfSHields;
            pos.x = Distance * Mathf.Sin(angle * Mathf.Deg2Rad);
            pos.z = Distance * Mathf.Cos(angle * Mathf.Deg2Rad);
            pos.y = 0;
            _ShieldsArr[i].transform.localPosition = pos;
            _ShieldsArr[i].transform.Rotate(new Vector3(0, angle, 0));
        }
        Parent.SetActive(false);

    }

    // Update is called once per frame
    void Update ()
    {
        if (_abilityIsActive)
        {
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                {
                    _Spin = true;
                    Parent.SetActive(true);


                        GameObject.FindWithTag("DonutTruck").GetComponent<AudioSource>().loop = true;
                        _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[4], GameObject.FindWithTag("DonutTruck"));
                                    
                }
            }

            else if (SCR_ButtonMaster.Player2 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {
                    _Spin = true;
                    Parent.SetActive(true);

                    GameObject.FindWithTag("DonutTruck").GetComponent<AudioSource>().loop = true;
                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[4], GameObject.FindWithTag("DonutTruck"));
                }
            }
        }

        if (_Spin)
        {
            _TimePassed += Time.deltaTime;
            Parent.transform.Rotate(new Vector3(0, RotationSpeed * Time.deltaTime, 0));
            for (int i = 0; i < NumberOfSHields; i++)
            {
                Vector3 pos;
                float angle = 360 * i / NumberOfSHields;
                pos.x = Distance * Mathf.Sin(angle * Mathf.Deg2Rad);
                pos.z = Distance * Mathf.Cos(angle * Mathf.Deg2Rad);
                pos.y = 0;
                _ShieldsArr[i].transform.localPosition = pos;
            }
            if (_TimePassed>Duration)
            {
                _Spin = false;
                Parent.SetActive(false);
                _TimePassed = 0;
                _abilityIsActive = false;

                GameObject.FindWithTag("DonutTruck").GetComponent<AudioSource>().loop = false;

                if (_abilityOne)
                {
                    if (_abilityOne)
                    {
                        SCR_AbilityManager.TruckAbilityActive = false;
                    }
                    else
                    {
                        SCR_AbilityManager.TruckAbilityActive2 = false;

                    }
                }
                else
                {
                    if (_abilityOne)
                    {
                        SCR_AbilityManager.TruckAbilityActive = false;
                    }
                    else
                    {
                        SCR_AbilityManager.TruckAbilityActive2 = false;

                    }

                }

            }

        }
    }

}

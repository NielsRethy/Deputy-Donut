using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AbilityFlameThrower : SCR_AbilityBase
{
    public SCR_FlameThrower  FlameScript;
    public float DamagerPerTick = 10;
    public float TickPerSecond = 2;
    public float TimeActive = 5;

    private bool _SecondActive = false;
    private float _TimeAlreadyActive = 0;
    void Start () {
        FlameScript.TickPerSecond = TickPerSecond;
        FlameScript.DamagerPerTick = DamagerPerTick;
        FlameScript.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_abilityIsActive)
        {
            if (SCR_ButtonMaster.Player1 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                {
                    if (!_SecondActive)
                    {
                        FlameScript.gameObject.SetActive(true);
                        EnableActivateParticles(true);
                        EnableIdleParticles(false);
                        _SecondActive = true;
                    }
                }

            }
            else if (SCR_ButtonMaster.Player2 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {
                    if (!_SecondActive)
                    {
                        FlameScript.gameObject.SetActive(true);
                        _SecondActive = true;
                        EnableActivateParticles(true);
                        EnableIdleParticles(false);

                    }
                }
            }
        }

        if(_SecondActive)
        {
            _TimeAlreadyActive += Time.deltaTime;
        }
        if(_TimeAlreadyActive>TimeActive)
        {
            _TimeAlreadyActive = 0;
            EnableActivateParticles(false);

            DisableAbilityPolice();
            _SecondActive = false;


        }

    }

    public override void OnPickUp(bool abilityOne)
    {
        _abilityIsActive = true;

        SetVisualModelActive(true);
        EnableIdleParticles(true);
        _abilityOne = abilityOne;
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MolotovSpawner : SCR_AbilityBase {

    public GameObject MolotovObject;
    public Transform SpawnLocation;
    private GameObject[] _MolotovObjectArr;

    private int _NextToSpawnIndex = 0;
    private int _MaxMolotofs = 10;
  
    void Start () {
        if (MolotovObject != null)
        {
            _MolotovObjectArr = new GameObject[_MaxMolotofs];
            for (int i = 0; i < _MaxMolotofs; i++)
            {
                _MolotovObjectArr[i] = Instantiate(MolotovObject);
                _MolotovObjectArr[i].SetActive(false);
            }
        }
        if(VisualModel!=null)
        {
            VisualModel.active = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_abilityIsActive)
        {
            if (SCR_ButtonMaster.Player1 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                {
                    if (MolotovObject != null)
                    {
                        _MolotovObjectArr[_NextToSpawnIndex].SetActive(true);
                        _MolotovObjectArr[_NextToSpawnIndex].transform.position = SpawnLocation.transform.position;
                        _MolotovObjectArr[_NextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Quaternion rot = SpawnLocation.parent.transform.rotation;

                        _MolotovObjectArr[_NextToSpawnIndex].transform.rotation = rot;
                        _MolotovObjectArr[_NextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));

                        _NextToSpawnIndex++;
                        if (_NextToSpawnIndex == _MaxMolotofs)
                        {
                            _NextToSpawnIndex = 0;
                        }
                    }
                    _abilityIsActive = false;


                    if (_abilityOne)
                    {
                        SCR_AbilityManager.PoliceAbilityActive = false;
                    }
                    else
                    {
                        SCR_AbilityManager.PoliceAbilityActive2 = false;

                    }
                    OnEnd();
                }

            }
            else if (SCR_ButtonMaster.Player2 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {

                    if (MolotovObject != null)
                    {
                        _MolotovObjectArr[_NextToSpawnIndex].SetActive(true);
                        _MolotovObjectArr[_NextToSpawnIndex].transform.position = SpawnLocation.transform.position;
                        _MolotovObjectArr[_NextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Quaternion rot = SpawnLocation.parent.transform.rotation;

                        _MolotovObjectArr[_NextToSpawnIndex].transform.rotation = rot;
                        _MolotovObjectArr[_NextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));

                        _NextToSpawnIndex++;
                        if (_NextToSpawnIndex == _MaxMolotofs)
                        {
                            _NextToSpawnIndex = 0;
                        }
                    }
                    _abilityIsActive = false;

                    if (_abilityOne)
                    {
                        SCR_AbilityManager.PoliceAbilityActive = false;
                    }
                    else
                    {
                        SCR_AbilityManager.PoliceAbilityActive2 = false;

                    }

                    OnEnd();
                }

            }
        }
    }
}

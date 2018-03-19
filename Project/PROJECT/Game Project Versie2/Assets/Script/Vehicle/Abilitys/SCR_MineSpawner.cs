using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MineSpawner : SCR_AbilityBase {

    public GameObject MineObject;
    public Transform SpawnLocation;
    public Transform SpawnLocation2;
    public Transform SpawnLocation3;

    private GameObject[] _mineObjectArr;

    private int _nextToSpawnIndex = 0;
    private int _maxMines = 21;
 
    void Start ()
    {

        if (MineObject != null)
        {
            _mineObjectArr = new GameObject[_maxMines];
            for (int i = 0; i < _maxMines; i++)
            {
                _mineObjectArr[i] = Instantiate(MineObject);
                _mineObjectArr[i].SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_abilityIsActive)
        {
            if (SCR_ButtonMaster.Player1 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                {
                    if (MineObject != null)
                    {
                        _mineObjectArr[_nextToSpawnIndex].SetActive(true);
                        _mineObjectArr[_nextToSpawnIndex].transform.position = SpawnLocation.transform.position;
                        _mineObjectArr[_nextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Quaternion rot = SpawnLocation.parent.transform.rotation;

                        _mineObjectArr[_nextToSpawnIndex].transform.rotation = rot;
                        _mineObjectArr[_nextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));

                        _nextToSpawnIndex++;
                        if (_nextToSpawnIndex == _maxMines)
                        {
                            _nextToSpawnIndex = 0;
                        }
                        _mineObjectArr[_nextToSpawnIndex].SetActive(true);
                        _mineObjectArr[_nextToSpawnIndex].transform.position = SpawnLocation2.transform.position;
                        _mineObjectArr[_nextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        rot = SpawnLocation.parent.transform.rotation;

                        _mineObjectArr[_nextToSpawnIndex].transform.rotation = rot;
                        _mineObjectArr[_nextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));
                        _nextToSpawnIndex++;
                        if (_nextToSpawnIndex == _maxMines)
                        {
                            _nextToSpawnIndex = 0;
                        }
                        _mineObjectArr[_nextToSpawnIndex].SetActive(true);
                        _mineObjectArr[_nextToSpawnIndex].transform.position = SpawnLocation3.transform.position;
                        _mineObjectArr[_nextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        rot = SpawnLocation.parent.transform.rotation;

                        _mineObjectArr[_nextToSpawnIndex].transform.rotation = rot;
                        _mineObjectArr[_nextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));
                        _nextToSpawnIndex++;
                        if (_nextToSpawnIndex == _maxMines)
                        {
                            _nextToSpawnIndex = 0;
                        }
                        _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[2], GameObject.FindWithTag("DonutTruck"));
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

                    if (VisualModel != null)
                    {
                        VisualModel.SetActive(false);
                    }
                }

            }
            else if (SCR_ButtonMaster.Player2 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {

                    if (MineObject != null)
                    {
                        _mineObjectArr[_nextToSpawnIndex].SetActive(true);
                        _mineObjectArr[_nextToSpawnIndex].transform.position = SpawnLocation.transform.position;
                        _mineObjectArr[_nextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Quaternion rot = SpawnLocation.parent.transform.rotation;

                        _mineObjectArr[_nextToSpawnIndex].transform.rotation = rot;
                        _mineObjectArr[_nextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));

                        _nextToSpawnIndex++;
                        if (_nextToSpawnIndex == _maxMines)
                        {
                            _nextToSpawnIndex = 0;
                        }
                        _mineObjectArr[_nextToSpawnIndex].SetActive(true);
                        _mineObjectArr[_nextToSpawnIndex].transform.position = SpawnLocation2.transform.position;
                        _mineObjectArr[_nextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        rot = SpawnLocation.parent.transform.rotation;

                        _mineObjectArr[_nextToSpawnIndex].transform.rotation = rot;
                        _mineObjectArr[_nextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));
                        _nextToSpawnIndex++;
                        if (_nextToSpawnIndex == _maxMines)
                        {
                            _nextToSpawnIndex = 0;
                        }
                        _mineObjectArr[_nextToSpawnIndex].SetActive(true);
                        _mineObjectArr[_nextToSpawnIndex].transform.position = SpawnLocation3.transform.position;
                        _mineObjectArr[_nextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        rot = SpawnLocation.parent.transform.rotation;

                        _mineObjectArr[_nextToSpawnIndex].transform.rotation = rot;
                        _mineObjectArr[_nextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));
                        _nextToSpawnIndex++;
                        if (_nextToSpawnIndex == _maxMines)
                        {
                            _nextToSpawnIndex = 0;
                        }

                        _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[2], GameObject.FindWithTag("DonutTruck"));
                    }
                    DisableAbilityPolice();
                }

            }
        }
    }
}

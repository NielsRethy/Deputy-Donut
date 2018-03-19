using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AbilityStop : SCR_AbilityBase
{
    public GameObject StopPrefab;
    public Transform SpawnLocation;
    public int MaxPrefabs = 5;

    private GameObject[] _SpawnArr;
    private int _NextToSpawnIndex = 0;
    // Use this for initialization
    void Start ()
    {
        _SpawnArr = new GameObject[MaxPrefabs];

        for (int i = 0; i < MaxPrefabs; i++)
        {
            _SpawnArr[i] = Instantiate(StopPrefab);
            _SpawnArr[i].SetActive(false);
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (_abilityIsActive)
        {
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                {
                    _SpawnArr[_NextToSpawnIndex].SetActive(true);
                    _SpawnArr[_NextToSpawnIndex].transform.position = SpawnLocation.transform.position;
                    _SpawnArr[_NextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                    Quaternion rot = SpawnLocation.parent.transform.rotation;

                    _SpawnArr[_NextToSpawnIndex].transform.rotation = rot;
                    _SpawnArr[_NextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));

                    _NextToSpawnIndex++;
                    if (_NextToSpawnIndex == MaxPrefabs)
                    {
                        _NextToSpawnIndex = 0;
                    }
                    DisableAbilityTruck();

                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[2], GameObject.FindWithTag("DonutTruck"));
                }
            }

            else if (SCR_ButtonMaster.Player2 == "Truck")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                {

                    _SpawnArr[_NextToSpawnIndex].SetActive(true);
                    _SpawnArr[_NextToSpawnIndex].transform.position = SpawnLocation.transform.position;
                    _SpawnArr[_NextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                    Quaternion rot = SpawnLocation.parent.transform.rotation;

                    _SpawnArr[_NextToSpawnIndex].transform.rotation = rot;
                    _SpawnArr[_NextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));

                    _NextToSpawnIndex++;
                    if (_NextToSpawnIndex == MaxPrefabs)
                    {
                        _NextToSpawnIndex = 0;
                    }
                    DisableAbilityTruck();

                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[2], GameObject.FindWithTag("DonutTruck"));
                }
            }
        }
    }
}

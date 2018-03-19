using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_RoadBlockSpawner : MonoBehaviour
{

    public GameObject RoadBlockPreFab;
    public Transform SpawnLocation;
    private GameObject[] _RoadBlockArr;
    public bool Spawn = false;
    private int _NextToSpawnIndex = 0;
    private int _MaxRoadBlocks = 10;
    private bool _EnableRoadBlock = false;

    public GameObject audioFiles;
    public void EnableRoadBlock(bool b)
    {
        _EnableRoadBlock = b;
    }
    void Start()
    {
        if (RoadBlockPreFab != null)
        {
            _RoadBlockArr = new GameObject[_MaxRoadBlocks];
            for (int i = 0; i < _MaxRoadBlocks; i++)
            {
                _RoadBlockArr[i] = Instantiate(RoadBlockPreFab);
                _RoadBlockArr[i].SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (_EnableRoadBlock)
        {
            audioFiles.GetComponent<SCR_AudioManager>().PlaySound(audioFiles.GetComponent<SCR_AudioManager>().GetSoundEffects()[1], gameObject);
            if (SCR_ButtonMaster.Player1 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(true)) > 0)
                {
                    if (RoadBlockPreFab != null)
                    {
                        _RoadBlockArr[_NextToSpawnIndex].SetActive(true);
                        _RoadBlockArr[_NextToSpawnIndex].transform.position = SpawnLocation.transform.position;
                        _RoadBlockArr[_NextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Quaternion rot = SpawnLocation.parent.transform.rotation;

                        _RoadBlockArr[_NextToSpawnIndex].transform.rotation = rot;
                        _RoadBlockArr[_NextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));

                        _NextToSpawnIndex++;
                        if (_NextToSpawnIndex == _MaxRoadBlocks)
                        {
                            _NextToSpawnIndex = 0;
                        }
                    }
                    _EnableRoadBlock = false;
                    SCR_AbilityManager.PoliceAbilityActive = true;
                }

            }
            else if (SCR_ButtonMaster.Player2 == "Police")
            {
                if (Input.GetAxisRaw(SCR_ButtonMaster.Master.Player2Ability) > 0)
                {

                    if (RoadBlockPreFab != null)
                    {
                        _RoadBlockArr[_NextToSpawnIndex].SetActive(true);
                        _RoadBlockArr[_NextToSpawnIndex].transform.position = SpawnLocation.transform.position;
                        _RoadBlockArr[_NextToSpawnIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Quaternion rot = SpawnLocation.parent.transform.rotation;

                        _RoadBlockArr[_NextToSpawnIndex].transform.rotation = rot;
                        _RoadBlockArr[_NextToSpawnIndex].transform.Rotate(new Vector3(0, 90, 0));

                        _NextToSpawnIndex++;
                        if (_NextToSpawnIndex == _MaxRoadBlocks)
                        {
                            _NextToSpawnIndex = 0;
                        }
                    }
                    _EnableRoadBlock = false;
                    SCR_AbilityManager.PoliceAbilityActive = true;
                }

            }
        }
    }
}
        


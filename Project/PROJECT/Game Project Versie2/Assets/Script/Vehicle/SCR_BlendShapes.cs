using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_BlendShapes: MonoBehaviour {

    // Use this for initialization
    [Range(0.0f,100.0f)]
    public float Blendeight = 0;
    private float _PreviousBlend=0;
    private SCR_TruckDestructionManager _DestructionManager;
    void Start()
    {
        _DestructionManager = GameObject.FindGameObjectWithTag("DonutTruck").GetComponent<SCR_TruckDestructionManager>();
    }
    void Update () {
        float max = _DestructionManager.GetMaxHealth();
        float health = _DestructionManager.GetDamage();
        float Blendeight = 100-(health / max) * 100;

        if (_PreviousBlend!=Blendeight)
        {
            _PreviousBlend = Blendeight;


            GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,Blendeight);
        }
	}
}

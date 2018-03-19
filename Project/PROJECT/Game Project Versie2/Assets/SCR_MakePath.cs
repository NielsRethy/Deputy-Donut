using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MakePath : MonoBehaviour {

    private List<Transform> _nodes = new List<Transform>();

    private void Awake()
    {
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();  // GetComponentsInChildren also gets component in parent object (this object)
        _nodes = new List<Transform>();
        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)
            {
                _nodes.Add(pathTransforms[i]);
            }
        }
    }
	
    private void OnDrawGizmos()
    {
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();  // GetComponentsInChildren also gets component in parent object (this object)
        _nodes = new List<Transform>();
        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)
            {
                _nodes.Add(pathTransforms[i]);
            }
        }
    }


    public List<Transform> GetPath()
    {
        return _nodes;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class SCR_Path : MonoBehaviour {

    public Color LineColor;
    private List<Transform> _nodes = new List<Transform>();

    // Function will be called in the editor while object with this script is selected
    private void OnDrawGizmos()     // OnDrawGizmosSelected 
    {
        Gizmos.color = LineColor;

        Transform[] pathTransforms = GetComponentsInChildren<Transform>();  // GetComponentsInChildren also gets component in parent object (this object)
        _nodes = new List<Transform>();
        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)
            {
                _nodes.Add(pathTransforms[i]);
            }
        }

        for (int i = 0; i < _nodes.Count; i++)
        {
            Vector3 currentNode = _nodes[i].position;
            Vector3 previousNode =  Vector3.zero;

            if (i > 0)
            {
                previousNode = _nodes[i - 1].position;
            }
            else if (i == 0 && _nodes.Count > 1)
            {
                previousNode = _nodes[_nodes.Count - 1].position;
            }

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
           // Handles.Label(currentNode, "Node " + i);
        }

    }

    public List<Transform> GetNodesList()
    {
        if (_nodes.Count == 0)
        {
            FillNodeList();
        }

        return _nodes;
    }

    private void FillNodeList()
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
}

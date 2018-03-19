using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class SCR_AISteering : MonoBehaviour {

    public GameObject path;
    public float MaxSteerAngle = 30.0f;
    public float MaxMotorTorque = 100.0f;
    public float MaxSpeed = 200.0f;
    public float CloseToNodeSpeed = 50.0f;
    public float WheelRotationTime = 1.0f;  // time it takes to adjust steer angle

    public List<WheelCollider> WheelColliders = new List<WheelCollider>();
    public List<GameObject> WheelMeshes = new List<GameObject>();
    private List<Transform> _nodes = new List<Transform>();
    private int _currentNode = 0;
    private float _targetWheelRotation;
    private Rigidbody _rb;

    private float _currentRotation;
    private float _rotationAdjustment;


    [Header("Gizmo debug")]
    public float _debug_WheelAngle = 0;    // May be remove for final build.

    // Use this for initialization
    void Start()
    {
        // Get Path
        _nodes = path.GetComponent<SCR_Path>().GetNodesList();

        // Cent Center of mass
        _rb = GetComponent<Rigidbody>();
        Vector3 centerOfMass = transform.Find("CenterOfMass").localPosition;
        _rb.centerOfMass = centerOfMass;

        //Determine starting node
        _currentNode = GetClosestNodeIdx();

        // Set start rotation
        transform.Rotate(_nodes[_currentNode].position - transform.position);
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        CheckNodeDistance();
        ApplySteer();
        Drive();
        RotateWheelMeshes();
    }

    private void ApplySteer()
    {
        // On which side is a point relative to a vector
        // source: https://stackoverflow.com/questions/1560492/how-to-tell-whether-a-point-is-to-the-right-or-left-side-of-a-line
        Vector3 targetNodePos = _nodes[_currentNode].position;
        Vector3 relativeVector = targetNodePos - transform.position;
        float direction = PointIsLeftOfVector(transform.position, transform.forward + transform.position, targetNodePos);
       
        float targetAngle = Vector3.Angle(transform.forward, relativeVector);
        float currentMaxSteerAngle = 0;
        if (targetAngle > 40.0f)
        {
            currentMaxSteerAngle = MaxSteerAngle * 1.5f;
        }
        else currentMaxSteerAngle = MaxSteerAngle;

        for (int i = 0; i < WheelColliders.Count - 2; i++)
        {
            WheelColliders[i].steerAngle = Mathf.Clamp(targetAngle * (-direction), -currentMaxSteerAngle, currentMaxSteerAngle);
        }
        _debug_WheelAngle = WheelColliders[0].steerAngle;
    }

    private void Drive()
    {
        // Slow down if getting closer to waypoint -> better turning
        float currentMaxSpeed;
        float slowDownDistance = 20.0f;
        float distanceToNextNode = (transform.position - _nodes[_currentNode].position).magnitude;
        float slowDownAngle = 40.0f;
        float angleDifference = Vector3.Angle(transform.forward, _nodes[_currentNode].position - transform.position);

        Debug.Log("Angle diff: " + angleDifference);
        // if close to next node -> lower max speed (avoid harsh turns)
        if (distanceToNextNode < slowDownDistance || slowDownAngle < angleDifference )
            currentMaxSpeed = CloseToNodeSpeed;
        else
            currentMaxSpeed = MaxSpeed;

        float torque = 0;
        float brakeTorque = 0;

        // Clamp to max speed
        // float currentSpeed = 2 * Mathf.PI * WheelColliders[0].radius * WheelColliders[0].rpm * 60 / 1000;
        float currentSpeed = _rb.velocity.magnitude;
        if (currentSpeed > currentMaxSpeed)
        {
            torque = 0;
            brakeTorque = (currentSpeed - currentMaxSpeed) * 40;
        }
        else
        {
            torque = MaxMotorTorque;
            brakeTorque = 0;
        }

        Debug.Log("CurrentMaxSpeed: " + currentMaxSpeed);

        for (int i = 0; i < WheelColliders.Count; i++)
        {
            WheelColliders[i].motorTorque = torque;
            WheelColliders[i].brakeTorque = brakeTorque;
        }
    }

    private void CheckNodeDistance()
    {
        float distanceToCurrentNode = Vector3.Distance(transform.position, _nodes[_currentNode].position);
        float distanceTrigger = 4f;

        if (distanceToCurrentNode < distanceTrigger)
        {
            if (_currentNode == _nodes.Count - 1)
            {
                _currentNode = 0;
            }
            else
            {
                ++_currentNode;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_currentNode < _nodes.Count)
        {
            Gizmos.DrawLine(transform.position, _nodes[_currentNode].position);
            Gizmos.DrawSphere(_nodes[_currentNode].position, 1.0f);
        
        
            Vector3 textPosition = transform.position + Vector3.right * 2;
            string labelString = "Wheel Anlge: " + _debug_WheelAngle + "\n";
            labelString += "Target Node: " + _currentNode + "\n";
            float currentSpeed = _rb.velocity.magnitude; // current speed;
            labelString += "Speed: " + currentSpeed + "\n";
        
            //Handles.Label(textPosition, labelString);
        
            // rotation adjustment
            //Handles.Label(transform.position, "current rotation: " + _currentRotation + "\nRotationAdjustment: " + _rotationAdjustment);
        }
    }

    private float PointIsLeftOfVector (Vector3 a, Vector3 b, Vector3 t)      // a = own position; b = forward vector world space; t = target position
    {
        
        return Mathf.Clamp(((b.x - a.x) * (t.z - a.z) - (b.z - a.z) * (t.x - a.x)),-1,1);
    }

    private int GetClosestNodeIdx()
    {
        int closestNodeIdx = 0;
        float shortestDistance = 1000000;
        for (int i = 0; i < _nodes.Count; i++)
        {
            float distance = (transform.position - _nodes[i].position).magnitude;
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestNodeIdx = i;
            } 
        }
        return closestNodeIdx;
    }

    private void RotateWheelMeshes()
    {
        for (int i = 0; i < WheelMeshes.Count; i++)
        {
            GameObject currentWheel = WheelMeshes[i];
            Vector3 wheelPos = new Vector3();
            Quaternion wheelRotation = new Quaternion();

            WheelColliders[i].GetWorldPose(out wheelPos, out wheelRotation);
            currentWheel.transform.position = wheelPos;
            currentWheel.transform.rotation = wheelRotation;
        }

    }
}

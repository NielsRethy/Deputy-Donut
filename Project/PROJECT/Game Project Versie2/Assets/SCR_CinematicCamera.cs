using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CinematicCamera : MonoBehaviour {

    //public List<CameraNode> NodeQueue = new List<CameraNode>();
    public List<Transform> CameraPosition = new List<Transform>();
    public List<Transform> LookAtPosition = new List<Transform>();
    public List<float> TimeToNextNode = new List<float>();
 
    private int _nodeIndex = 0;
    private bool _isLerping = false;
    private float _timeStartedLerping;
    private Vector3 _startCameraPosition;
    private Vector3 _startLookAtPostion;
    private Vector3 _endCameraPosition;
    private Vector3 _endLookAtPosition;

    [Header("Slow motion")]
    public float TimeScale = 1f;
    
	// Use this for initialization
	void Start () {
        StartLerp();
        Time.timeScale = TimeScale;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Debug.Log("Index: " + _nodeIndex);


        // trigger lerping
        if (_nodeIndex == CameraPosition.Count - 1 )
        {
            _isLerping = false;
            Debug.Log("Done lerping");
        }


        if (_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageCompleted = timeSinceStarted / (TimeToNextNode[_nodeIndex]* (1/TimeScale));

            transform.position = Vector3.Lerp(_startCameraPosition, _endCameraPosition, percentageCompleted);
            Transform lookAtTransform = LookAtPosition[_nodeIndex];
            lookAtTransform.position = Vector3.Lerp(_startLookAtPostion, _endLookAtPosition, percentageCompleted);
            transform.LookAt(lookAtTransform);



            if (percentageCompleted >= 1)
            {
                _isLerping = false; 
                if (CameraPosition.Count - 2 > _nodeIndex)
                {
                    ++_nodeIndex;
                    Debug.Log("Index incremented from " + (_nodeIndex-1) + " to " + _nodeIndex);
                    StartLerp();
                }
            }
        }
	}

    private void StartLerp()
    {
        _isLerping = true;
        _timeStartedLerping = Time.time;

        _startCameraPosition = CameraPosition[_nodeIndex].position;
        _endCameraPosition = CameraPosition[_nodeIndex + 1].position;

        _startLookAtPostion = LookAtPosition[_nodeIndex].position;
        _endLookAtPosition = LookAtPosition[_nodeIndex + 1].position;
    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i < CameraPosition.Count; i++)
        {
            //draw target gizmo
           // Camera cam = GetComponent<Camera>();
            float cubeSize = 0.2f;
            Vector3 cube = new Vector3(cubeSize, cubeSize, cubeSize);
           // Gizmos.DrawFrustum(LookAtPosition[i].position, cam.fieldOfView, 5, 10,1.777f);
            Gizmos.DrawCube(LookAtPosition[i].position, cube);
            Gizmos.DrawSphere(CameraPosition[i].position, 0.4f);
            Gizmos.DrawLine(CameraPosition[i].position, LookAtPosition[i].position);
        }
        
    }

}

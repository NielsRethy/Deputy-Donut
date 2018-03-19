using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CameraFollow : MonoBehaviour {


    // Use this for initialization
    public GameObject Car;         
    public float Distance = 6.4f;
    public float Height = 1.4f;
    public float RotationSpeedTurn = 3.0f;
    public float HeightSpeedTurn = 2.0f;
    public float ZoomRatio = 1f;
    public float DefaultFOV = 60.0f;
    public float Afstand = 2.0f;
    private Vector3 _rotationVector;
    private Rigidbody _rb;
    private Camera _camera;

    public float CameraRotSpeed = 2f;

    private float _currentX = 0.5f;
    private float _currentY = 0.5f;
    private bool _mouseUp = false;

    private const float Y_ANGLE_MIN = -30f;
    private const float Y_ANGLE_MAX = 20f;
    private bool _player1 = false;
    private bool _buttonDown = false;

    private bool _doOnce = false;

    private bool _lookAtTruck;
    void Start()
    {
        _rb = Car.GetComponent<Rigidbody>();
        _camera = GetComponent<Camera>();

        if (Car.GetComponent<SRC_CarControllerInput>().Police)
        {
            if (SCR_ButtonMaster.Player1 == "Police")
            {
                _player1 = true;
            }
            else if (SCR_ButtonMaster.Player2 == "Police")
            {
                _player1 = false;
            }
        }
        else
        {
            if (SCR_ButtonMaster.Player1 == "Truck")
            {
                _player1 = true;
            }
            else if (SCR_ButtonMaster.Player2 == "Truck")
            {
                _player1 = false;
            }
        }

    }
    void Update()
    {


      //  if (Input.GetMouseButtonDown(0))
       // {
      //      _currentX = _rotationVector.y;
       //     _currentY = _rotationVector.x;
       // }
       // if (Mathf.Abs(Input.GetAxisRaw("Fire1")) > 0)
      //  {
        //    _mouseUp = true;
       //     Cursor.visible = false;
       //     _currentX += Input.GetAxis("Mouse X") * CameraRotSpeed;
        //    _currentY -= Input.GetAxis("Mouse Y") * CameraRotSpeed;

        //    Cursor.lockState = CursorLockMode.Locked;
      //  }
      //  else
      //  {
      //      Cursor.lockState = CursorLockMode.None;
      //      Cursor.visible = true;

     //       _mouseUp = false;
     //   }

       // _currentY = Mathf.Clamp(_currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    void LateUpdate()
    {
        if (!_lookAtTruck)
        {
            float wantedAngle = _rotationVector.y;
            float wantedHeight = Car.transform.position.y + Height;
            float myAngle = transform.eulerAngles.y;
            float myHeight = transform.position.y;
            myAngle = Mathf.LerpAngle(myAngle, wantedAngle, RotationSpeedTurn * Time.deltaTime);
            myHeight = Mathf.Lerp(myHeight, wantedHeight, HeightSpeedTurn * Time.deltaTime);
            Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);
            transform.position = Car.transform.position;
            transform.position -= currentRotation * Vector3.forward * Distance;
            transform.position = new Vector3(transform.position.x, myHeight, transform.position.z);
            var newPos = Car.transform.position + (Car.transform.forward * Afstand);
        }
        else
        {
            float wantedAngle = _rotationVector.y;
            float wantedHeight = Car.transform.position.y + Height;
            float myAngle = transform.eulerAngles.y;
            float myHeight = transform.position.y;
            myHeight = Mathf.Lerp(myHeight, wantedHeight, HeightSpeedTurn * Time.deltaTime);
            Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);
            transform.position = Car.transform.position;
            transform.position -= currentRotation * Vector3.forward * Distance;
            transform.position = new Vector3(transform.position.x, myHeight, transform.position.z);
            var newPos = Car.transform.position + (Car.transform.forward * Afstand);
        }

        if (_player1)
        {
            if (Input.GetButtonDown(SCR_ButtonMaster.Master.Player1CameraTurnToPlayer))
            {
                _lookAtTruck = !_lookAtTruck;
            }
        }
        else
        {
            if (Input.GetButtonDown(SCR_ButtonMaster.Master.Player2CameraTurnToPlayer))
            {
                _lookAtTruck = !_lookAtTruck;
            }
        }
        

        if (_lookAtTruck)
        {
            if ((SCR_ButtonMaster.Player1 == "Police" && _player1))
            {
                transform.LookAt(GameObject.FindWithTag("DonutTruck").transform);
                _rotationVector.y = GameObject.FindWithTag("DonutTruck").transform.transform.eulerAngles.y + 180;
            }
            else if(SCR_ButtonMaster.Player1 == "Police" && !_player1)
            {
                if (GameObject.FindWithTag("AbilityManager").GetComponent<SCR_CaptureTheFlagg>().FlaggActive)
                {
                    transform.LookAt(GameObject.FindWithTag("PoliceCar").transform);
                    _rotationVector.y = GameObject.FindWithTag("PoliceCar").transform.transform.eulerAngles.y + 180;
                }
                else
                {
                    transform.LookAt(GameObject.FindWithTag("Flagg").transform);
                    _rotationVector.y = GameObject.FindWithTag("Flagg").transform.transform.eulerAngles.y + 180;
                }
            }
        }
        else
        {
            transform.LookAt(Car.transform);
        }

    }

    
    void FixedUpdate()
    {
        if (!_lookAtTruck)
        {
            if (!_mouseUp)
            {
                Vector3 localVilocity = Car.transform.InverseTransformDirection(_rb.velocity);
                if (_player1)
                {
                    if (Input.GetButton(SCR_ButtonMaster.Master.Player1CameraFlip))
                    {
                        _rotationVector.y = Car.transform.eulerAngles.y + 180;
                        _buttonDown = true;
                    }
                    else
                    {
                        _buttonDown = false;
                    }
                    if (localVilocity.z < -1.5f && !_buttonDown)
                    {
                        _rotationVector.y = Car.transform.eulerAngles.y + 180;
                    }
                    else if (!_buttonDown)
                    {
                        _rotationVector.y = Car.transform.eulerAngles.y;
                    }
                }
                else
                {
                    if (Input.GetButton(SCR_ButtonMaster.Master.Player2CameraFlip))
                    {
                        _rotationVector.y = Car.transform.eulerAngles.y + 180;
                        _buttonDown = true;
                    }
                    else
                    {
                        _buttonDown = false;
                    }
                    if (localVilocity.z < -1.5f && !_buttonDown)
                    {
                        _rotationVector.y = Car.transform.eulerAngles.y + 180;
                    }
                    else if (!_buttonDown)
                    {
                        _rotationVector.y = Car.transform.eulerAngles.y;
                    }

                }

                float acc = _rb.velocity.magnitude;
                float fov = DefaultFOV + acc * ZoomRatio;
                fov = Mathf.Clamp(fov, DefaultFOV, 90);
                _camera.fieldOfView = fov;
            }
            else
            {
                var rotation = Quaternion.Euler(_currentY, _currentX, 0);
                var position = rotation * new Vector3(0.0f, Height, -Distance) + Car.transform.position;

                _rotationVector = new Vector3(_currentY, _currentX, 0);
                transform.rotation = rotation;
                transform.position = position;
            }
        }


    }

   
}
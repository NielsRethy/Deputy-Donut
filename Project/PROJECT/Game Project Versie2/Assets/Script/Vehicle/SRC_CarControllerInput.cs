using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SCR_Vehicle))]

public class SRC_CarControllerInput : MonoBehaviour
{
    private SCR_Vehicle m_Car; // the car controller we want to use
    private GameObject m_HUD_Reference;

    private bool _player1 = false;
    public bool Police = false;
  
    public string BrakeInput;
    private bool _disabled = false;

    private float _time = 0.0f;

    public float TimeToFlipBetweenFlip = 3.0f;

    //public AudioSource CarEngineSound;
    //public AudioClip CarEnginClip;
    public void SetDisabled(bool toSet)
    {
        _disabled = toSet;
    }
    private void Awake()
    {
        // get the car controller
        m_Car = GetComponent<SCR_Vehicle>();
        m_HUD_Reference = GameObject.FindGameObjectWithTag("HUD");
    }

    void Start()
    {
        if (Police)
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

       // CarEngineSound.clip = CarEnginClip;
        //CarEngineSound.loop = true;
        //CarEngineSound.volume = 0.02f;
       // CarEngineSound.Play();

    }

    private void FixedUpdate()
    {
        if (_player1)
        {
            // pass the input to the car!
            float h = 0;
            if (Input.GetAxis(SCR_ButtonMaster.Master.Player1HorizontalAxesController) != 0)
            {
                h = Input.GetAxis(SCR_ButtonMaster.Master.Player1HorizontalAxesController);
            }
            else
            {
                h = Input.GetAxis(SCR_ButtonMaster.Master.Player1HorizontalAxesKeyboard);
            }

            float v = 0;
            if (Input.GetAxis(SCR_ButtonMaster.Master.Player1VerticalAxesControllerForward) != 0)
            {
                v = Input.GetAxis(SCR_ButtonMaster.Master.Player1VerticalAxesControllerForward);
            }
            else if (Input.GetAxis(SCR_ButtonMaster.Master.Player1VerticalAxesControllerBackward) != 0)
            {
                v = Input.GetAxis(SCR_ButtonMaster.Master.Player1VerticalAxesControllerBackward);
            }
            else
            {
                v = Input.GetAxis(SCR_ButtonMaster.Master.Player1VerticalAxesKeyboard);
            }
            float handbrake = Input.GetAxis(SCR_ButtonMaster.Master.Player1Brake);

            if (m_HUD_Reference.GetComponent<SCR_Timer>().getTimer() && !_disabled)
            {
                if (v < 0.0f)
                {
                    
                    if (m_Car.GetComponentInChildren<WheelCollider>().rpm > 1.0f)
                    {
                        MoveCar(h, v, v, 1.0f);
                      
                    }
                    else
                    {
                        MoveCar(h, v, v, 0.0f);
                        
                    }
                }
                else
                {
                    MoveCar(h, v, v, handbrake);
                    
                }
              //  CarEngineSound.volume = v / 20f;
//
              //  if (CarEngineSound.volume < 0.02f)
              //  {
             //       CarEngineSound.volume = 0.02f;
              //  }

            
            }
    


        }
        else
        {
            // pass the input to the car!
            float h = 0;
            if (Input.GetAxis(SCR_ButtonMaster.Master.Player2HorizontalAxesController) != 0)
            {
                h = Input.GetAxis(SCR_ButtonMaster.Master.Player2HorizontalAxesController);
            }
            else
            {
                h = Input.GetAxis(SCR_ButtonMaster.Master.Player2HorizontalAxesKeyboard);
            }

            float v = 0;
            if (Input.GetAxis(SCR_ButtonMaster.Master.Player2VerticalAxesControllerForward) != 0)
            {
                v = Input.GetAxis(SCR_ButtonMaster.Master.Player2VerticalAxesControllerForward);
            }
            else if (Input.GetAxis(SCR_ButtonMaster.Master.Player2VerticalAxesControllerBackward) != 0)
            {
                v = Input.GetAxis(SCR_ButtonMaster.Master.Player2VerticalAxesControllerBackward);
            }
            else
            {
                v = Input.GetAxis(SCR_ButtonMaster.Master.Player2VerticalAxesKeyboard);
            }
            float handbrake = Input.GetAxis(SCR_ButtonMaster.Master.Player2Brake);

            if (m_HUD_Reference.GetComponent<SCR_Timer>().getTimer() && !_disabled)
            {
                if (v < 0.0f)
                {

                    if (m_Car.GetComponentInChildren<WheelCollider>().rpm > 1.0f)
                    {
                        MoveCar(h, v, v, 1.0f);
                        
                    }
                    else
                    {
                        MoveCar(h, v, v, 0.0f);
                        
                    }
                }
                else
                {
                    MoveCar(h, v, v, handbrake);
                    
                }

              //  CarEngineSound.volume = v / 20f;
              //
               // if (CarEngineSound.volume < 0.02f)
              //  {
              //      CarEngineSound.volume = 0.02f;
              //  }

            }

        }

        if (_TurnCar)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,new Quaternion(0.0f, transform.rotation.y, 0.0f,0.0f), Time.deltaTime * 10 );

            if ((int)transform.rotation.eulerAngles.x == 0 && (int)transform.rotation.eulerAngles.z == 0)
            {
                _TurnCar = false;
            }
        }
    }

    private bool _TurnCar;
    void MoveCar(float h, float v, float s, float handbrake)
    {

        int vel = (int)GetComponent<Rigidbody>().velocity.magnitude;
        if (vel == 0 && _time >= 3.0f && (((int)Mathf.Abs(transform.rotation.eulerAngles.x % 360) >= 30 && (int)Mathf.Abs(transform.rotation.eulerAngles.x % 360) <= 330) || ((int)Mathf.Abs(transform.rotation.eulerAngles.z % 360) >= 30 && (int)Mathf.Abs(transform.rotation.eulerAngles.z % 360) <= 330)))
        {
            GetComponent<Rigidbody>().AddForce(0, GetComponent<Rigidbody>().mass * 10,0,ForceMode.Impulse);
            _TurnCar = true;

            _time = 0.0f;
        }
        else if (vel != 0)
        {
            
                _time = 0.0f;
            
        }
        _time += Time.deltaTime;

        m_Car.Move(h, v, s, handbrake);
       


    }
}


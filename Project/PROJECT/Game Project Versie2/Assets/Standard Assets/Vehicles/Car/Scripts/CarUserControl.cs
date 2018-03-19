using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        public GameObject HUD;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = 0;
            if (Input.GetAxis("PoliceHorizontalControllerTest") != 0)
            {
               h = Input.GetAxis("PoliceHorizontalControllerTest");
            }
            else
            {
                h = Input.GetAxis("PoliceHorizontal");
            }
            
            float v = Input.GetAxis("PoliceVerticalControllerTest");
#if !MOBILE_INPUT
            float handbrake = Input.GetAxis("Jump");

            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}

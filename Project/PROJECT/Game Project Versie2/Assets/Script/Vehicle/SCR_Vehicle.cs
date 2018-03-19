using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    internal enum CarDriveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }

    internal enum SpeedType
    {
        MPH,
        KPH
    }

public class SCR_Vehicle : MonoBehaviour
{
    [SerializeField] private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
    [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
    private WheelCollider[] m_WheelCollidersOriginal = new WheelCollider[4];
    [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
    [SerializeField] private SRC_WheelEffects[] m_WheelEffects = new SRC_WheelEffects[4];
    [SerializeField] private Vector3 m_CentreOfMassOffset;
    [SerializeField] private float m_MaximumSteerAngle;
    private float m_MaximumSteerAngleOriginal;

    [Range(0, 1)] [SerializeField] private float m_SteerHelper;
        // 0 is raw physics , 1 the car will grip in the direction it is facing

    [Range(0, 1)] [SerializeField] private float m_TractionControl; // 0 is no traction control, 1 is full interference
    [SerializeField] private float m_FullTorqueOverAllWheels;
    [SerializeField] private float m_ReverseTorque;
    [SerializeField] private float m_MaxHandbrakeTorque;
    [SerializeField] private float m_Downforce = 100f;
    [SerializeField] private SpeedType m_SpeedType;
    [SerializeField] private float m_Topspeed = 200;
    [SerializeField] private static int NoOfGears = 5;
    [SerializeField] private float m_RevRangeBoundary = 1f;
    [SerializeField] private float m_SlipLimit;
    [SerializeField] private float m_BrakeTorque;

    [Header("Acceleration help")]
    [SerializeField] private float _accelForce = 35.0f;        // Push force
    [SerializeField] private float _accelUntilVel = 15.0f;    // Apply force until m_AccelUntilVel is reached ( if(velocity.Forward >= m_AccelUntilVel) -> 0 force applied)
    [SerializeField] private float _accelEaseOut = 5.0f;   // in last 5 velocity.z, m_AccelForce will decrease linearly 

    private Quaternion[] m_WheelMeshLocalRotations;
    private Vector3 m_Prevpos, m_Pos;
    private float m_SteerAngle;
    private int m_GearNum;
    private float m_GearFactor;
    private float m_OldRotation;
    private float m_CurrentTorque;
    private Rigidbody m_Rigidbody;
    private const float k_ReversingThreshold = 0.01f;

    private GameObject _trailSlip;
    public Transform LeftTrailPos;
    public Transform RightTrailPos;



    public bool Skidding { get; private set; }
    public float BrakeInput { get; private set; }

    public float CurrentSteerAngle
    {
        get { return m_SteerAngle; }
    }

    public float CurrentSpeed
    {
        get { return m_Rigidbody.velocity.magnitude * 2.23693629f; }
    }

    public float MaxSpeed
    {
        get { return m_Topspeed; }
    }

    public float Revs { get; private set; }
    public float AccelInput { get; private set; }
    private float timeEnds =  5;
    private float CurrenTime = 0;


  
public bool Slip
    {
        get { return _slip; }
        set
        {
            _slip = value;
            CurrenTime = 0;
        }
    }

    public Vector3 MCentreOfMassOffset
    {
        get { return m_CentreOfMassOffset; }
        set { m_CentreOfMassOffset = value; }
    }

    private bool _slip = false;


    // Use this for initialization
    private void Start()
    {
        m_WheelMeshLocalRotations = new Quaternion[4];
        for (int i = 0; i < 4; i++)
        {
            m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
        }
        m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;

        m_MaxHandbrakeTorque = float.MaxValue;

        m_Rigidbody = GetComponent<Rigidbody>();
        m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);

        m_WheelCollidersOriginal = m_WheelColliders;

        m_MaximumSteerAngleOriginal = m_MaximumSteerAngle;


        _trailSlip = GameObject.FindWithTag("TrailSlip");

        _trailSlip.GetComponent<TrailRenderer>().enabled = false;
    }


    private void GearChanging()
    {
        float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
        float upgearlimit = (1 / (float) NoOfGears) * (m_GearNum + 1);
        float downgearlimit = (1 / (float) NoOfGears) * m_GearNum;

        if (m_GearNum > 0 && f < downgearlimit)
        {
            m_GearNum--;
        }

        if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
        {
            m_GearNum++;
        }
    }


    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }


    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }


    private void CalculateGearFactor()
    {
        float f = (1 / (float) NoOfGears);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
        m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_GearNum / (float) NoOfGears;
        var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
    }


    public void Move(float steering, float accel, float footbrake, float handbrake)
    {

       
        Slippen();

        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            Quaternion quat;
            Vector3 position;
            m_WheelColliders[i].GetWorldPose(out position, out quat);
            m_WheelMeshes[i].transform.position = position;
            m_WheelMeshes[i].transform.rotation = quat; 
        }
        if (_slip)
        {
            foreach (var wheel in m_WheelColliders)
            {
                var swf = wheel.sidewaysFriction;
                var fwf = wheel.forwardFriction;
                swf.extremumSlip = 2.0f;
                fwf.extremumSlip = 2.0f;
                wheel.sidewaysFriction = swf;
                wheel.forwardFriction = fwf;
                CurrenTime += Time.deltaTime;
            }
        }
       

        if (CurrenTime > timeEnds)
        {
            _slip = false;
            foreach (var wheel in m_WheelColliders)
            {
                var swf = wheel.sidewaysFriction;
                var fwf = wheel.forwardFriction;
                swf.extremumSlip = 0.25f;
                fwf.extremumSlip = 0.4f;
                wheel.sidewaysFriction = swf;
                wheel.forwardFriction = fwf;
                CurrenTime += Time.deltaTime;       // ~ is dit niet 4* time.DeltaTime?
            }
        }

        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        handbrake = Mathf.Clamp(handbrake, 0, 1);

        //Set the steer on the front wheels.
        //Assuming that wheels 0 and 1 are the front wheels.
        m_SteerAngle = steering * m_MaximumSteerAngle;
        m_WheelColliders[0].steerAngle = m_SteerAngle;
        m_WheelColliders[1].steerAngle = m_SteerAngle;

        SteerHelper();
        ApplyDrive(accel, footbrake);
        CapSpeed();

        //Set the handbrake.
        //Assuming that wheels 2 and 3 are the rear wheels.
        if (handbrake > 0f)
        {
            var hbTorque = handbrake * m_MaxHandbrakeTorque;
            m_WheelColliders[0].brakeTorque = hbTorque;
            m_WheelColliders[1].brakeTorque = hbTorque;
            m_WheelColliders[2].brakeTorque = hbTorque;
            m_WheelColliders[3].brakeTorque = hbTorque;
        }
        else // my code
        {
            m_WheelColliders[0].brakeTorque = 0;
            m_WheelColliders[1].brakeTorque = 0;
            m_WheelColliders[2].brakeTorque = 0;
            m_WheelColliders[3].brakeTorque = 0;
        }


        CalculateRevs();
        GearChanging();

        AddDownForce();
        CheckForWheelSpin();
        TractionControl();

        HelpAcceleration(AccelInput,BrakeInput); // ~custom



    }

    private void Slippen()
    {
        if (gameObject.tag == "PoliceCar")
        {
                 
            if ((SCR_ButtonMaster.Player1 == "Police" && Input.GetButton(SCR_ButtonMaster.Master.Player1Drift)) ||
                (SCR_ButtonMaster.Player2 == "Police" && Input.GetButton(SCR_ButtonMaster.Master.Player2Drift)))
            {

                if (Physics.Raycast(transform.position, -Vector3.up, 0.2f))
                {
                    if (GetComponentInChildren<TrailRenderer>() == null)
                    {
                        var i = Instantiate(_trailSlip, LeftTrailPos.position, Quaternion.identity,
                       this.gameObject.transform);
                        i.GetComponent<TrailRenderer>().enabled = true;

                        var i2 = Instantiate(_trailSlip, RightTrailPos.position, Quaternion.identity,
                            this.gameObject.transform);
                        i2.GetComponent<TrailRenderer>().enabled = true;

                        Destroy(i.gameObject, 5.0f);
                        Destroy(i2.gameObject, 5.0f);
                    }
                    var swf = m_WheelColliders[0].sidewaysFriction;
                    swf.extremumSlip = 1.0f;
                    swf.stiffness = 1.0f;
                    m_WheelColliders[0].sidewaysFriction = swf;
                    m_WheelColliders[1].sidewaysFriction = swf;
                    m_WheelColliders[2].sidewaysFriction = swf;
                    m_WheelColliders[3].sidewaysFriction = swf;
                    m_MaximumSteerAngle = 30.0f;
                    m_SteerHelper = 0.0f;
                }
                else
                {
                    var swf = m_WheelColliders[0].sidewaysFriction;
                    swf.extremumSlip = 0.25f;
                    swf.stiffness = 0.6f;
                    m_WheelColliders[0].sidewaysFriction = swf;
                    m_WheelColliders[1].sidewaysFriction = swf;
                    m_WheelColliders[2].sidewaysFriction = swf;
                    m_WheelColliders[3].sidewaysFriction = swf;
                    m_MaximumSteerAngle = 15.0f;
                    m_SteerHelper = 1.0f;
                    foreach (var skid in GetComponentsInChildren<TrailRenderer>())
                    {
                        if (skid.tag == "TrailSlip")
                        {
                            skid.transform.parent = null;
                        }
                    }

                }


            }
            else
            {
                var swf = m_WheelColliders[0].sidewaysFriction;
                swf.extremumSlip = 0.25f;
                swf.stiffness = 0.6f;
                m_WheelColliders[0].sidewaysFriction = swf;
                m_WheelColliders[1].sidewaysFriction = swf;
                m_WheelColliders[2].sidewaysFriction = swf;
                m_WheelColliders[3].sidewaysFriction = swf;
                m_MaximumSteerAngle = 15.0f;
                m_SteerHelper = 1.0f;
                foreach (var skid in GetComponentsInChildren<TrailRenderer>())
                {
                    if (skid.tag == "TrailSlip")
                    {
                        skid.transform.parent = null;
                    }
                }

            }
        }

        if (gameObject.tag == "DonutTruck")
        {

           
            if ((SCR_ButtonMaster.Player1 == "Truck" && Input.GetButton(SCR_ButtonMaster.Master.Player1Drift)) ||
                (SCR_ButtonMaster.Player2 == "Truck" && Input.GetButton(SCR_ButtonMaster.Master.Player2Drift)))
            {
                if (Physics.Raycast(transform.position, -Vector3.up, 1.1f))
                {
                    if (GetComponentInChildren<TrailRenderer>() == null)
                    {
                        var i = Instantiate(_trailSlip, LeftTrailPos.position, Quaternion.identity,
                       this.gameObject.transform);
                        i.GetComponent<TrailRenderer>().enabled = true;

                        var i2 = Instantiate(_trailSlip, RightTrailPos.position, Quaternion.identity,
                            this.gameObject.transform);
                        i2.GetComponent<TrailRenderer>().enabled = true;

                        Destroy(i.gameObject, 5.0f);
                        Destroy(i2.gameObject, 5.0f);
                    }

                    var swf = m_WheelColliders[0].sidewaysFriction;
                    swf.extremumSlip = 1.0f;
                    swf.stiffness = 1.0f;
                    m_WheelColliders[0].sidewaysFriction = swf;
                    m_WheelColliders[1].sidewaysFriction = swf;
                    m_WheelColliders[2].sidewaysFriction = swf;
                    m_WheelColliders[3].sidewaysFriction = swf;
                    m_MaximumSteerAngle = 30.0f;
                    m_SteerHelper = 0.0f;
                }
                else
                {
                    var swf = m_WheelColliders[0].sidewaysFriction;
                    swf.extremumSlip = 0.25f;
                    swf.stiffness = 0.6f;
                    m_WheelColliders[0].sidewaysFriction = swf;
                    m_WheelColliders[1].sidewaysFriction = swf;
                    m_WheelColliders[2].sidewaysFriction = swf;
                    m_WheelColliders[3].sidewaysFriction = swf;
                    m_MaximumSteerAngle = 20.0f;
                    m_SteerHelper = 1.0f;
                    foreach (var skid in GetComponentsInChildren<TrailRenderer>())
                    {
                        if (skid.tag == "TrailSlip")
                        {
                            skid.transform.parent = null;
                        }
                    }

                }


            }
            else
            {
                var swf = m_WheelColliders[0].sidewaysFriction;
                swf.extremumSlip = 0.25f;
                swf.stiffness = 0.6f;
                m_WheelColliders[0].sidewaysFriction = swf;
                m_WheelColliders[1].sidewaysFriction = swf;
                m_WheelColliders[2].sidewaysFriction = swf;
                m_WheelColliders[3].sidewaysFriction = swf;
                m_MaximumSteerAngle = 20.0f;
                m_SteerHelper = 1.0f;
                foreach (var skid in GetComponentsInChildren<TrailRenderer>())
                {
                    if (skid.tag == "TrailSlip")
                    {
                        skid.transform.parent = null;
                    }
                }

            }
        }
    }


    private void CapSpeed()
    {
        float speed = m_Rigidbody.velocity.magnitude;
        float slowDownFactor = 7f;
        switch (m_SpeedType)
        {
            case SpeedType.MPH:

                speed *= 2.23693629f;
                if (speed > m_Topspeed)
                    //m_Rigidbody.velocity = (m_Topspeed / 2.23693629f) * m_Rigidbody.velocity.normalized;  // Unity asset default
                    m_Rigidbody.velocity -= m_Rigidbody.velocity.normalized * slowDownFactor * Time.deltaTime; // Slows vehicle down over time. instead of hard capping the velocity
                break;

            case SpeedType.KPH:
                speed *= 3.6f;
                if (speed > m_Topspeed)
                    // m_Rigidbody.velocity = (m_Topspeed / 3.6f) * m_Rigidbody.velocity.normalized; // Unity asset default
                    m_Rigidbody.velocity -= m_Rigidbody.velocity.normalized * slowDownFactor * Time.deltaTime; // Slows vehicle down over time. instead of hard capping the velocity
                break;
        }
    }


    private void ApplyDrive(float accel, float footbrake)
    {


        //
        float thrustTorque;
        switch (m_CarDriveType)
        {
            case CarDriveType.FourWheelDrive:
                thrustTorque = accel * (m_CurrentTorque / 4f);
                for (int i = 0; i < 4; i++)
                {
                    m_WheelColliders[i].motorTorque = thrustTorque;
                }
                break;

            case CarDriveType.FrontWheelDrive:
                thrustTorque = accel * (m_CurrentTorque / 2f);
                m_WheelColliders[0].motorTorque = m_WheelColliders[1].motorTorque = thrustTorque;
                break;

            case CarDriveType.RearWheelDrive:
                thrustTorque = accel * (m_CurrentTorque / 2f);
                m_WheelColliders[2].motorTorque = m_WheelColliders[3].motorTorque = thrustTorque;
                break;

        }

        for (int i = 0; i < 4; i++)
        {
            if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 50f)
            {
                m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
            }
            else if (footbrake > 0)
            {
                m_WheelColliders[i].brakeTorque = 0f;
                m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
            }
        }
    }


    private void SteerHelper()
    {
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelhit;
            m_WheelColliders[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return; // wheels arent on the ground so dont realign the rigidbody velocity
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
        {
            //Debug.Log(m_OldRotation + ", " + transform.eulerAngles.y);
            var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
        }
        m_OldRotation = transform.eulerAngles.y;
    }


    // this is used to add more grip in relation to speed
    private void AddDownForce()
    {
        m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up * m_Downforce *
                                                       m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
    }


    // checks if the wheels are spinning and is so does three things
    // 1) emits particles
    // 2) plays tiure skidding sounds
    // 3) leaves skidmarks on the ground
    // these effects are controlled through the WheelEffects class
    private void CheckForWheelSpin()
    {
        // loop through all wheels
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            m_WheelColliders[i].GetGroundHit(out wheelHit);

            //// is the tire slipping above the given threshhold
            //if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit)
            //{
            //    m_WheelEffects[i].EmitTyreSmoke();

            //    // avoiding all four tires screeching at the same time
            //    // if they do it can lead to some strange audio artefacts
            //    if (!AnySkidSoundPlaying())
            //    {
            //        m_WheelEffects[i].PlayAudio();
            //    }
            //    continue;
            //}

            // if it wasnt slipping stop all the audio
            //if (m_WheelEffects[i].PlayingAudio)
            //{
            //    m_WheelEffects[i].StopAudio();
            //}
            //// end the trail generation
            //m_WheelEffects[i].EndSkidTrail();
        }
    }

    // crude traction control that reduces the power to wheel if the car is wheel spinning too much
    private void TractionControl()
    {
        WheelHit wheelHit;
        switch (m_CarDriveType)
        {
            case CarDriveType.FourWheelDrive:
                // loop through all wheels
                for (int i = 0; i < 4; i++)
                {
                    m_WheelColliders[i].GetGroundHit(out wheelHit);

                    AdjustTorque(wheelHit.forwardSlip);
                }
                break;

            case CarDriveType.RearWheelDrive:
                m_WheelColliders[2].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);

                m_WheelColliders[3].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;

            case CarDriveType.FrontWheelDrive:
                m_WheelColliders[0].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);

                m_WheelColliders[1].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;
        }
    }


    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
        {
            m_CurrentTorque -= 10 * m_TractionControl;
        }
        else
        {
            m_CurrentTorque += 10 * m_TractionControl;
            if (m_CurrentTorque > m_FullTorqueOverAllWheels)
            {
                m_CurrentTorque = m_FullTorqueOverAllWheels;
            }
        }
    }

    private void HelpAcceleration(float forward, float backward)
    {

        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelhit;
            m_WheelColliders[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return;
        }
  
        Vector3 localVel = transform.InverseTransformDirection(m_Rigidbody.velocity);
        //Debug.Log("x: " + localVel.x + "||" + "y: " + localVel.y + "||" + "z: " + localVel.z);
        if (forward > 0 && localVel.z < _accelUntilVel)
        {
            float accelForce = _accelForce - (_accelForce * Mathf.Clamp((localVel.z - _accelUntilVel) / _accelEaseOut, 0, 1));
            m_Rigidbody.velocity += transform.forward * accelForce * Time.deltaTime;
        }
        if (backward > 0 && localVel.z > -_accelUntilVel)
        {
            float accelForce = _accelForce - (_accelForce * Mathf.Clamp(((-localVel.z) - _accelUntilVel) / _accelEaseOut, 0, 1));
            m_Rigidbody.velocity += transform.forward * (-accelForce) * Time.deltaTime;
        }
    }


    private bool AnySkidSoundPlaying()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_WheelEffects[i].PlayingAudio)
            {
                return true;
            }
        }
        return false;
    }
}


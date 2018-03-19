using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_AbilityTimeBomb : SCR_AbilityBase
{
    // Source: https://unity3d.com/learn/tutorials/topics/scripting/enumerations
    enum TimeBombState      // enum pratices:    - ONLY this class needs enum -> write enum inside class
    {                       //                   - Other classes ALSO need acces to enum -> write outside class
        IDLE, ACTIVATED, DETONATED
    }

    [Header("Explosion Settings")]
    [SerializeField] private float _radius = 10.0f;     // [serializeField] private -> variable only needs to be visible in inspector, but not publicly accessible.
    [SerializeField] private float _strength = 20000.0f;
    [SerializeField] private float _truckStrengthMultiplier = 150.0f;
    [SerializeField] private float _maxDamage = 100.0f;

    [Header("Bomb settings")]
    [SerializeField] private float _countDownTime = 3.0f;
    [Tooltip("Bomb model that will be instantiated and put on vehicle.")]
    [SerializeField] private Vector3 _bombPosition = new Vector3(0, 1.5f, 0);

    [Header("Display settings")]
    [SerializeField] private int _fontSizeText = 180;
    [SerializeField] private int _fontSizeNumbers = 240;
    [SerializeField] private Color _startColor = Color.green;
    [SerializeField] private Color _activateColor = new Vector4 (0.0f, 0.28f,0.22f,1.0f);
    [SerializeField] private float _timeDisplayActivated = 0.5f;
    [SerializeField] private float _timeDisplayDetonating = 0.5f;
    [SerializeField] private float _flickerSpeed = 0.1f;
    private Text _displayText;

    private TimeBombState _timeBombState;
    private float _countDownTimer;
    private bool _bIscountingDown;

    private void Start()
    {
        EnableActivateParticles(false);

        GameObject textObject = VisualModel.transform.Find("Canvas").gameObject.transform.Find("Text").gameObject;
        _displayText = textObject.GetComponent<Text>();

        if (_displayText == null)
        {
            Debug.Log("_displayText Not found.");
        }

        SetVisualModelActive(false);

        GameObject.FindWithTag("PoliceCar").GetComponent<SCR_OnCollisionEvents>().onCollisionWithTruck += Explode;
    }

    private void Update()
    {
        if (_abilityIsActive)
        {
            switch (_timeBombState)
            {
                case TimeBombState.IDLE:    // Bomb is on vehicle, won't detonate before activated.
                    if (SCR_ButtonMaster.Player1 == "Police")
                    {
                        // Transition condition
                        if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility1(_abilityOne)) > 0)
                            // if PlayerAbilityButton is pressed
                        {
                            _displayText.fontSize = _fontSizeText;
                            StartCoroutine("StartCountDown");
                        }
                    }
                    if (SCR_ButtonMaster.Player2 == "Police")
                    {
                        // Transition condition
                        if (Input.GetAxisRaw(SCR_ButtonMaster.Master.AcitvateAbility2(_abilityOne)) > 0)
                        // if PlayerAbilityButton is pressed
                        {
                            _displayText.fontSize = _fontSizeText;
                            StartCoroutine("StartCountDown");
                        }
                    }


                    break;
                case TimeBombState.ACTIVATED:   // bomb is on vehicle, starts counting down and is triggerable by collision.

                    if (_bIscountingDown)
                    {
                        _countDownTimer -= Time.deltaTime;
                        _displayText.fontSize = _fontSizeNumbers;
                        UpdateDisplay(_countDownTimer.ToString("F2"));

                        if (_countDownTimer <= _timeDisplayDetonating)
                        {
                            _displayText.fontSize = _fontSizeText;
                            StartCoroutine("TransitionToDetonated");
                        }
                    }
    
                    // Explode is called on OnCollisionWithTruck

                    break;
                case TimeBombState.DETONATED:
                    DisableAbilityPolice();

                    break;
                default:
                    break;
            }      
        }
    }

    public override void OnPickUp( bool abilityOne)
    {
        // bomb placed on 
        _abilityIsActive = true;

        SetVisualModelActive(true);
        _timeBombState = TimeBombState.IDLE;

        UpdateDisplay("NOT ACTIVE");
        _countDownTimer = _countDownTime;

        _abilityOne = abilityOne;
        _bIscountingDown = false;

        _displayText.color = _startColor;
        _displayText.fontSize = _fontSizeText;
    }

    IEnumerator StartCountDown()
    {
        _bIscountingDown = false;

        _displayText.color = _activateColor;

        int flickerAmount = Mathf.FloorToInt(_timeDisplayActivated / _flickerSpeed / 2);

        for (int i = 0; i < flickerAmount; i++)
        {
            UpdateDisplay(">>ACTIVE<<");
            yield return new WaitForSeconds(_flickerSpeed);
            UpdateDisplay(" ");
            yield return new WaitForSeconds(_flickerSpeed);
        }

        _countDownTimer = _countDownTime;
        _timeBombState = TimeBombState.ACTIVATED;
        _bIscountingDown = true;
    }

    IEnumerator TransitionToDetonated()
    {

        int flickerAmount = Mathf.FloorToInt(_timeDisplayDetonating / _flickerSpeed / 2); 
        for (int i = 0; i < flickerAmount; i++)
        {
            UpdateDisplay(">>BOOM<<");
            yield return new WaitForSeconds(_flickerSpeed);
            UpdateDisplay(" ");
            yield return new WaitForSeconds(_flickerSpeed);
        }
        Explode();
    }

    private void UpdateDisplay(string toDisplay)
    {
        _displayText.text = toDisplay;
    }

    private void Explode()
    {
        if (_timeBombState != TimeBombState.ACTIVATED)
            return;

        _timeBombState = TimeBombState.DETONATED;

        EnableActivateParticles(true);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, _radius);
        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (rb.CompareTag("DonutTruck"))
                {
                    SCR_TruckDestructionManager truckDestrManager = col.GetComponent<SCR_TruckDestructionManager>();

                    // Calc damage
                    Vector3 truckPos = rb.position;
                    float distance = (explosionPos - truckPos).magnitude;
                    float damage = (distance / _radius) * _maxDamage;

                    truckDestrManager.TakeDamage(damage);

                    rb.AddExplosionForce(_strength * _truckStrengthMultiplier, explosionPos, _radius);
                }
                else if (!rb.CompareTag("PoliceCar"))
                {
                    rb.AddExplosionForce(_strength, explosionPos, _radius);
                    _SoundHolder.GetComponent<SCR_AudioManager>().PlaySound(_SoundHolder.GetComponent<SCR_AudioManager>().GetSoundEffects()[9], GameObject.FindWithTag("PoliceCar"));
                }
            }
        }
    }
}

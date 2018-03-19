using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SCR_AbilityManager : MonoBehaviour
{

    private static bool _policeAbilityActive;
    private static bool _truckAbilityActive;

    private static bool _policeAbilityActive2;
    private static bool _truckAbilityActive2;

    public int AddedNitroPerPickup = 25;

    private static SCR_AbilityBase _activeAbilityPolice;
    private static SCR_AbilityBase _activeAbilityTruck;

    private static SCR_AbilityBase _activeAbilityPolice2;
    private static SCR_AbilityBase _activeAbilityTruck2;

    private List<SCR_AbilityBase> _abilityListCop = new List<SCR_AbilityBase>();
    private List<SCR_AbilityBase> _abilityListTruck = new List<SCR_AbilityBase>();

    private KeyCode[] keyCodes = {
         KeyCode.Keypad0,
         KeyCode.Keypad1,
         KeyCode.Keypad2,
         KeyCode.Keypad3,
         KeyCode.Keypad4,
         KeyCode.Keypad5,
         KeyCode.Keypad6,
         KeyCode.Keypad7,
         KeyCode.Keypad8,
         KeyCode.Keypad9,
     };

    public static bool PoliceAbilityActive
    {
        get { return _policeAbilityActive; }
        set
        {
            _policeAbilityActive = value;
            GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
        }
    }

    public static bool TruckAbilityActive
    {
        get { return _truckAbilityActive; }
        set
        {
            _truckAbilityActive = value;
            GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
        }
    }

    public static SCR_AbilityBase ActiveAbilityPolice
    {
        get { return _activeAbilityPolice; }
        set { _activeAbilityPolice = value; }
    }

    public static SCR_AbilityBase ActiveAbilityTruck
    {
        get { return _activeAbilityTruck; }
        set { _activeAbilityTruck = value; }
    }

    public static bool PoliceAbilityActive2
    {
        get { return _policeAbilityActive2; }
        set
        {
            _policeAbilityActive2 = value;
            GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
        }
    }

    public static bool TruckAbilityActive2
    {
        get { return _truckAbilityActive2; }
        set
        {
            _truckAbilityActive2 = value;
            GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
        }
    }

    public static SCR_AbilityBase ActiveAbilityPolice2
    {
        get { return _activeAbilityPolice2; }
        set { _activeAbilityPolice2 = value; }
    }

    public static SCR_AbilityBase ActiveAbilityTruck2
    {
        get { return _activeAbilityTruck2; }
        set { _activeAbilityTruck2 = value; }
    }

    // Use this for initialization
    void Start()
    {
        List<SCR_AbilityBase> abilityListCop = new List<SCR_AbilityBase>();
        List < SCR_AbilityBase > abilityListTruck = new List<SCR_AbilityBase>();
        abilityListCop = GameObject.FindGameObjectWithTag("PoliceAbility").GetComponents<SCR_AbilityBase>().ToList();
        abilityListTruck = GameObject.FindGameObjectWithTag("TruckAbility").GetComponents<SCR_AbilityBase>().ToList();

        foreach (SCR_AbilityBase abilityBase in abilityListCop)
        {
            if (abilityBase.enabled == true)
            {
                _abilityListCop.Add(abilityBase);
            }
        }

        foreach (SCR_AbilityBase abilityBase in abilityListTruck)
        {
            if (abilityBase.enabled == true)
            {
                _abilityListTruck.Add(abilityBase);
            }
        }


        _policeAbilityActive = false;
        _truckAbilityActive = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (!_policeAbilityActive  || !_policeAbilityActive2)
        {
            foreach (var ac in _abilityListCop)
            {

                if (Input.GetKeyDown(keyCodes[ac.GodKey]))
                {
                    if (!_policeAbilityActive)
                    {
                        if (ActiveAbilityPolice2 != ac || !_policeAbilityActive2)
                        {
                            ac.OnPickUp(true);
                            _policeAbilityActive = true;
                            ActiveAbilityPolice = ac;
                            GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
                            SCR_NotificationQueue.NotificationPolice.DisplayText("GODMODE: Ability " + ac.AbilityName,3.0f,0.20f);
                        }
                    }
                    else if (_abilityListCop.Count > 1)
                    {

                        if (ActiveAbilityPolice != ac || !_policeAbilityActive)
                        {
                            ac.OnPickUp(false);
                            _policeAbilityActive2 = true;
                            ActiveAbilityPolice2 = ac;
                            GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
                            SCR_NotificationQueue.NotificationPolice.DisplayText("GODMODE: Ability " + ac.AbilityName, 3.0f, 0.20f);
                        }
                       
                    }
                }
            }

        }

        if (!_truckAbilityActive  || !_truckAbilityActive2 )
        {
            foreach (var at in _abilityListTruck)
            {
                if (Input.GetKeyDown(keyCodes[at.GodKey]))
                {
                    if (!_truckAbilityActive)
                    {
                        if (ActiveAbilityTruck2 != at || !_truckAbilityActive2)
                        {

                            at.OnPickUp(true);
                            _truckAbilityActive = true;
                            ActiveAbilityTruck = at;
                            GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
                            SCR_NotificationQueue.NotificationTruck.DisplayText("GODMODE: Ability " + at.AbilityName, 3.0f, 0.20f);
                        }

                    }
                    else if (_abilityListTruck.Count > 1)
                    {

                        if (ActiveAbilityPolice != at || !_truckAbilityActive)
                        {
                            at.OnPickUp(false);
                            _truckAbilityActive2 = true;
                            ActiveAbilityTruck2 = at;
                            GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
                            SCR_NotificationQueue.NotificationTruck.DisplayText("GODMODE: Ability " + at.AbilityName, 3.0f, 0.20f);
                        }
                    }

                }
            }
        }

    }
    public void ActivateAbility(bool police) // True = police false = truck
    {
        if (police)
        {
            if (!_policeAbilityActive)
            {
                //_abilityListCop = GameObject.FindGameObjectWithTag("PoliceAbility").GetComponents<SCR_AbilityBase>().ToList();
                Random.seed = (int)System.DateTime.Now.Ticks;
                int r = Random.Range(0, _abilityListCop.Count);

                if (_policeAbilityActive2)
                {
                    if (_abilityListCop.Count > 1)
                    {
                        do
                        {
                            r = Random.Range(0, _abilityListCop.Count);

                        } while (ActiveAbilityPolice2 == _abilityListCop[r]);
                    }
                }
               

                _abilityListCop[r].OnPickUp(true);
                _policeAbilityActive = true;
                ActiveAbilityPolice = _abilityListCop[r];
                GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();          
            }
            else if (_abilityListCop.Count > 1 )
            {
                Random.seed = (int)System.DateTime.Now.Ticks;
                int r = Random.Range(0, _abilityListCop.Count);
                do
                {
                    r = Random.Range(0, _abilityListCop.Count);
                   
                } while (ActiveAbilityPolice == _abilityListCop[r]);

                _abilityListCop[r].OnPickUp(false);
                _policeAbilityActive2 = true;
                ActiveAbilityPolice2 = _abilityListCop[r];
                GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
            }
            
        }
        else
        {
            if (!_truckAbilityActive)
            {
                //_abilityListTruck = GameObject.FindGameObjectWithTag("TruckAbility").GetComponents<SCR_AbilityBase>().ToList();
                Random.seed = (int)System.DateTime.Now.Ticks;


                var r = Random.Range(0, _abilityListTruck.Count);

                if (_truckAbilityActive2)
                {
                    if (_abilityListTruck.Count > 1)
                    {
                        do
                        {
                            r = Random.Range(0, _abilityListTruck.Count);

                        } while (ActiveAbilityTruck2 == _abilityListTruck[r]);
                    }
                }


                _abilityListTruck[r].OnPickUp(true);
                _truckAbilityActive = true;
                ActiveAbilityTruck = _abilityListTruck[r];
                GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();

            }
            else if (_abilityListTruck.Count > 1)
            {
               
                Random.seed = (int)System.DateTime.Now.Ticks;
                var r = Random.Range(0, _abilityListTruck.Count);
                do
                {
                    r = Random.Range(0, _abilityListTruck.Count);

                } while (_abilityListTruck[r] == ActiveAbilityTruck);
                _abilityListTruck[r].OnPickUp(false);
                _truckAbilityActive2 = true;
                ActiveAbilityTruck2 = _abilityListTruck[r];
                GameObject.FindWithTag("HUD").GetComponent<SCR_AbilityUI>().UpdatePickupUI();
            }
        }

    }
}
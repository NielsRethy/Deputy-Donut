using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AbilityBase : MonoBehaviour
{
    [Header("Base ablity settings")]
    public Sprite UiSprite;
    [SerializeField] protected GameObject VisualModel;
    [SerializeField] protected GameObject _AbilityIndicator;
     protected bool _abilityIsActive = false;
    [SerializeField] protected List<ParticleSystem> _OnActivatePs = new List<ParticleSystem>();
    [SerializeField] protected List<ParticleSystem> _IdlePs = new List<ParticleSystem>();
    [Space]
    [SerializeField] protected GameObject _SoundHolder;

 
    public int GodKey;

    public string AbilityName;

    private float _fadeDuration = 1.0f;

    void Update()
    {
    }
    void Start()
    {
        DisableAbilityPolice();
        DisableAbilityTruck();
    }
    public void Awake()
    {
        EnableActivateParticles(false);
        EnableIdleParticles(false);
        EnableIndicator(false);
    }

    public bool _abilityOne;

    public virtual void OnPickUp(bool abilityOne)
    {
        _abilityIsActive = true;

        if (VisualModel != null)
        {
            SetVisualModelActive(true);
        }
        _abilityOne = abilityOne;
    }


    public virtual void OnEnd()
    {
        if (VisualModel != null)
        {
            SetVisualModelActive(false);
        }
        // Disable this ability in manager?
    }

    protected void SetVisualModelActive(bool state)
    {
        if(VisualModel != null)
        {
         VisualModel.SetActive(state);
        }
    }

    protected void DisableAbilityPolice()
    {
        _abilityIsActive = false;

        if (_abilityOne)
        {
            SCR_AbilityManager.PoliceAbilityActive = false;
        }
        else
        {
            SCR_AbilityManager.PoliceAbilityActive2 = false;

        }

        if (VisualModel != null)
        {
            VisualModel.SetActive(false);
        }
    }
    protected void DisableAbilityTruck()
    {
        _abilityIsActive = false;

        if (_abilityOne)
        {
            SCR_AbilityManager.TruckAbilityActive = false;
        }
        else
        {
            SCR_AbilityManager.TruckAbilityActive2 = false;

        }

        if (VisualModel != null)
        {
            VisualModel.SetActive(false);
        }
    }

    protected void EnableActivateParticles(bool enable)
    {
        foreach (ParticleSystem ps in _OnActivatePs)
        {
            if (ps != null)
            {
                if (enable)
                    ps.Play();
                else
                    ps.Stop();
            }
        }
    }

    protected void EnableIdleParticles(bool enable)
    {
        foreach (ParticleSystem ps in _IdlePs)
        {
            if (ps != null)
            {

            if (enable)
                ps.Play();
            else
                ps.Stop();
            }
        }
    }

    protected void EnableIndicator(bool enable)
    {
        if (_AbilityIndicator != null)
        {
            MeshRenderer mr = _AbilityIndicator.GetComponent<MeshRenderer>();
            mr.enabled = enable;
        }
    }
}

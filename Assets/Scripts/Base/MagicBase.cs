using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MagicBase))]
public abstract class MagicBase : MonoBehaviour
{
    protected abstract void UseAbility(MagicAbilityType abilityType);

    public MagicBaseData MagicDataSet;
    protected MagicBaseData _magicDataSet => MagicDataSet;

    [HideInInspector] public GameObject CurrentAbility;
    protected GameObject _currentAbility => CurrentAbility;

    [Header("Effects")]
    [SerializeField] ParticleSystem _muzzleFlash = null;
    [SerializeField] ParticleSystem _impactEffect = null;
    [SerializeField] AudioClip _shotSound = null;
    [SerializeField] AudioClip _hitSound = null;

    protected Transform _launchLocation;
    AudioSource _audioSource;

    protected bool _isCast = false;
    protected bool _isBeam = false;
    protected bool _onCooldown = false;

    protected float _fireRate = 1;
    protected float _toCastSpeed = 10;
    protected float _currentMana;
    protected float _manaGainPerSecond;

    float startRegen = 0;
    float endRegen = 0;

    private void Start()
    {
        if (MagicDataSet != null)
        {
            InitData();
        }
        
    }

    private void Update()
    {
        UseAbilityCheck();
        //Debug.Log("Current Mana: " + _currentMana);
    }

    void InitData()
    {
        switch (_magicDataSet._magicAbilityType)
        {
            case MagicAbilityType.NULL:
                throw new System.Exception();

            case (MagicAbilityType.CAST):

                _isCast = true;
                _isBeam = false;
                break;

            case (MagicAbilityType.BEAM):

                _isBeam = true;
                _isCast = false;
                break;
        }

        _currentMana = _magicDataSet._mana;
        Mathf.Clamp(_currentMana, 0, 100);
        CurrentAbility = this.gameObject;
        _launchLocation = gameObject.transform;
    }

    void UseAbilityCheck()
    {
        if (_isCast)
        {
            if (!_onCooldown)
            {
                CastMagic();
                AbilityUseFeedback();
            }
        }
        else if (_isBeam)
        {
            if (!_onCooldown)
            {
                BeamMagic();
                AbilityUseFeedback();
            }
        }
        else if (_isBeam && _isCast)
        {
            throw new System.Exception();
        }
    }

    IEnumerator LoseMana()
    {
        if (_currentMana > 0)
        {
            _currentMana -= 1f;
        } 
        else if (_currentMana <= 0)
        {
            StopCoroutine(LoseMana());
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(LoseMana());
    }

    IEnumerator ManaRecharger()
    {
        _magicDataSet._mana = Mathf.Clamp(_magicDataSet._mana, 0, 100);
        _magicDataSet._manaRechargeRate = Mathf.Clamp(_magicDataSet._manaRechargeRate, 0, 100);
        _currentMana = Mathf.Clamp(_currentMana, 0, 100);

        float timeTillFull = (_magicDataSet._mana - _currentMana) * (1 / _magicDataSet._manaRechargeRate);
        yield return new WaitForSecondsRealtime(_magicDataSet._rechargeDelay);

        ParticleSystem idlePS = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.EmissionModule startEmissionModule = idlePS.emission;
        ParticleSystem.MinMaxCurve startEmissionCurve = startEmissionModule.rateOverTime;
        ParticleSystem.MinMaxCurve currentEmissionCurve = startEmissionCurve;

        float startEmissionValue = startEmissionCurve.constant;
        float rate = 0;
        //Debug.Log("Start SEV: " + startEmissionValue);

        currentEmissionCurve.constant = startEmissionValue * (_currentMana / _magicDataSet._mana);
        //Debug.Log("Start CEC.c1: " + currentEmissionCurve.constant);
        rate = (startEmissionValue - currentEmissionCurve.constant) / (_currentMana / _magicDataSet._manaRechargeRate);
        //Debug.Log("Charge Rate: " + rate);
        StartCoroutine(RateOverTimeIncrease(rate, timeTillFull));

        yield return new WaitForSecondsRealtime(timeTillFull * (_currentMana / _magicDataSet._mana));
    }

    IEnumerator RateOverTimeIncrease(float rate, float time)
    {
        if (_currentMana >= _magicDataSet._mana)
        {
            StopCoroutine(RateOverTimeIncrease(rate, time));
            endRegen = Time.time;
            Debug.Log("Time to regen: " + (endRegen - startRegen));
            Debug.Log("Charge Time Value: " + time);
            StopCoroutine(RateOverTimeIncrease(rate, time));
        }
        else if (_currentMana < _magicDataSet._mana)
        {

            _currentMana += rate;
            startRegen = Time.time;

        }

        Debug.Log(_currentMana);

        yield return new WaitForSeconds(1f);
        StartCoroutine(RateOverTimeIncrease(rate, time));
    }

    void CastMagic() 
    {
        StopCoroutine(ManaRecharger());

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UseAbility(MagicAbilityType.CAST);
            _currentMana -= 10;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StartCoroutine(ManaRecharger());
            StartCoroutine(Cooldown());

        }
    }

    void BeamMagic()
    {
        StopCoroutine(ManaRecharger());

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UseAbility(MagicAbilityType.BEAM);
            StartCoroutine(LoseMana());
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopCoroutine(LoseMana());
            StartCoroutine(ManaRecharger());
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        _onCooldown = true;
        StartCoroutine(ManaRecharger());
        yield return new WaitForSeconds(_magicDataSet._coolDown);
        _onCooldown = false;
    }

    protected void CurrentStats()
    {

    }

    void AbilityUseFeedback()
    {

    }
}

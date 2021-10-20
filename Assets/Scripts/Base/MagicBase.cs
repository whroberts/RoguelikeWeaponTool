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
        CurrentAbility = this.gameObject;
        _launchLocation = this.gameObject.transform;

    }

    void UseAbilityCheck()
    {
        if (_isCast)
        {
            if (!_onCooldown)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    StartCoroutine(CastMagic());
                    AbilityUseFeedback();
                }
            }
        }
        else if (_isBeam)
        {
            if (!_onCooldown)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    StartCoroutine(BeamMagic());
                    AbilityUseFeedback();
                }
            }
        }
        else if (_isBeam && _isCast)
        {
            throw new System.Exception();
        }
    }

    IEnumerator ManaRecharger()
    {
        _magicDataSet._mana = Mathf.Clamp(_magicDataSet._mana, 0, 100);
        _magicDataSet._manaRechargeRate = Mathf.Clamp(_magicDataSet._manaRechargeRate, 0, 100);
        _currentMana = Mathf.Clamp(_currentMana, 0, 100);

        float timeTillFull = (_magicDataSet._mana - _currentMana) * (1 / _magicDataSet._manaRechargeRate);
        Debug.Log(timeTillFull);
        yield return new WaitForSecondsRealtime(_magicDataSet._rechargeDelay);

        ParticleSystem[] idlePS = CurrentAbility.GetComponentsInChildren<ParticleSystem>();
        ParticleSystem.EmissionModule[] startEmissionModule = null;
        ParticleSystem.MinMaxCurve[] startEmissionCurve = null;
        ParticleSystem.MinMaxCurve[] currentEmissionCurve = null;

        float[] startEmissionValue = null;

        for (int i = 0; i < idlePS.Length; i++)
        {
            startEmissionModule[i] = idlePS[i].emission;
            startEmissionCurve[i] = startEmissionModule[i].rateOverTime;

            currentEmissionCurve[i] = startEmissionCurve[i];
            currentEmissionCurve[i].constant = startEmissionValue[i] * (_currentMana / _magicDataSet._mana);
            RateOverTimeIncrease(currentEmissionCurve[i], timeTillFull);
        }

        yield return new WaitForSecondsRealtime(timeTillFull);
    }

    IEnumerator RateOverTimeIncrease(ParticleSystem.MinMaxCurve mmc, float timeTillFull)
    {
        mmc.constant += (_magicDataSet._mana - mmc.constant) / timeTillFull;

        if (_currentMana >= _magicDataSet._mana)
        {
            StopCoroutine(RateOverTimeIncrease(mmc, timeTillFull));
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(RateOverTimeIncrease(mmc, timeTillFull));
    }

    IEnumerator CastMagic() 
    {
        StopCoroutine(ManaRecharger());
        UseAbility(MagicAbilityType.CAST);

        _onCooldown = true;

        if (!Input.GetKeyUp(KeyCode.Mouse0))
        {
            ManaRecharger();
        }
        yield return new WaitForSeconds(_magicDataSet._coolDown);
        _onCooldown = false;
    }

    IEnumerator BeamMagic()
    {
        StopCoroutine(ManaRecharger());
        UseAbility(MagicAbilityType.BEAM);

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _onCooldown = true;
            ManaRecharger();
            yield return new WaitForSeconds(_magicDataSet._coolDown);
            _onCooldown = false;
        }
    }

    protected void CurrentStats()
    {

    }

    void AbilityUseFeedback()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Types;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(GunBase))]
public abstract class MagicBase : MonoBehaviour
{
    protected abstract void CastMagic();
    protected abstract void EquipAbility();

    public MagicBaseData MagicBaseData;
    protected MagicBaseData _magicBaseData;

    [Header("Standard Effects")]
    //[SerializeField] ParticleSystem _muzzleFlash = null;
    AudioSource _audioSource;

    protected float _timeOfLastAbilityUse = 0;
    protected bool _canHoldToCast = false;

    /* pulls data from the saved data set for simplicity
     * 
     * redundant, can be cleaned up later
    */
    protected float _fireRate = 1;
    protected float _accuracy = 100;
    protected float _bulletTravelSpeed = 10;

    private void Awake()
    {
        if (MagicBaseData != null)
        {
            _magicBaseData = MagicBaseData;
            InitDataFromSet();
            EquipAbility();
        }
    }

    private void Update()
    {
        CanUseMagicCheck();
    }

    void InitDataFromSet()
    {
        switch (_magicBaseData._magicFireType)
        {
            case (MagicFireType.CAST):

                _fireRate = 52;
                _canHoldToCast = false;

                break;

            case (MagicFireType.HOLD):

                _fireRate = 3;
                _canHoldToCast = false;

                break;
        }


        _accuracy = _magicBaseData._accuracy;
        _bulletTravelSpeed = _magicBaseData._abilityTravelSpeed;
        _timeOfLastAbilityUse = Time.time;
    }

    void CanUseMagicCheck()
    {
        if (!_canHoldToCast)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_timeOfLastAbilityUse <= Time.time - _fireRate)
                {
                    _timeOfLastAbilityUse = Time.time;
                    CastMagic();
                    MuzzleFeedback();
                }
            }
        }
        else if (_canHoldToCast)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (_timeOfLastAbilityUse <= Time.time - _fireRate)
                {
                    _timeOfLastAbilityUse = Time.time;
                    CastMagic();
                    MuzzleFeedback();
                }
            }
        }
    }

    protected void CurrentStats()
    {

    }

    void MuzzleFeedback()
    {

    }
}

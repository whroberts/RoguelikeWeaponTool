using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(GunBase))]
public abstract class GunBase : MonoBehaviour
{
    protected abstract void Shoot();
    protected abstract void EquipWeapon();

    public GunBaseData GunDataSet;
    protected GunBaseData _gunDataSet => GunDataSet;

    public GameObject CurrentWeapon;
    protected GameObject _currentWeapon => CurrentWeapon;

    [Header("Header")]
    [SerializeField] ParticleSystem _muzzleFlash = null;
    [SerializeField] protected Transform _muzzleLocation = null;

    AudioSource _audioSource;

    protected float _timeOfLastShot = 0;
    protected bool _canHoldTrigger = false;

    /* pulls data from the saved data set for simplicity
     * 
     * redundant, can be cleaned up later
    */
    protected float _fireRate = 1;
    protected float _accuracy = 100;
    protected float _bulletTravelSpeed = 10;

    // for recoil
    protected float _recoil = 0;

    private void Start()
    {
        if (GunDataSet != null)
        {
            InitDataFromSet();
            EquipWeapon();
        }
    }

    private void Update()
    {
        CanUseWeaponCheck();
    }

    void InitDataFromSet()
    {
        switch (_gunDataSet._gunFireType)
        {
            case (GunFireType.SEMI):

                _fireRate = 5;
                _canHoldTrigger = false;

                break;

            case (GunFireType.BURST):

                _fireRate = 3;
                _canHoldTrigger = false;

                break;

            case (GunFireType.AUTO):

                _fireRate = 1;
                _canHoldTrigger = true;

                break;
        }

        switch (_gunDataSet._recoilType)
        {
            case (RecoilType.NONE):

                _recoil = 0;

                break;

            case (RecoilType.LIGHT):

                _recoil = 2;

                break;

            case (RecoilType.NORMAL):

                _recoil = 5;

                break;

            case (RecoilType.HIGH):

                _recoil = 7;

                break;
        }


        _accuracy = _gunDataSet._accuracy;
        _bulletTravelSpeed = _gunDataSet._bulletTravelSpeed;
        _timeOfLastShot = Time.time;
    }

    void CanUseWeaponCheck()
    {
        if (!_canHoldTrigger)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_timeOfLastShot <= Time.time - _fireRate)
                {
                    _timeOfLastShot = Time.time;
                    Shoot();
                    MuzzleFeedback();
                }
            }
        }
        else if (_canHoldTrigger)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (_timeOfLastShot <= Time.time - _fireRate)
                {
                    _timeOfLastShot = Time.time;
                    Shoot();
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

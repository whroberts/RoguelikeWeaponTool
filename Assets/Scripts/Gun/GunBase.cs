using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(GunBase))]
public abstract class GunBase : MonoBehaviour
{
    protected abstract void Shoot(int shots);

    public GunBaseData GunDataSet;
    protected GunBaseData _gunDataSet => GunDataSet;

    [HideInInspector] public GameObject CurrentWeapon;
    protected GameObject _currentWeapon => CurrentWeapon;

    [Header("Effects")]
    [SerializeField] ParticleSystem _muzzleFlash = null;
    [SerializeField] ParticleSystem _impactEffect = null;
    [SerializeField] AudioClip _shotSound = null;
    [SerializeField] AudioClip _hitSound = null;

    [SerializeField] protected Transform _muzzleLocation = null;

    AudioSource _audioSource;

    protected bool _canHoldTrigger = false;

    /* pulls data from the saved data set for simplicity
     * 
     * redundant, can be cleaned up later
    */
    protected int _fireRate = 1;
    protected float _timeOfLastShot;
    protected float _accuracy = 100;
    protected float _bulletTravelSpeed = 10;

    // for recoil
    protected float _recoil = 0;

    protected float _fireDelay = 0.1f;

    private void Start()
    {
        if (GunDataSet != null)
        {
            InitDataFromSet();
        }
        _timeOfLastShot = -_fireRate;
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

                _fireRate = 1;
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
            case (RecoilType.NULL):

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
            if (_timeOfLastShot <= Time.time - _fireRate)
            {
                Debug.Log(_timeOfLastShot);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _timeOfLastShot = Time.time;
                    Shoot(_fireRate);
                    MuzzleFeedback();
                }
            }
        }
        else if (_canHoldTrigger)
        {
            Debug.Log("Can Shoot");
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Shoot(_fireRate);
                MuzzleFeedback();
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

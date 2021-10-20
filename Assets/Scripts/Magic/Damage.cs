using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using Projectile;

public class Damage : MagicBase
{
    [Header("Ability Systems")]
    [SerializeField] GameObject _damageCast = null;
    [SerializeField] GameObject _damageBeam = null;

    protected override void UseAbility(MagicAbilityType _abilityType)
    {
        switch (_magicDataSet._magicAbilityType)
        {
            case MagicAbilityType.NULL:

                throw new System.Exception();
                
            case MagicAbilityType.CAST:

                Cast();

                break;
            case MagicAbilityType.BEAM:
                break;
        }
    }

    void Cast()
    {
        GameObject newCast = Instantiate(_damageCast, _launchLocation.position, _launchLocation.rotation);

        CastAbility cA = newCast.GetComponent<CastAbility>();

        if (cA != null)
        {
            cA.Speed = _magicDataSet._castLaunchSpeed;
        }

    }

    void Beam()
    {
        GameObject newBeam = Instantiate(_damageBeam, _launchLocation.position, _launchLocation.rotation);

        BeamAbility bA = newBeam.GetComponent<BeamAbility>();

        if (bA != null)
        {
            bA.GetComponent<ParticleSystem>().Play();
        }
    }
}


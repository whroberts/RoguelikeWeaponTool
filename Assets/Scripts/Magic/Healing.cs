using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using Projectile;

public class Healing : MagicBase
{
    [Header("Ability Systems")]
    [SerializeField] GameObject _healingCast = null;
    [SerializeField] GameObject _healingBeam = null;

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

                Beam();

                break;
        }
    }

    void Cast()
    {
        GameObject newCast = Instantiate(_healingCast, _launchLocation.position, _launchLocation.rotation);

        CastAbility cA = newCast.GetComponent<CastAbility>();

        if (cA != null)
        {
            cA.Speed = _magicDataSet._castLaunchSpeed;
        }

    }

    void Beam()
    {
        GameObject newBeam = Instantiate(_healingBeam, _launchLocation, false);

        BeamAbility bA = newBeam.GetComponent<BeamAbility>();

        if (bA == null)
        {
            throw new System.Exception();
        }
    }
}

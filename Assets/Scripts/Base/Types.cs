using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Types
{
    public enum BaseWeaponClass
    {
        NULL,
        GUN,
        MAGIC
    }

    public enum BaseGunType
    {
        NULL,
        PISTOL,
        RIFLE
    }

    public enum GunFireType
    {
        NULL,
        SEMI,
        BURST,
        AUTO
    }

    public enum RecoilType
    {
        NULL,
        LIGHT,
        NORMAL,
        HIGH
    }

    public enum BaseMagicType
    {
        NULL,
        DAMAGE,
        HEAL
    }

    public enum MagicAbilityType
    {
        NULL,
        CAST,
        BEAM
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Types
{
    public enum BaseWeaponClass
    {
        NONE,
        GUN,
        MAGIC
    }

    public enum BaseGunType
    {
        NONE,
        PISTOL,
        RIFLE
    }

    public enum GunFireType
    {
        NONE,
        SEMI,
        BURST,
        AUTO
    }

    public enum BaseMagicType
    {
        NONE,
        DAMAGE,
        HEAL
    }

    public enum MagicFireType
    {
        NONE,
        CAST,
        HOLD
    }

    public enum RecoilType
    {
        NONE,
        LIGHT,
        NORMAL,
        HIGH
    }
}

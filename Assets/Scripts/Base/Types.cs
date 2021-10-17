using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Types
{
    public enum BaseWeaponClass
    {
        GUN,
        MAGIC
    }

    public enum BaseGunType
    {
        PISTOL,
        RIFLE
    }

    public enum BaseMagicType
    {
        DAMAGE,
        HEAL
    }

    public enum GunFireType
    {
        SEMI,
        AUTO
    }

    public enum MagicFireType
    {
        CAST,
        HOLD
    }
}

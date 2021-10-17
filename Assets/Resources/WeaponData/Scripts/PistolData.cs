using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[CreateAssetMenuAttribute(fileName = "New Pistol Data", menuName = "Weapon Data/ Pistol")]
public class PistolData : WeaponData
{
    public WeaponType _weaponType;
    public FireType _fireType;
}

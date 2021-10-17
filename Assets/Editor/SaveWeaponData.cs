using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class SaveWeaponData : SetupWindow
{
    virtual public void SaveData(WeaponType weaponType)
    {
        string prefabPath; // path to the base prefab
        string newPrefabPath = "Assets/Prefabs/Characters/";
        string dataPath = "Assets/Resources/CharacterData/Data/";

        switch (weaponType)
        {
            case WeaponType.PISTOL:

                if (_saveDataSet)
                {
                    //create the .asset file
                    dataPath += "Pistol/" + WeaponCreationWindow.PistolInfo._name + ".asset";
                    AssetDatabase.CreateAsset(WeaponCreationWindow.PistolInfo, dataPath);
                }

                if (_savePrefab)
                {
                    //create the .prefab file path
                    newPrefabPath += "Pistol/" + WeaponCreationWindow.PistolInfo._name + ".prefab";

                    //get prefab path
                    prefabPath = AssetDatabase.GetAssetPath(WeaponCreationWindow.PistolInfo._basePrefab);
                    AssetDatabase.CopyAsset(prefabPath, newPrefabPath);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    GameObject weaponPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));

                    if (!weaponPrefab.GetComponent<Pistol>())
                    {
                        weaponPrefab.AddComponent(typeof(Pistol));
                    }

                    //not really sure what this does
                    //weaponPrefab.GetComponent<Pistol>()._pistolData = WeaponCreationWindow.PistolInfo;
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                break;
        }
    }
}

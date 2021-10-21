using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using Types;
using System;

public class CreationWindow : EditorWindow
{
    private bool _createNewDataSet = true;
    private bool _createNewPrefab = true;
    private bool _addToPlayer = false;
    private bool _isSaveable = false;

    static CreationWindow _window;

    static WeaponData _weaponData;

    static GunBaseData _gunBaseData;
    static MagicBaseData _magicBaseData;
    public static GunBaseData CurrentGunBase { get { return _gunBaseData; } }
    public static MagicBaseData CurrentMagicBase { get { return _magicBaseData; } }

    PopUpWindow _popUpWindow;

    GameObject _newPrefab;
    GameObject _locationToEquipTo;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();
        _weaponData = WeaponCreationWindow.WeaponData;

        AssignWeaponData();
    }

    private void AssignWeaponData()
    {
        switch (_weaponData._baseWeaponClass)
        {
            case BaseWeaponClass.NULL:
                // test for error

                throw new System.Exception();

            case BaseWeaponClass.GUN:
                // checks and assigns to gun data

                _gunBaseData = (GunBaseData)CreateInstance(typeof(GunBaseData));
                _gunBaseData._baseWeaponClass = BaseWeaponClass.GUN;
                break;
            case BaseWeaponClass.MAGIC:
                // checks and assigns to magic data

                _magicBaseData = (MagicBaseData)CreateInstance(typeof(MagicBaseData));
                _magicBaseData._baseWeaponClass = BaseWeaponClass.MAGIC;

                _magicBaseData._mana = Mathf.Clamp(_magicBaseData._mana, 0, 100);
                _magicBaseData._manaRechargeRate = Mathf.Clamp(_magicBaseData._manaRechargeRate, 0, 100);

                break;
        }
    }

    public static void OpenCreationWindow()
    {
        _window = (CreationWindow)GetWindow(typeof(CreationWindow));
        _window.minSize = new Vector2(300, 300);
        _window.Show();
    }

    private void OnGUI()
    {
        if (_gunBaseData != null)
        {
            DrawGunCreationWindow();
        }
        else if (_magicBaseData != null)
        {
            DrawMagicCreationWindow();
        }
        else
        {
            _window.Close();
            throw new System.Exception();
        }

        SetEquipLocation();
        DrawButtons();
    }

    void DrawGunCreationWindow()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Gun");
        _gunBaseData._baseGunType = (BaseGunType)EditorGUILayout.EnumPopup(_gunBaseData._baseGunType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Gun Fire Type");
        _gunBaseData._gunFireType = (GunFireType)EditorGUILayout.EnumPopup(_gunBaseData._gunFireType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Recoil Type");
        _gunBaseData._recoilType = (RecoilType)EditorGUILayout.EnumPopup(_gunBaseData._recoilType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Base Prefab");

        _gunBaseData._basePrefab = EditorGUILayout.ObjectField(_gunBaseData._basePrefab, typeof(GameObject), false);

        EditorGUILayout.EndHorizontal();

        if (_gunBaseData._basePrefab == null)
        {
            EditorGUILayout.HelpBox("Required [Prefab] missing", MessageType.Error);
            _isSaveable = false;
        }
        else if (_gunBaseData._basePrefab != null)
        {
            GameObject prefabTester = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(_gunBaseData._basePrefab), typeof(GameObject));

            if (!prefabTester.GetComponent<GunBase>())
            {
                EditorGUILayout.HelpBox("Required [Prefab][GunBase] missing", MessageType.Error);
                _isSaveable = false;
            }
        }
        else
        {
            _isSaveable = true;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        _gunBaseData._name = EditorGUILayout.TextField(_gunBaseData._name);
        EditorGUILayout.EndHorizontal();

        if (_gunBaseData._name == null)
        {
            EditorGUILayout.HelpBox("Required [Name] missing", MessageType.Error);
            _isSaveable = false;
        }
        else
        {
            _isSaveable = true;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage");
        _gunBaseData._damage = EditorGUILayout.FloatField(_gunBaseData._damage);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon Accuracy");
        _gunBaseData._accuracy = EditorGUILayout.FloatField(_gunBaseData._accuracy);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Projectile Speed");
        _gunBaseData._bulletTravelSpeed = EditorGUILayout.FloatField(_gunBaseData._bulletTravelSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Save as new DataSet");
        _createNewDataSet = EditorGUILayout.Toggle(_createNewDataSet);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Save as new Prefab");
        _createNewPrefab = EditorGUILayout.Toggle(_createNewPrefab);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Equip to Player?");
        _addToPlayer = EditorGUILayout.Toggle(_addToPlayer);
        EditorGUILayout.EndHorizontal();
    }

    void DrawMagicCreationWindow()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Type");
        _magicBaseData._baseMagicType = (BaseMagicType)EditorGUILayout.EnumPopup(_magicBaseData._baseMagicType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Magic Fire Type");
        _magicBaseData._magicAbilityType = (MagicAbilityType)EditorGUILayout.EnumPopup(_magicBaseData._magicAbilityType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Prefab");
        _magicBaseData._basePrefab = EditorGUILayout.ObjectField(_magicBaseData._basePrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        if (_magicBaseData._basePrefab == null)
        {
            EditorGUILayout.HelpBox("Required [Prefab] missing", MessageType.Error);
            _isSaveable = false;
        }
        else if (_magicBaseData._basePrefab != null)
        {
            GameObject prefabTester = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(_magicBaseData._basePrefab), typeof(GameObject));

            if (!prefabTester.GetComponent<MagicBase>())
            {
                EditorGUILayout.HelpBox("Required [Prefab][MagicBase] missing", MessageType.Error);
                _isSaveable = false;
            }
        }
        else
        {
            _isSaveable = true;
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        _magicBaseData._name = EditorGUILayout.TextField(_magicBaseData._name);
        EditorGUILayout.EndHorizontal();

        if (_magicBaseData._name == null)
        {
            EditorGUILayout.HelpBox("Required [Name] missing", MessageType.Error);
            _isSaveable = false;
        }
        else
        {
            _isSaveable = true;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginVertical();

        GUILayout.Label("Mana");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Player Mana");
        _magicBaseData._mana = EditorGUILayout.FloatField(_magicBaseData._mana);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("% Mana Recharge / Second");
        _magicBaseData._manaRechargeRate = EditorGUILayout.FloatField(_magicBaseData._manaRechargeRate);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginVertical();
        switch (_magicBaseData._baseMagicType)
        {

            case BaseMagicType.NULL:

                EditorGUILayout.BeginHorizontal();
                if (_magicBaseData._name == null)
                {
                    EditorGUILayout.HelpBox("Required [MagicType] missing", MessageType.Error);
                    _isSaveable = false;
                }
                EditorGUILayout.EndHorizontal();

                break;
            case BaseMagicType.DAMAGE:
                // draws the box for damage input

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Damage");
                _magicBaseData._damageValue = EditorGUILayout.FloatField(_magicBaseData._damageValue);
                EditorGUILayout.EndHorizontal();

                break;
            case BaseMagicType.HEAL:
                // draws the box for healing value input

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Healing Amount");
                _magicBaseData._healingValue = EditorGUILayout.FloatField(_magicBaseData._healingValue);
                EditorGUILayout.EndHorizontal();

                break;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginVertical();

        switch (_magicBaseData._magicAbilityType)
        {

            case MagicAbilityType.CAST:

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Cast Projectile Speed");
                _magicBaseData._castLaunchSpeed = EditorGUILayout.FloatField(_magicBaseData._castLaunchSpeed);
                EditorGUILayout.EndHorizontal();

                break;
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Ability Cooldown");
        _magicBaseData._coolDown = EditorGUILayout.FloatField(_magicBaseData._coolDown);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Save as new DataSet");
        _createNewDataSet = EditorGUILayout.Toggle(_createNewDataSet);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Save as new Prefab");
        _createNewPrefab = EditorGUILayout.Toggle(_createNewPrefab);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Equip to Player?");
        _addToPlayer = EditorGUILayout.Toggle(_addToPlayer);
        EditorGUILayout.EndHorizontal();
    }

    void DrawButtons()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save & Exit", GUILayout.Height(30)))
        {
            if (_isSaveable)
            {
                CreateNewWeaponData();
                EquipToLocation();
                _window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (_isSaveable)
            {
                CreateNewWeaponData();

                if (_gunBaseData != null)
                {
                    WeaponEditWindow.OpenWeaponEditWindow(_gunBaseData);
                }
                else if (_magicBaseData != null)
                {
                    MagicEditWindow.OpenMagicEditWindow(_magicBaseData);
                }
                else
                {
                    _window.Close();
                    throw new System.Exception();
                }
                EquipToLocation();
                _window.Close();
            }
        }

        if (GUILayout.Button("Exit", GUILayout.Height(30)))
        {
            PopUpWindow.OpenPopUpWindow(PopUpWindow.WindowType.CONFIRMATION);
        }
        else if (_popUpWindow.IsConfirmed)
        {
            _window.Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    void CreateNewWeaponData()
    {
        string prefabPath = "Assets/Prefabs/BaseWeapons/";
        string newPrefabPath = "Assets/Prefabs/CreatedWeapons/";
        string dataPath = "Assets/Resources/WeaponData/Data/";

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        switch (_weaponData._baseWeaponClass)
        {
            case BaseWeaponClass.NULL:
                // test for error
                _window.Close();
                throw new ArgumentNullException();

            case BaseWeaponClass.GUN:
                // checks and assigns to gun data

                SaveGunDataToFiles(prefabPath, newPrefabPath, dataPath);
                break;
            case BaseWeaponClass.MAGIC:
                // checks and assigns to magic data

                SaveMagicDataToFiles(prefabPath, newPrefabPath, dataPath);
                break;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void SaveGunDataToFiles(string basePrefabPath, string savedPrefabPath, string dataFilePath)
    {
        if (_createNewDataSet)
        {
            //create the .asset file path
            dataFilePath += _gunBaseData._name + ".asset";

            AssetDatabase.CreateAsset(_gunBaseData, dataFilePath);
        }

        if (_createNewPrefab)
        {
            //create the .prefab file path
            savedPrefabPath += "Guns/" + _gunBaseData._name + ".prefab";

            //get base prefab path

            basePrefabPath = AssetDatabase.GetAssetPath(_gunBaseData._basePrefab);

            AssetDatabase.CopyAsset(basePrefabPath, savedPrefabPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _newPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(savedPrefabPath, typeof(GameObject));

            switch (_gunBaseData._baseGunType)
            {
                case BaseGunType.PISTOL:

                    if (_newPrefab.GetComponent<Pistol>())
                    {
                        _newPrefab.GetComponent<Pistol>().GunDataSet = _gunBaseData;
                    }

                    break;
                case BaseGunType.RIFLE:

                    if (_newPrefab.GetComponent<Rifle>())
                    {
                        _newPrefab.GetComponent<Rifle>().GunDataSet = _gunBaseData;
                    }

                    break;
            }
        }
    }

    void SaveMagicDataToFiles(string basePrefabPath, string savedPrefabPath, string dataFilePath)
    {
        if (_createNewDataSet)
        {
            //create the .asset file path
            dataFilePath += _magicBaseData._name + ".asset";

            AssetDatabase.CreateAsset(_magicBaseData, dataFilePath);
        }

        if (_createNewPrefab)
        {
            //create the .prefab file path
            savedPrefabPath += "Magic/" + _magicBaseData.name + ".prefab";
            //get base prefab path
            basePrefabPath = AssetDatabase.GetAssetPath(_magicBaseData._basePrefab);

            AssetDatabase.CopyAsset(basePrefabPath, savedPrefabPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _newPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(savedPrefabPath, typeof(GameObject));

            switch (_magicBaseData._baseMagicType)
            {
                case BaseMagicType.DAMAGE:

                    if (_newPrefab.GetComponent<Damage>())
                    {
                        _newPrefab.GetComponent<Damage>().MagicDataSet = _magicBaseData;
                    }

                    break;
                case BaseMagicType.HEAL:

                    if (_newPrefab.GetComponent<Healing>())
                    {
                        _newPrefab.GetComponent<Healing>().MagicDataSet = _magicBaseData;
                    }

                    break;
            }
        }
    }

    void SetEquipLocation()
    {
        EditorGUILayout.Space(5);
        if (_addToPlayer)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Location to Equip To");
            _locationToEquipTo = (GameObject)EditorGUILayout.ObjectField(_locationToEquipTo, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();

            if (_locationToEquipTo != null)
            {
                if (_locationToEquipTo.GetComponentInChildren<GunBase>())
                {
                    GunBase[] locationChildren = _locationToEquipTo.GetComponentsInChildren<GunBase>();

                    if (locationChildren.Length > 0)
                    {
                        for (int i = 0; i < locationChildren.Length; i++)
                        {
                            DestroyImmediate(locationChildren[i].gameObject);
                        }
                    }
                }
                else if (_locationToEquipTo.GetComponentInChildren<MagicBase>())
                {
                    MagicBase[] locationChildren = _locationToEquipTo.GetComponentsInChildren<MagicBase>();

                    if (locationChildren.Length > 0)
                    {
                        for (int i = 0; i < locationChildren.Length; i++)
                        {
                            DestroyImmediate(locationChildren[i].gameObject);
                        }
                    }
                }
            }
            else if (_locationToEquipTo == null)
            {
                EditorGUILayout.HelpBox("Required [GameObject][Transform] missing", MessageType.Error);
            }
            EditorGUILayout.EndVertical();
        }
    }

    void EquipToLocation()
    {
        if (_locationToEquipTo != null)
        {
            GunBase[] locationChildren = _locationToEquipTo.GetComponentsInChildren<GunBase>();

            if (locationChildren.Length > 0)
            {
                for (int i = 0; i < locationChildren.Length; i++)
                {
                    GameObject[] children = _locationToEquipTo.GetComponentsInChildren<GameObject>();

                    DestroyImmediate(children[i]);
                }
            }
            GameObject newWeapon = Instantiate(_newPrefab, _locationToEquipTo.transform, false);
        }
    }

    private void OnDestroy()
    {
        _gunBaseData = null;
    }
}
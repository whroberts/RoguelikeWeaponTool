using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEditor;
using UnityEditor.Events;
using Types;
using System;

public class GunSetupWindow : EditorWindow
{
    private bool _createNewDataSet = true;
    private bool _createNewPrefab = true;
    private bool _isSaved = false;
    private bool _isSaveable = false;

    static GunSetupWindow _window;
    static GunBaseData _gunBaseData;
    public static GunBaseData CurrentGunBase { get { return _gunBaseData; } }

    PopUpWindow _popUpWindow;

    public event Action Check = delegate { };

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();
        _gunBaseData = (GunBaseData)CreateInstance(typeof(GunBaseData));

        _gunBaseData._baseWeaponClass = BaseWeaponClass.GUN;
    }

    public static void OpenGunSetupWindow()
    {
        _window = (GunSetupWindow)GetWindow(typeof(GunSetupWindow));
        _window.titleContent = new GUIContent("New Gun Setup");
        _window.minSize = new Vector2(300, 300);
        _window.Show();
    }

    private void OnGUI()
    {
        DrawGunCreationWindow();
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
            EditorGUILayout.HelpBox("This needs a [Name] before it can be created.", MessageType.Error);
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

        DrawButtons();
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
                _window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (_isSaveable)
            {
                _isSaved = true;
                CreateNewWeaponData();
                GunEditWindow.OpenGunEditWindow(_gunBaseData);
                _window.Close();
            }
        }

        if (GUILayout.Button("Exit", GUILayout.Height(30)))
        {
            PopUpWindow.OpenPopUpWindow(PopUpWindow.WindowType.CONFIRMATION);
        }
        else if (_popUpWindow.IsConfirmed)
        {
            //_window.Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    void CreateNewWeaponData()
    {
        string prefabPath; // path to the base prefab
        string newPrefabPath = "Assets/Prefabs/CreatedWeapons/Guns/";
        string dataPath = "Assets/Resources/WeaponData/Data/";
        string name = _gunBaseData._name;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if (_createNewDataSet)
        {
            //create the .asset file path
            dataPath += name + ".asset";

            AssetDatabase.CreateAsset(_gunBaseData, dataPath);
        }

        if (_createNewPrefab)
        {
            //create the .prefab file path
            newPrefabPath += name + ".prefab";
            //get base prefab path
            prefabPath = AssetDatabase.GetAssetPath(_gunBaseData._basePrefab);

            AssetDatabase.CopyAsset(prefabPath, newPrefabPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GameObject newPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));

            switch (_gunBaseData._baseGunType)
            {
                case BaseGunType.PISTOL:

                    if (newPrefab.GetComponent<Pistol>())
                    {
                        newPrefab.GetComponent<Pistol>().GunDataSet = _gunBaseData;
                    }

                    break;
                case BaseGunType.RIFLE:

                    if (newPrefab.GetComponent<Rifle>())
                    {
                        newPrefab.GetComponent<Rifle>().GunDataSet = _gunBaseData;
                    }

                    break;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void OnDestroy()
    {
        _gunBaseData = null;
    }
}

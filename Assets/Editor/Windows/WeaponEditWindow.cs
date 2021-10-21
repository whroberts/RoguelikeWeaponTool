using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using Types;

public class WeaponEditWindow : EditorWindow
{
    private bool _isSaved = false;
    private bool _isSaveable = false;
    private string _originalName;

    static WeaponEditWindow _window;

    static GunBaseData _unsavedData;
    static GunBaseData _savedData;

    PopUpWindow _popUpWindow;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();

        CreateSaveFileFrom(_savedData);
        _originalName = _savedData._name;
    }

    public static void OpenWeaponEditWindow(GunBaseData importData)
    {
        _savedData = importData;
        _window = (WeaponEditWindow)GetWindow(typeof(WeaponEditWindow));
        _window.titleContent = new GUIContent(importData._name);
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
        _unsavedData._baseGunType = (BaseGunType)EditorGUILayout.EnumPopup(_unsavedData._baseGunType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Gun Fire Type");
        _unsavedData._gunFireType = (GunFireType)EditorGUILayout.EnumPopup(_unsavedData._gunFireType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Recoil Type");
        _unsavedData._recoilType = (RecoilType)EditorGUILayout.EnumPopup(_unsavedData._recoilType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Base Prefab");

        _unsavedData._basePrefab = EditorGUILayout.ObjectField(_unsavedData._basePrefab, typeof(GameObject), false);

        EditorGUILayout.EndHorizontal();

        if (_unsavedData._basePrefab == null)
        {
            EditorGUILayout.HelpBox("Required [Prefab] missing", MessageType.Error);
            _isSaveable = false;
        }
        else if (_unsavedData._basePrefab != null)
        {
            GameObject prefabTester = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(_unsavedData._basePrefab), typeof(GameObject));

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

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage");
        _unsavedData._damage = EditorGUILayout.FloatField(_unsavedData._damage);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon Accuracy");
        _unsavedData._accuracy = EditorGUILayout.FloatField(_unsavedData._accuracy);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Projectile Speed");
        _unsavedData._bulletTravelSpeed = EditorGUILayout.FloatField(_unsavedData._bulletTravelSpeed);
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
                SaveWeaponData(_unsavedData);
                _isSaved = true;
                _window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (_isSaveable)
            {
                _isSaved = true;
                SaveWeaponData(_unsavedData);
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

    void CreateSaveFileFrom(GunBaseData gunData)
    {
        string tempPath = "Assets/Resources/WeaponData/Data/";
        _unsavedData = new GunBaseData();

        _unsavedData._baseGunType = gunData._baseGunType;
        _unsavedData._gunFireType = gunData._gunFireType;
        _unsavedData._basePrefab = gunData._basePrefab;
        _unsavedData._name = "tmp_" + gunData._name;
        _unsavedData._damage = gunData._damage;
        _unsavedData._damage = gunData._accuracy;
        _unsavedData._damage = gunData._bulletTravelSpeed;

        AssetDatabase.CreateAsset(_unsavedData, tempPath + _unsavedData._name + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void SaveWeaponData(GunBaseData gunData)
    {
        string tempPath = "Assets/Resources/WeaponData/Data/";
        _savedData = new GunBaseData();

        _savedData._baseGunType = gunData._baseGunType;
        _savedData._gunFireType = gunData._gunFireType;
        _savedData._basePrefab = gunData._basePrefab;
        _savedData._name = _originalName;
        _savedData._damage = gunData._damage;
        _savedData._damage = gunData._accuracy;
        _savedData._damage = gunData._bulletTravelSpeed;

        AssetDatabase.CreateAsset(_savedData, tempPath + _savedData._name + ".asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void OnDestroy()
    {
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_unsavedData));
        _unsavedData = null;
        _savedData = null;
    }
}

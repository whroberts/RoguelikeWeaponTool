using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEditor;
using UnityEditor.Events;
using Types;
using System;

public class GunEditWindow : EditorWindow
{
    private bool _isSaved = false;
    private bool _isSaveable = false;
    private string _originalName;

    static GunEditWindow _window;

    static GunBaseData _unsavedGunData;
    static GunBaseData _savedGunData;

    PopUpWindow _popUpWindow;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();

        CreateSaveFileFrom(_savedGunData);
        _originalName = _savedGunData._name;
    }

    public static void OpenGunEditWindow(GunBaseData importData)
    {
        _savedGunData = importData;
        _window = (GunEditWindow)GetWindow(typeof(GunEditWindow));
        _window.titleContent = new GUIContent(importData._name);
        _window.minSize = new Vector2(300, 300);
        _window.Show();
    }

    private void OnGUI()
    {
        DrawGunEditWindow();
    }

    void DrawGunEditWindow()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Gun");
        _unsavedGunData._baseGunType = (BaseGunType)EditorGUILayout.EnumPopup(_unsavedGunData._baseGunType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Gun Fire Type");
        _unsavedGunData._gunFireType = (GunFireType)EditorGUILayout.EnumPopup(_unsavedGunData._gunFireType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Prefab");
        _unsavedGunData._basePrefab = EditorGUILayout.ObjectField(_unsavedGunData._basePrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        if (_unsavedGunData._basePrefab == null)
        {
            EditorGUILayout.HelpBox("Required [Prefab] missing", MessageType.Error);
            _isSaveable = false;
        }
        else
        {
            _isSaveable = true;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        _unsavedGunData._name = EditorGUILayout.TextField(_unsavedGunData._name);
        EditorGUILayout.EndHorizontal();

        if (_unsavedGunData._name == null)
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
        _unsavedGunData._damage = EditorGUILayout.FloatField(_unsavedGunData._damage);
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
                SaveWeaponData(_unsavedGunData);
                _isSaved = true;
                //_window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (_isSaveable)
            {
                _isSaved = true;
                SaveWeaponData(_unsavedGunData);
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
        _unsavedGunData = new GunBaseData();

        _unsavedGunData._baseGunType = gunData._baseGunType;
        _unsavedGunData._gunFireType = gunData._gunFireType;
        _unsavedGunData._basePrefab = gunData._basePrefab;
        _unsavedGunData._name = "tmp_" + gunData._name;
        _unsavedGunData._damage = gunData._damage;

        AssetDatabase.CreateAsset(_unsavedGunData, tempPath + _unsavedGunData._name + ".asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void SaveWeaponData(GunBaseData gunData)
    {
        string tempPath = "Assets/Resources/WeaponData/Data/";

        //AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_savedGunData));
        _savedGunData = new GunBaseData();

        _savedGunData._baseGunType = gunData._baseGunType;
        _savedGunData._gunFireType = gunData._gunFireType;
        _savedGunData._basePrefab = gunData._basePrefab;
        _savedGunData._name = _originalName;
        _savedGunData._damage = gunData._damage;

        AssetDatabase.CreateAsset(_savedGunData, tempPath + _savedGunData._name + ".asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    bool IsFileDirty()
    {
        bool dirty = false;

        if (_unsavedGunData._baseGunType != _savedGunData._baseGunType)
        {
            dirty = true;
        }

        if (_unsavedGunData._gunFireType != _savedGunData._gunFireType)
        {
            dirty = true;
        }

        if (_unsavedGunData._basePrefab != _savedGunData._basePrefab)
        {
            dirty = true;
        }

        if (_unsavedGunData._damage != _savedGunData._damage)
        {
            dirty = true;
        }

        return dirty;
    }

    private void OnDestroy()
    {
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_unsavedGunData));
        _unsavedGunData = null;
        _savedGunData = null;
    }
}

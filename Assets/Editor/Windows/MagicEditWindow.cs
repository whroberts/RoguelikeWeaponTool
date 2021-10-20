using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEditor;
using UnityEditor.Events;
using Types;
using System;

public class MagicEditWindow : EditorWindow
{
    private bool _isSaved = false;
    private bool _isSaveable = false;
    private string _originalName;

    static MagicEditWindow _window;

    static MagicBaseData _unsavedMagicData;
    static MagicBaseData _savedMagicData;

    PopUpWindow _popUpWindow;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();

        CreateSaveFileFrom(_savedMagicData);
        _originalName = _savedMagicData._name;
    }

    public static void OpenMagicEditWindow(MagicBaseData importData)
    {
        _savedMagicData = importData;
        _window = (MagicEditWindow)GetWindow(typeof(MagicEditWindow));
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
        _unsavedMagicData._baseMagicType = (BaseMagicType)EditorGUILayout.EnumPopup(_unsavedMagicData._baseMagicType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Gun Fire Type");
        _unsavedMagicData._magicAbilityType = (MagicAbilityType)EditorGUILayout.EnumPopup(_unsavedMagicData._magicAbilityType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Prefab");
        _unsavedMagicData._basePrefab = EditorGUILayout.ObjectField(_unsavedMagicData._basePrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        if (_unsavedMagicData._basePrefab == null)
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
        _unsavedMagicData._name = EditorGUILayout.TextField(_unsavedMagicData._name);
        EditorGUILayout.EndHorizontal();

        if (_unsavedMagicData._name == null)
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
        _unsavedMagicData._damageValue = EditorGUILayout.FloatField(_unsavedMagicData._damageValue);
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
                SaveWeaponData(_unsavedMagicData);
                _isSaved = true;
                //_window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (_isSaveable)
            {
                _isSaved = true;
                SaveWeaponData(_unsavedMagicData);
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

    void CreateSaveFileFrom(MagicBaseData gunData)
    {
        string tempPath = "Assets/Resources/WeaponData/Data/";
        _unsavedMagicData = new MagicBaseData();
        _unsavedMagicData._baseWeaponClass = BaseWeaponClass.MAGIC;
        _unsavedMagicData._baseMagicType = gunData._baseMagicType;
        _unsavedMagicData._magicAbilityType = gunData._magicAbilityType;
        _unsavedMagicData._basePrefab = gunData._basePrefab;
        _unsavedMagicData._name = "tmp_" + gunData._name;
        _unsavedMagicData._damageValue = gunData._damageValue;

        AssetDatabase.CreateAsset(_unsavedMagicData, tempPath + _unsavedMagicData._name + ".asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void SaveWeaponData(MagicBaseData gunData)
    {
        string tempPath = "Assets/Resources/WeaponData/Data/";

        //AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_savedGunData));
        _savedMagicData = new MagicBaseData();

        _savedMagicData._baseMagicType = gunData._baseMagicType;
        _savedMagicData._magicAbilityType = gunData._magicAbilityType;
        _savedMagicData._basePrefab = gunData._basePrefab;
        _savedMagicData._name = _originalName;
        _savedMagicData._damageValue = gunData._damageValue;

        AssetDatabase.CreateAsset(_savedMagicData, tempPath + _savedMagicData._name + ".asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    bool IsFileDirty()
    {
        bool dirty = false;

        if (_unsavedMagicData._baseMagicType != _savedMagicData._baseMagicType)
        {
            dirty = true;
        }

        if (_unsavedMagicData._magicAbilityType != _savedMagicData._magicAbilityType)
        {
            dirty = true;
        }

        if (_unsavedMagicData._basePrefab != _savedMagicData._basePrefab)
        {
            dirty = true;
        }

        if (_unsavedMagicData._damageValue != _savedMagicData._damageValue)
        {
            dirty = true;
        }

        return dirty;
    }

    private void OnDestroy()
    {
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_unsavedMagicData));
        _unsavedMagicData = null;
        _savedMagicData = null;
    }
}
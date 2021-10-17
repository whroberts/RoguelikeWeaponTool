using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class SetupWindow : EditorWindow
{
    protected bool _saveDataSet = true;
    protected bool _savePrefab = true;

    static WeaponType _weaponType;
    static SetupWindow _window;

    PopUpWindow _popUpWindow;
    SaveWeaponData _saveWeaponData;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();
        _saveWeaponData = CreateInstance<SaveWeaponData>();
    }

    public static void OpenSetupWindow(WeaponType weaponType)
    {
        _weaponType = weaponType;
        _window = (SetupWindow)GetWindow(typeof(SetupWindow));
        _window.titleContent = new GUIContent(weaponType.ToString().Substring(0, 1) + weaponType.ToString().Substring(1).ToLower() + " Setup");
        _window.minSize = new Vector2(300, 300);
        _window.Show();
    }

    private void OnGUI()
    {
        switch (_weaponType)
        {
            case WeaponType.PISTOL:
                DrawSettings(WeaponCreationWindow.PistolInfo);
                break;
        }
    }

    void DrawSettings(WeaponData weaponData)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Weapon");
        weaponData._basePrefab = EditorGUILayout.ObjectField(weaponData._basePrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Max Health");
        weaponData._name = EditorGUILayout.TextField(weaponData._name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Max Energy");
        weaponData._damage = EditorGUILayout.FloatField(weaponData._damage);
        EditorGUILayout.EndHorizontal();


        DrawButtons(weaponData);
    }

    void DrawButtons(WeaponData weaponData)
    {
        bool isSaveable = false;

        EditorGUILayout.BeginVertical();
        if (weaponData._basePrefab == null)
        {
            EditorGUILayout.HelpBox("This enemy needs a [Prefab] before it can be created.", MessageType.Error);
            isSaveable = false;
        }
        if (weaponData._name == null)
        {
            EditorGUILayout.HelpBox("This enemy needs a [Name] before it can be created.", MessageType.Error);
            isSaveable = false;
        }
        if (weaponData._basePrefab != null && weaponData._name != null)
        {
            isSaveable = true;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save & Exit", GUILayout.Height(30)))
        {
            if (isSaveable)
            {
                _saveWeaponData.SaveData(_weaponType);
                _window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (isSaveable)
            {
                _saveWeaponData.SaveData(_weaponType);
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

        if (GUILayout.Button("Create New", GUILayout.Height(30)))
        {
            
        }

        EditorGUILayout.EndVertical();
    }
}

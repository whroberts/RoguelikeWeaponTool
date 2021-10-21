using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class LoadWindow : EditorWindow
{
    static LoadWindow _window;

    static WeaponData _weaponData;
    static GunBaseData _loadedGunBaseData;
    static MagicBaseData _loadedMagicBaseData;

    public static GunBaseData LoadedGunBaseData { get { return _loadedGunBaseData; } }
    public static MagicBaseData LoadedMagicBaseData { get { return _loadedMagicBaseData; } }

    private void OnEnable()
    {
        _weaponData = null;
    }

    public static void OpenLoadWindow()
    {
        _window = (LoadWindow)GetWindow(typeof(LoadWindow));
        _window.titleContent = new GUIContent("Load Data");
        _window.minSize = new Vector2(300, 150);
        _window.Show();
    }

    private void OnGUI()
    {
        DrawLoadWindow();
    }

    private void DrawLoadWindow()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Load Data Set");
        _weaponData = (WeaponData) EditorGUILayout.ObjectField(_weaponData, typeof(WeaponData), false);
        EditorGUILayout.EndHorizontal();

        if (_weaponData != null)
        {
            if (_weaponData.GetType().Equals(typeof(GunBaseData)))
            {
                _loadedGunBaseData = (GunBaseData)_weaponData;
            }
            else if (_weaponData.GetType().Equals(typeof(MagicBaseData)))
            {
                _loadedMagicBaseData = (MagicBaseData)_weaponData;
            }
        }
        else 
            EditorGUILayout.HelpBox("[Data Set] Required", MessageType.Error);

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(5);

        DrawButtons();
    }

    private void DrawButtons()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Edit", GUILayout.Height(40)))
        {
            AssetDatabase.Refresh();

            switch (_weaponData._baseWeaponClass)
            {
                case BaseWeaponClass.GUN:

                    if (_loadedGunBaseData != null)
                    {
                        WeaponEditWindow.OpenWeaponEditWindow(_loadedGunBaseData);
                        _window.Close();
                    }

                    break;
                case BaseWeaponClass.MAGIC:

                    if (_loadedMagicBaseData != null)
                    {
                        MagicEditWindow.OpenMagicEditWindow(_loadedMagicBaseData);
                        _window.Close();
                    }

                    break;
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Cancel", GUILayout.Height(40)))
        {
            _window.Close();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}

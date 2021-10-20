using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class WeaponCreationWindow : EditorWindow
{
    Texture2D _headerSectionTexture;
    Texture2D _pistolSectionTexture;

    Texture2D[] _textures;

    Rect _headerSection;
    Rect _creationSection;
    Rect[] _baseSections;

    static WeaponData _weaponData;
    public static WeaponData WeaponData { get { return _weaponData; } }

    [MenuItem("Window/Weapon Designer")]
    static void OpenWindow()
    {
        WeaponCreationWindow window = (WeaponCreationWindow)GetWindow(typeof(WeaponCreationWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        InitSectionVisuals();
        InitData();
    }
    
    private static void InitData()
    {
        _weaponData = (WeaponData)CreateInstance(typeof(WeaponData));
    }

    /// <summary>
    /// Initialize 2D visuals
    /// </summary>

    void InitSectionVisuals()
    {
        _headerSectionTexture = Resources.Load<Texture2D>("icons/blue");
        _pistolSectionTexture = Resources.Load<Texture2D>("icons/redOrange");

        _textures = new Texture2D[] { _headerSectionTexture, _pistolSectionTexture };
        _baseSections = new Rect[] { _headerSection, _creationSection};
    }

    private void OnGUI()
    {
        DrawBaseSections();
        DrawHeaderSection();
        DrawCreationSection();
    }

    void DrawBaseSections()
    {
        _baseSections[0].x = 0;
        _baseSections[0].y = 0;
        _baseSections[0].width = Screen.width;
        _baseSections[0].height = 50;

        for (int i = 1; i < _baseSections.Length; i++)
        {
            _baseSections[i].x = (i - 1) * Screen.width / (_baseSections.Length - 1);
            _baseSections[i].y = _baseSections[0].height;
            _baseSections[i].width = Screen.width / (_baseSections.Length - 1);
            _baseSections[i].height = Screen.height - _baseSections[0].height;

            for (int j = 0; j <= i; j++)
            {
                GUI.DrawTexture(_baseSections[j], _textures[j]);
            }
        }

        _headerSection = _baseSections[0];
        _creationSection = _baseSections[1];
    }
    void DrawHeaderSection()
    {
        GUILayout.BeginArea(_headerSection);

        GUILayout.Label("Enemy Designer Testing");

        GUILayout.EndArea();
    }

    void DrawCreationSection()
    {
        GUILayout.BeginArea(_creationSection);

        GUILayout.Label("Create New Weapon");
        GUILayout.Space(5);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Weapon Base Class");
        _weaponData._baseWeaponClass = (BaseWeaponClass)EditorGUILayout.EnumPopup(_weaponData._baseWeaponClass);

        EditorGUILayout.EndHorizontal();

        if (_weaponData._baseWeaponClass == BaseWeaponClass.NULL)
        {
            EditorGUILayout.HelpBox("Required [WeaponClass] missing", MessageType.Error);
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(5);

        DrawButtons();

        GUILayout.EndArea();
    }
    

    void DrawButtons()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create New", GUILayout.Height(40)))
        {
            AssetDatabase.Refresh();

            /*
            switch (_weaponData._baseWeaponClass)
            {
                case BaseWeaponClass.GUN:
                    GunSetupWindow.OpenGunSetupWindow();
                    break;
                case BaseWeaponClass.MAGIC:
                    MagicSetupWindow.OpenMagicSetupWindow();
                    break;
            }
            */
            CreationWindow.OpenCreationWindow();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load", GUILayout.Height(40)))
        {
            AssetDatabase.Refresh();
            LoadWindow.OpenLoadWindow();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}

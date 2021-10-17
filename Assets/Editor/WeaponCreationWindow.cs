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
    Rect _pistolSection;
    Rect[] _baseSections;

    //most important
    static TempData _tempData;
    static EmptyDataSet _emptyData;
    public static TempData TempInfo { get { return _tempData; } }
    public static EmptyDataSet EmptyInfo { get { return _emptyData; } }

    //rest
    static PistolData _pistolData;
    public static PistolData PistolInfo { get { return _pistolData; } }

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
        _pistolData = (PistolData)CreateInstance(typeof(PistolData));
        _tempData = (TempData)CreateInstance(typeof(TempData));
        _emptyData = (EmptyDataSet)CreateInstance(typeof(EmptyDataSet));
    }

    /// <summary>
    /// Initialize 2D visuals
    /// </summary>

    void InitSectionVisuals()
    {
        _headerSectionTexture = Resources.Load<Texture2D>("icons/blue");
        _pistolSectionTexture = Resources.Load<Texture2D>("icons/redOrange");

        _textures = new Texture2D[] { _headerSectionTexture, _pistolSectionTexture };
        _baseSections = new Rect[] { _headerSection, _pistolSection};
    }

    private void OnGUI()
    {
        DrawBaseSections();
        DrawHeaderSection();
        //DrawInitialMenu();
        DrawPistolSection();
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
        _pistolSection = _baseSections[1];
    }
    void DrawHeaderSection()
    {
        GUILayout.BeginArea(_headerSection);

        GUILayout.Label("Enemy Designer Testing");

        GUILayout.EndArea();
    }

    void DrawInitialMenu()
    {
        GUILayout.BeginArea(_pistolSection);
        // {

        GUILayout.Label("Pistol");
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage");
        _tempData._weaponType = (WeaponType)EditorGUILayout.EnumPopup(_tempData._weaponType);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon");
        _tempData._fireType = (FireType)EditorGUILayout.EnumPopup(_tempData._fireType);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        CreateButtons(WeaponType.PISTOL);

        // }
        GUILayout.EndArea();
    }

    
    void DrawPistolSection()
    {
        GUILayout.BeginArea(_pistolSection);
        // {

        GUILayout.Label("Pistol");
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage");
        _pistolData._weaponType = (WeaponType)EditorGUILayout.EnumPopup(_pistolData._weaponType);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon");
        _pistolData._fireType = (FireType)EditorGUILayout.EnumPopup(_pistolData._fireType);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        CreateButtons(WeaponType.PISTOL);

        // }
        GUILayout.EndArea();
    }
    

    void CreateButtons(WeaponType weaponType)
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create", GUILayout.Height(40)))
        {
            AssetDatabase.Refresh();
            SetupWindow.OpenSetupWindow(weaponType);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class EditExistingWindow : EditorWindow
{
    private bool _saveDataSet = true;
    private bool _savePrefab = true;
    private bool _isSaved = false;
    private string _savedName = "";

    static EditExistingWindow _window;

    PopUpWindow _popUpWindow;
    LoadWindow _loadWindow;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();
        _loadWindow = CreateInstance<LoadWindow>();
    }

    public static void OpenEditExistingWindow()
    {
        _window = (EditExistingWindow)GetWindow(typeof(EditExistingWindow));
        //_window.titleContent = new GUIContent(weaponType.ToString().Substring(0, 1) + weaponType.ToString().Substring(1).ToLower() + " Setup");
        _window.minSize = new Vector2(300, 300);
        _window.Show();
    }

    /*
    private void OnGUI()
    {
        DrawSettings(_loadWindow.LoadedDataSet);
    }

    void DrawSettings(ScriptableObject loadedDataSet)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Weapon");
        loadedDataSet._basePrefab = EditorGUILayout.ObjectField(loadedDataSet._basePrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        loadedDataSet._name = EditorGUILayout.TextField(loadedDataSet._name);
        _savedName = weaponData._name;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage");
        loadedDataSet._damage = EditorGUILayout.FloatField(loadedDataSet._damage);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Save as new DataSet");
        _saveDataSet = EditorGUILayout.Toggle(_saveDataSet);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Save as new Prefab");
        _savePrefab = EditorGUILayout.Toggle(_savePrefab);
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
                SaveWeaponData();

                _isSaved = true;
                _window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (isSaveable)
            {
                _isSaved = true;
                SaveWeaponData();
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

    void SaveWeaponData()
    {
        string prefabPath; // path to the base prefab
        string newPrefabPath = "Assets/Prefabs/CreatedWeapons/";
        string dataPath = "Assets/Resources/WeaponData/Data/";

        switch (_saveToThisWeaponType)
        {
            case WeaponType.PISTOL:

                if (_saveDataSet)
                {
                    string test = AssetDatabase.RenameAsset(dataPath + "TempDataSet.asset", _savedName + ".asset");
                    Debug.Log(test);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                break;
        }
    }

    private void OnDestroy()
    {
        if (!_isSaved)
        {
            AssetDatabase.DeleteAsset("Assets/Resources/WeaponData/Data/TempDataSet.asset");
        }
    }
    */
}

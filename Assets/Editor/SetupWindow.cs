using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class SetupWindow : EditorWindow
{
    private bool _saveDataSet = true;
    private bool _savePrefab = true;

    static WeaponType _saveToThisWeaponType;
    static SetupWindow _window;
    static PistolData _newPistolData;
    public static PistolData NewPistolInfo { get { return _newPistolData; } }

    PopUpWindow _popUpWindow;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();
    }

    public static void OpenSetupWindow(WeaponType weaponType)
    {
        _saveToThisWeaponType = weaponType;
        _window = (SetupWindow)GetWindow(typeof(SetupWindow));
        _window.titleContent = new GUIContent(weaponType.ToString().Substring(0, 1) + weaponType.ToString().Substring(1).ToLower() + " Setup");
        _window.minSize = new Vector2(300, 300);
        _window.Show();
    }

    private void OnGUI()
    {
        switch (_saveToThisWeaponType)
        {
            case WeaponType.PISTOL:
                //writting to new PistolInfo
                DrawSettings(WeaponCreationWindow.PistolInfo);

                //writting to base PistolInfo
                //DrawSettings(WeaponCreationWindow.PistolInfo);

                //writting to temp
                //DrawSettings(WeaponCreationWindow.TempInfo);
                break;
        }
    }

    void DrawSettings(WeaponData weaponData)
    {
        CreateNewWeaponData();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Weapon");
        weaponData._basePrefab = EditorGUILayout.ObjectField(weaponData._basePrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        weaponData._name = EditorGUILayout.TextField(weaponData._name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage");
        weaponData._damage = EditorGUILayout.FloatField(weaponData._damage);
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

                //AssignTempFile("ToEmpty");

                _window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (isSaveable)
            {
                SaveWeaponData();
            }
        }

        if (GUILayout.Button("Exit", GUILayout.Height(30)))
        {
            PopUpWindow.OpenPopUpWindow(PopUpWindow.WindowType.CONFIRMATION);
        }
        else if (_popUpWindow.IsConfirmed)
        {
            //AssignTempFile("ToEmpty");

            _window.Close();

        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create New", GUILayout.Height(30)))
        {

        }

        EditorGUILayout.EndVertical();
    }
    void CreateNewWeaponData()
    {
        string dataPath = "Assets/Resources/WeaponData/Data/";

        switch (_saveToThisWeaponType)
        {
            case WeaponType.PISTOL:


                if (_saveDataSet)
                {
                    //create the .asset file
                    //dataPath += WeaponCreationWindow.PistolInfo._name + ".asset";
                    dataPath += "NewPistolData" + ".asset";
                    AssetDatabase.CreateAsset(NewPistolInfo, dataPath);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                break;
        }
    }

    void SaveWeaponData()
    {
        string prefabPath; // path to the base prefab
        string newPrefabPath = "Assets/Prefabs/CreatedWeapons/";
        string dataPath = "Assets/Resources/WeaponData/Data/";

        switch (_saveToThisWeaponType)
        {
            case WeaponType.PISTOL:

                //AssignTempFile("ToDataFile");

                if (_saveDataSet)
                {
                    //create the .asset file
                    dataPath += WeaponCreationWindow.PistolInfo._name + ".asset";
                    AssetDatabase.CreateAsset(WeaponCreationWindow.PistolInfo, dataPath);
                }

                if (_savePrefab)
                {
                    //create the .prefab file path
                    newPrefabPath += WeaponCreationWindow.PistolInfo._name + ".prefab";

                    //get prefab path
                    prefabPath = AssetDatabase.GetAssetPath(WeaponCreationWindow.PistolInfo._basePrefab);
                    AssetDatabase.CopyAsset(prefabPath, newPrefabPath);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    GameObject weaponPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));

                    if (!weaponPrefab.GetComponent<Pistol>())
                    {
                        weaponPrefab.AddComponent(typeof(Pistol));
                    }

                    //not really sure what this does
                    //weaponPrefab.GetComponent<Pistol>()._pistolData = WeaponCreationWindow.PistolInfo;
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                break;
        }
    }

    /*
    void AssignTempFile(string operation)
    {
        string basePath = "Assets/Resources/WeaponData/Scripts/";

        switch (operation)
        {
            case "ToDataFile":

                AssetDatabase.DeleteAsset(basePath + "PistolData.cs");
                AssetDatabase.CopyAsset(basePath + "TempData.cs", basePath + "PistolData.cs");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                break;

            case "ToEmpty":

                AssetDatabase.DeleteAsset(basePath + "TempData.cs");
                AssetDatabase.CopyAsset(basePath + "EmptyData.cs", basePath + "TempData.cs");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                break;
        }
    }
    */
}

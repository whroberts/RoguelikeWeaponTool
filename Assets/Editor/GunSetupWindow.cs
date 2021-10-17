using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class GunSetupWindow : EditorWindow
{
    private bool _createNewDataSet = true;
    private bool _createNewPrefab = true;
    private bool _isSaved = false;
    private bool _isSaveable = false;

    static GunSetupWindow _window;

    static GunBaseData _gunBaseData;
    public static GunBaseData NewGunBase { get { return _gunBaseData; } }

    PopUpWindow _popUpWindow;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();
        _gunBaseData = (GunBaseData)CreateInstance(typeof(GunBaseData));
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

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Prefab");
        _gunBaseData._basePrefab = EditorGUILayout.ObjectField(_gunBaseData._basePrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        if (_gunBaseData._basePrefab == null)
        {
            EditorGUILayout.HelpBox("This needs a [Prefab] before it can be created.", MessageType.Error);
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
                SaveWeaponData();

                _isSaved = true;
                _window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (_isSaveable)
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
        EditorGUILayout.EndVertical();
    }

    void SaveWeaponData()
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

                    if (!newPrefab.GetComponent<Pistol>())
                    {
                        newPrefab.AddComponent(typeof(Pistol));
                        newPrefab.GetComponent<Pistol>()._gunBaseData = _gunBaseData;
                    }

                    break;
                case BaseGunType.RIFLE:

                    if (!newPrefab.GetComponent<Rifle>())
                    {
                        newPrefab.AddComponent(typeof(Rifle));
                        newPrefab.GetComponent<Rifle>()._gunBaseData = _gunBaseData;
                    }

                    break;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

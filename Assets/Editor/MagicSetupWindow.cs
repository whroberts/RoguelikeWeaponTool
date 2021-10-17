using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class MagicSetupWindow : EditorWindow
{
    private bool _createNewDataSet = true;
    private bool _createNewPrefab = true;
    private bool _isSaved = false;
    private bool _isSaveable = false;

    static MagicSetupWindow _window;

    static MagicBaseData _magicBaseData;
    public static MagicBaseData NewMagicBase { get { return _magicBaseData; } }

    PopUpWindow _popUpWindow;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();
        _magicBaseData = (MagicBaseData)CreateInstance(typeof(MagicBaseData));
    }

    public static void OpenMagicSetupWindow()
    {
        _window = (MagicSetupWindow)GetWindow(typeof(MagicSetupWindow));
        _window.titleContent = new GUIContent("New Magic Setup");
        _window.minSize = new Vector2(300, 300);
        _window.Show();
    }

    private void OnGUI()
    {
        DrawMagicSetupWindow();
    }

    void DrawMagicSetupWindow()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Gun");
        _magicBaseData._baseMagicType = (BaseMagicType)EditorGUILayout.EnumPopup(_magicBaseData._baseMagicType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Gun Fire Type");
        _magicBaseData._magicFireType = (MagicFireType)EditorGUILayout.EnumPopup(_magicBaseData._magicFireType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Prefab");
        _magicBaseData._basePrefab = EditorGUILayout.ObjectField(_magicBaseData._basePrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        if (_magicBaseData._basePrefab == null)
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
        _magicBaseData._name = EditorGUILayout.TextField(_magicBaseData._name);
        EditorGUILayout.EndHorizontal();

        if (_magicBaseData._name == null)
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
        _magicBaseData._damage = EditorGUILayout.FloatField(_magicBaseData._damage);
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
        string newPrefabPath = "Assets/Prefabs/CreatedWeapons/Magic/";
        string dataPath = "Assets/Resources/WeaponData/Data/";
        string name = _magicBaseData._name;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if (_createNewDataSet)
        {
            dataPath += name + ".asset";
            AssetDatabase.CreateAsset(_magicBaseData, dataPath);
        }

        if (_createNewPrefab)
        {
            //create the .prefab file path
            newPrefabPath += name + ".prefab";
            //get base prefab path
            prefabPath = AssetDatabase.GetAssetPath(_magicBaseData._basePrefab);

            AssetDatabase.CopyAsset(prefabPath, newPrefabPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GameObject newPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));

            switch (_magicBaseData._baseMagicType)
            {
                case BaseMagicType.DAMAGE:

                    if (!newPrefab.GetComponent<Damage>())
                    {
                        newPrefab.AddComponent(typeof(Damage));
                        newPrefab.GetComponent<Damage>()._magicBaseData = _magicBaseData;
                    }

                    break;
                case BaseMagicType.HEAL:

                    if (!newPrefab.GetComponent<Heal>())
                    {
                        newPrefab.AddComponent(typeof(Heal));
                        newPrefab.GetComponent<Heal>()._magicBaseData = _magicBaseData;
                    }

                    break;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

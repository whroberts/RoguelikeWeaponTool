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
    public static MagicBaseData CurrentMagicBase { get { return _magicBaseData; } }

    PopUpWindow _popUpWindow;

    private void OnEnable()
    {
        _popUpWindow = CreateInstance<PopUpWindow>();
        _magicBaseData = (MagicBaseData)CreateInstance(typeof(MagicBaseData));
        _magicBaseData._baseWeaponClass = BaseWeaponClass.MAGIC;
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
        GUILayout.Label("Base Type");
        _magicBaseData._baseMagicType = (BaseMagicType)EditorGUILayout.EnumPopup(_magicBaseData._baseMagicType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Magic Fire Type");
        _magicBaseData._magicAbilityType = (MagicAbilityType)EditorGUILayout.EnumPopup(_magicBaseData._magicAbilityType);
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
        _magicBaseData._damageValue = EditorGUILayout.FloatField(_magicBaseData._damageValue);
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
                CreateNewDataSet();

                _isSaved = true;
                _window.Close();
            }
        }
        else if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (_isSaveable)
            {
                _isSaved = true;
                CreateNewDataSet();
                MagicEditWindow.OpenMagicEditWindow(_magicBaseData);
                _window.Close();
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

    void CreateNewDataSet()
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
                        newPrefab.GetComponent<Damage>().MagicDataSet = _magicBaseData;
                    }

                    break;
                case BaseMagicType.HEAL:

                    if (!newPrefab.GetComponent<Healing>())
                    {
                        newPrefab.AddComponent(typeof(Healing));
                        newPrefab.GetComponent<Healing>().MagicDataSet = _magicBaseData;
                    }

                    break;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

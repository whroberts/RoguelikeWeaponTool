using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class LoadWindow : EditorWindow
{
    static LoadWindow _window;

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
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Load Data");
        //weaponData._basePrefab = EditorGUILayout.ObjectField(weaponData._basePrefab, typeof(GameObject), false);
        //_loadedDataSet = (ScriptableObject) EditorGUILayout.ObjectField(_loadedDataSet, typeof(ScriptableObject), false);
        EditorGUILayout.EndHorizontal();
    }
}

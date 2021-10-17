using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class LoadWindow : EditorWindow
{
    static LoadWindow _window;

    private bool isConfirmed = false;
    public bool IsConfirmed => isConfirmed;

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

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class PopUpWindow : EditorWindow
{
    public enum WindowType
    {
        CONFIRMATION,
        ERROR,
        NULL
    }

    static WindowType _windowSetting;
    static PopUpWindow _window;

    private bool _isConfirmed = false;
    public bool IsConfirmed => _isConfirmed;

    public static void OpenPopUpWindow(WindowType setting)
    {
        _windowSetting = setting;
        _window = (PopUpWindow)GetWindow(typeof(PopUpWindow));
        _window.titleContent = new GUIContent(setting.ToString().Substring(0, 1) + setting.ToString().Substring(1).ToLower() + " Window");
        _window.minSize = new Vector2(200, 75);
        _window.Show();
    }

    private void OnGUI()
    {
        switch (_windowSetting)
        {
            case WindowType.CONFIRMATION:
                DrawConfirmationPopUp();
                break;
            case WindowType.ERROR:
                DrawErrorPopUp();
                break;
            case WindowType.NULL:
                break;
        }
    }

    private void DrawConfirmationPopUp()
    {
        EditorGUILayout.HelpBox("Are you sure you want to close without saving?", MessageType.Warning);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Confirm", GUILayout.Height(30)))
        {
            _isConfirmed = true;
            _window.Close();
        }
        else if (GUILayout.Button("Cancel", GUILayout.Height(30)))
        {
            _isConfirmed = false;
            _window.Close();
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawErrorPopUp()
    {
        EditorGUILayout.HelpBox("Error", MessageType.Error);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Okay", GUILayout.Height(30)))
        {
            _window.Close();
        }
        EditorGUILayout.EndHorizontal();
    }
}

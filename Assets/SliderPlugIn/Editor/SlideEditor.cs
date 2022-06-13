using System;
using UnityEditor;
using UnityEngine;



public class SlideEditor : EditorWindow
{
    bool _drawNewUI = false;
    bool _drawSlideControllerUI = false;
    bool _drawSlideUI = false;

    Editor _currentEditor = null;

    [MenuItem(itemName: "Slider SDK", menuItem = "Slider/Options", priority = 0)]
    public static void ShowWindow()
    {
        GetWindow<SlideEditor>();
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (!_drawNewUI)
        {
            if (GUILayout.Button("Slide Controller Actions",GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2f)))
            {
                _drawNewUI = true;
                _drawSlideControllerUI = true;

            }
            if (GUILayout.Button("Slide Module", GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2f)))
            {
                _drawNewUI = true;
                _drawSlideUI = true;
            }
        }

        EditorGUILayout.EndHorizontal();

        if (_drawSlideUI)
        {
            HandleStepModule();
        }
        else if (_drawSlideControllerUI)
        {
            HandleStepControllerActions();
        }
    }

    

   

    #region Slide Module

    private void HandleStepModule()
    {
        EditorGUILayout.Space();
        if (GUILayout.Button(GUILayoutExtension.BackButtonContent, GUILayoutExtension.SingleLineButtonOptions))
        {
            _drawNewUI = false;
            _drawSlideUI = false;
            _currentEditor = null;
        }
        SlideEditorUI.OnGUI();
    }
    #endregion

    #region Slide Controller Actions
    private void HandleStepControllerActions()
    {
        EditorGUILayout.Space();
        if (GUILayout.Button(GUILayoutExtension.BackButtonContent, GUILayoutExtension.SingleLineButtonOptions))
        {
            _drawNewUI = false;
            _drawSlideControllerUI = false;
            _currentEditor = null;
        }

        SlideControllerUI.OnGUI();
    }


    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class SlideControllerUI : EditorWindow
{
    static SlideController _stepController;
    static List<Slide> _stepsInScene = new List<Slide>();
    static List<Slide> _stepsOfController = new List<Slide>();

    //style
    static GUIStyle guiStyle_RedBold16 = new GUIStyle();
    static GUIStyle guiStyle_GreenBold16 = new GUIStyle();


    //ScrollBar
    static Vector2 scrollPosition = Vector2.zero;


    const int TOP_PADDING = 2;
    const string HELP_TEXT = "Can not find 'Slide(s)' component on any GameObject in the Scene.";

    static Vector2 s_WindowsMinSize = Vector2.one * 300.0f;
    static Rect s_HelpRect = new Rect(0.0f, 0.0f, 300.0f, 100.0f);
    static Rect s_ListRect = new Rect(Vector2.zero, new Vector2(EditorGUIUtility.currentViewWidth, 600.0f));

    static SerializedObject m_StepsSO = null;
    static ReorderableList m_ReordableList = null;

    public static void OnInit()
    {
        _stepController = FindObjectOfType<SlideController>();
        if (_stepController)
        {
            m_StepsSO = new SerializedObject(_stepController);
            m_ReordableList = new ReorderableList(m_StepsSO, m_StepsSO.FindProperty("_slides"), true, true, true, true);

            m_ReordableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Slides");
            m_ReordableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += TOP_PADDING;
                rect.height = EditorGUIUtility.singleLineHeight;
                GUIContent steplabel = new GUIContent(string.Format("Slide {0}", index));
                EditorGUI.PropertyField(rect, m_ReordableList.serializedProperty.GetArrayElementAtIndex(index), steplabel);
            };
        }
    }

    public static void OnGUI()
    {

        s_ListRect.size = new Vector2(EditorGUIUtility.currentViewWidth - 10.0f, 0);

        //-----
        guiStyle_RedBold16.fontSize = 16;
        guiStyle_RedBold16.fontStyle = FontStyle.Bold;
        guiStyle_RedBold16.normal.textColor = Color.red;

        guiStyle_GreenBold16.fontSize = 16;
        guiStyle_GreenBold16.fontStyle = FontStyle.Bold;
        guiStyle_GreenBold16.normal.textColor = Color.green;
        //-----


        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true);


        if (_stepController == null)
        {
            EditorGUILayout.LabelField("Slide Controller NOT selected. Create or Find", guiStyle_RedBold16);
        }
        else
        {
            EditorGUILayout.LabelField("Slide Controller selected : " + _stepController.transform.name, guiStyle_GreenBold16);
        }

        GUILayout.Space(15);



        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("CREATE", GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2f)))
        {


        }
        if (GUILayout.Button("FIND IN SCENE", GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2f)))
        {
            OnInit();
        }
        EditorGUILayout.EndHorizontal();


        if (_stepController)
        {
            if (m_StepsSO != null)
            {
                m_StepsSO.Update();
                m_ReordableList.DoList(s_ListRect);
                m_StepsSO.ApplyModifiedProperties();
            }
            else
            {
                EditorGUI.HelpBox(s_HelpRect, HELP_TEXT, MessageType.Warning);
            }
        }


        GUILayout.EndScrollView();
    }
}

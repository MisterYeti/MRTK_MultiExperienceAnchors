using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class SlidesEditor : Editor
{

}

[CustomEditor(typeof(VideoSlide))]
public class VideoSlideEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        VideoSlide script = (VideoSlide)target;

        script.AutoTransition = EditorGUILayout.Toggle("Auto Transition", script.AutoTransition);
    }
}


[CustomEditor(typeof(SlideController))]
public class SlideControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var myScript = target as SlideController;

       
        if (myScript.GlobalTimeTransition)
            myScript.TransitionDuration = EditorGUILayout.FloatField("Transition Duration", myScript.TransitionDuration);
    }
}
#endif
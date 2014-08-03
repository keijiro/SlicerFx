//
// Slicer FX
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SlicerFx)), CanEditMultipleObjects]
public class SlicerFxEditor : Editor
{
    SerializedProperty propMode;
    SerializedProperty propOrigin;
    SerializedProperty propDirection;
    SerializedProperty propAlbedoFront;
    SerializedProperty propAlbedoBack;
    SerializedProperty propEmission;
    SerializedProperty propInterval;
    SerializedProperty propWidth;
    SerializedProperty propSpeed;

    void OnEnable()
    {
        propMode        = serializedObject.FindProperty("_mode");
        propOrigin      = serializedObject.FindProperty("_origin");
        propDirection   = serializedObject.FindProperty("_direction");
        propAlbedoFront = serializedObject.FindProperty("_albedoFront");
        propAlbedoBack  = serializedObject.FindProperty("_albedoBack");
        propEmission    = serializedObject.FindProperty("_emission");
        propInterval    = serializedObject.FindProperty("_interval");
        propWidth       = serializedObject.FindProperty("_width");
        propSpeed       = serializedObject.FindProperty("_speed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propMode);

        EditorGUI.indentLevel++;

        if (propMode.hasMultipleDifferentValues ||
            propMode.enumValueIndex == (int)SlicerFx.SlicerMode.Directional)
            EditorGUILayout.PropertyField(propDirection);

        if (propMode.hasMultipleDifferentValues ||
            propMode.enumValueIndex == (int)SlicerFx.SlicerMode.Spherical)
            EditorGUILayout.PropertyField(propOrigin);

        EditorGUI.indentLevel--;

        EditorGUILayout.PropertyField(propAlbedoFront, new GUIContent("Albedo"));
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(propAlbedoBack, new GUIContent("(back face)"));
        EditorGUI.indentLevel--;

        EditorGUILayout.PropertyField(propEmission, new GUIContent("Emission"));

        EditorGUILayout.LabelField("Stripe Parameters");
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(propInterval, new GUIContent("Interval"));
        EditorGUILayout.PropertyField(propWidth, new GUIContent("Width"));
        EditorGUILayout.PropertyField(propSpeed, new GUIContent("Scroll Speed"));
        EditorGUI.indentLevel--;
        
        serializedObject.ApplyModifiedProperties();
    }
}

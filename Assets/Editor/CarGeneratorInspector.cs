using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CarGenerator))]
public class CarGeneratorInspector : Editor {

    CarGenerator m_carGenerator;

    private void OnEnable()
    {
        m_carGenerator = target as CarGenerator;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        m_carGenerator.m_MinX = EditorGUILayout.FloatField(m_carGenerator.m_MinX);
        m_carGenerator.m_MaxX = EditorGUILayout.FloatField(m_carGenerator.m_MaxX);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider("Width range", ref m_carGenerator.m_MinX, ref m_carGenerator.m_MaxX, 1, 4);

        EditorGUILayout.BeginHorizontal();
        m_carGenerator.m_MinY = EditorGUILayout.FloatField(m_carGenerator.m_MinY);
        m_carGenerator.m_MaxY = EditorGUILayout.FloatField(m_carGenerator.m_MaxY);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider("Height range", ref m_carGenerator.m_MinY, ref m_carGenerator.m_MaxY, 1, 3);

        EditorGUILayout.BeginHorizontal();
        m_carGenerator.m_MinZ = EditorGUILayout.FloatField(m_carGenerator.m_MinZ);
        m_carGenerator.m_MaxZ = EditorGUILayout.FloatField(m_carGenerator.m_MaxZ);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider("Length range", ref m_carGenerator.m_MinZ, ref m_carGenerator.m_MaxZ, 2, 8);

        EditorGUILayout.BeginHorizontal();
        m_carGenerator.m_WheelMinRadius = EditorGUILayout.FloatField(m_carGenerator.m_WheelMinRadius);
        m_carGenerator.m_WheelMaxRadius = EditorGUILayout.FloatField(m_carGenerator.m_WheelMaxRadius);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider("Wheel radius range", ref m_carGenerator.m_WheelMinRadius, ref m_carGenerator.m_WheelMaxRadius, .2f, .8f);

        if (GUILayout.Button("Generate"))
        {
            m_carGenerator.GenerateCar();
        }
        EditorGUILayout.EndVertical();
    }

}

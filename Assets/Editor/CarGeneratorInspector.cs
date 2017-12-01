using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CarGenerator))]
public class CarGeneratorInspector : Editor {

    CarGenerator carGenerator;

    private void OnEnable()
    {
        carGenerator = target as CarGenerator;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        carGenerator.minX = EditorGUILayout.FloatField(carGenerator.minX);
        carGenerator.maxX = EditorGUILayout.FloatField(carGenerator.maxX);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider("X range", ref carGenerator.minX, ref carGenerator.maxX, 1, 4);

        EditorGUILayout.BeginHorizontal();
        carGenerator.minY = EditorGUILayout.FloatField(carGenerator.minY);
        carGenerator.maxY = EditorGUILayout.FloatField(carGenerator.maxY);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider("Y range", ref carGenerator.minY, ref carGenerator.maxY, 1, 3);

        EditorGUILayout.BeginHorizontal();
        carGenerator.minZ = EditorGUILayout.FloatField(carGenerator.minZ);
        carGenerator.maxZ = EditorGUILayout.FloatField(carGenerator.maxZ);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider("Z range", ref carGenerator.minZ, ref carGenerator.maxZ, 2, 8);

        if (GUILayout.Button("Generate"))
        {
            carGenerator.GenerateCar();
        }
        EditorGUILayout.EndVertical();
    }

    //public void MakeRangeSlider(float min, float max, float leftMin, float leftMax, string label)
    //{
    //    EditorGUILayout.BeginHorizontal();
    //    carGenerator.minZ = EditorGUILayout.FloatField(min);
    //    carGenerator.maxZ = EditorGUILayout.FloatField(max);
    //    EditorGUILayout.EndHorizontal();
    //    EditorGUILayout.MinMaxSlider("Lower Width range", ref min, ref max, leftMin, leftMax);
    //}
}

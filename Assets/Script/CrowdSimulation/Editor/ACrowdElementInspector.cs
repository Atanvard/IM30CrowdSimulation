using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RootMotion.Dynamics;

[CustomEditor(typeof(ACrowdElement))]
public class ACrowdElementInspector : Editor {
    public override void OnInspectorGUI()
    {
        ACrowdElement currentTarget = target as ACrowdElement;

        GUI.skin.label.fontSize = 16;
        GUILayout.Label("Monitor");

        GUI.skin.label.fontSize = 12;
        EditorGUILayout.LabelField("Own agent number", currentTarget.ownAgentNum.ToString());
        if (!currentTarget.GetComponentInChildren<PuppetMaster>())
        {
            EditorGUILayout.LabelField("High Quantity Agent");
        }
        else
        {
            EditorGUILayout.LabelField("High Quality Agent");
        }
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        GUI.skin.label.fontSize = 16;
        GUILayout.Label("Parameter");
        currentTarget.bRandomSpeed = EditorGUILayout.ToggleLeft("Whether need random speed", currentTarget.bRandomSpeed);
        if (currentTarget.bRandomSpeed)
        {
            EditorGUILayout.LabelField("Min random speed", currentTarget.minRandomSpeed.ToString());
            EditorGUILayout.LabelField("Max random speed", currentTarget.maxRandomSpeed.ToString());
            EditorGUILayout.MinMaxSlider(ref currentTarget.minRandomSpeed, ref currentTarget.maxRandomSpeed, 0, 100);
        }
        currentTarget.navStopDistance = EditorGUILayout.FloatField("Stop distance", currentTarget.navStopDistance);
        currentTarget.animationScale = EditorGUILayout.FloatField("Move animation scale", currentTarget.animationScale);
        //base.DrawDefaultInspector();
    }
}

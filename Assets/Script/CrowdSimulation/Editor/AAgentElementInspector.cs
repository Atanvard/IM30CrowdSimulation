using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AAgentElement))]
public class AAgentElementInspector : Editor
{
    public override void OnInspectorGUI()
    {
        AAgentElement currentTarget = target as AAgentElement;

        GUI.skin.label.fontSize = 16;
        GUILayout.Label("Monitor");

        GUI.skin.label.fontSize = 12;
        EditorGUILayout.LabelField("Current state", currentTarget.currentAgentState.ToString());
        EditorGUILayout.LabelField("Current destination", currentTarget.agentNavDestination.ToString());

    }
}

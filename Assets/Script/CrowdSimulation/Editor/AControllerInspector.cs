using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AController))]
public class AControllerInspector : Editor
{
    public ControllerType controllerType;
    public override void OnInspectorGUI()
    {
        GUI.skin.label.fontSize = 16;
        AController currentTarget = target as AController;
        GUILayout.BeginVertical();
        GUILayout.Label("Type");
        currentTarget.enable = EditorGUILayout.ToggleLeft("Enable", currentTarget.enable);
        currentTarget.controllerType =  (ControllerType)EditorGUILayout.EnumPopup("Controller type", currentTarget.controllerType);
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        GUILayout.Label("Detail");
        currentTarget.nextCrowdState = (AgentState)EditorGUILayout.EnumPopup("Next animation state", currentTarget.nextCrowdState);
        if(currentTarget.controllerType == ControllerType.Range)
        {
            currentTarget.affectPercent = EditorGUILayout.Slider("Affect crowd percent", currentTarget.affectPercent, 0, 100);
            currentTarget.delayTime =  EditorGUILayout.Slider("Delay start time(sec)", currentTarget.delayTime, 0, 10);
            currentTarget.durationTime =  EditorGUILayout.Slider("During time(sec)", currentTarget.durationTime, 0, 100);
            currentTarget.newVelocityScale = EditorGUILayout.FloatField("New velocity scale", currentTarget.newVelocityScale);
            currentTarget.bKill = EditorGUILayout.ToggleLeft("Whether kill agent", currentTarget.bKill);
        }
        else if(currentTarget.controllerType == ControllerType.Attach)
        {
            currentTarget.newVelocityScale = EditorGUILayout.FloatField("New velocity scale", currentTarget.newVelocityScale);
            currentTarget.durationTime = EditorGUILayout.Slider("During time(sec)", currentTarget.durationTime, 0, 100);
            currentTarget.bKill = EditorGUILayout.ToggleLeft("Whether kill agent", currentTarget.bKill);
        }
        else if(currentTarget.controllerType == ControllerType.TempleteExplose)
        {
            currentTarget.exploseForce =  EditorGUILayout.FloatField("Explose force", currentTarget.exploseForce);
            //currentTarget.exploseRadius =  EditorGUILayout.FloatField("Explose radius", currentTarget.exploseRadius);
            currentTarget.durationTime = EditorGUILayout.Slider("During time(sec)", currentTarget.durationTime, 0, 100);
            currentTarget.bKill = EditorGUILayout.ToggleLeft("Whether kill agent", currentTarget.bKill);
        }
        else if(currentTarget.controllerType == ControllerType.TempleteColider)
        {
            currentTarget.bKill = EditorGUILayout.ToggleLeft("Whether kill agent", currentTarget.bKill);
        }
        //base.DrawDefaultInspector();
    }
}

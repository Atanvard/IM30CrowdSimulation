using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AController))]

[CanEditMultipleObjects]
public class AControllerInspector : Editor
{
    public ControllerType controllerType;
    SerializedProperty m_crowsProperty;
    void OnEnable()
    {
    }
    public override void OnInspectorGUI()
    {
        GUI.skin.label.fontSize = 16;
        AController currentTarget = target as AController;
        GUILayout.BeginVertical();
        GUILayout.Label("Type");
        currentTarget.enable = EditorGUILayout.ToggleLeft("Enable", currentTarget.enable);
        currentTarget.controllerType =  (ControllerType)EditorGUILayout.EnumPopup("Controller type", currentTarget.controllerType);
        m_crowsProperty = serializedObject.FindProperty("affectCrowds");
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_crowsProperty, true);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        GUILayout.Label("Detail");
        currentTarget.nextCrowdState = (AgentState)EditorGUILayout.EnumPopup("Next animation state", currentTarget.nextCrowdState);

        if (currentTarget.controllerType != ControllerType.TempleteExplose)
            currentTarget.bKill = EditorGUILayout.ToggleLeft("Whether kill agent", currentTarget.bKill);
        currentTarget.bDestroy = EditorGUILayout.ToggleLeft("Whether destroy agent", currentTarget.bDestroy);
        if (currentTarget.bDestroy)
        {
            ++EditorGUI.indentLevel;
            currentTarget.destroyDelayTime = EditorGUILayout.Slider("Destroy delay time(sec)", currentTarget.destroyDelayTime, 0, 30);
            --EditorGUI.indentLevel;
        }
        EditorGUILayout.Separator();
        if (currentTarget.controllerType == ControllerType.Range)
        {
            currentTarget.affectPercent = EditorGUILayout.Slider("Affect crowd percent", currentTarget.affectPercent, 0, 100);
            currentTarget.delayTime =  EditorGUILayout.Slider("Delay start time(sec)", currentTarget.delayTime, 0, 10);
            currentTarget.durationTime =  EditorGUILayout.Slider("During time(sec)", currentTarget.durationTime, 0.1f, 300);
            currentTarget.bSetNewSpeed = EditorGUILayout.ToggleLeft("Set new speed", currentTarget.bSetNewSpeed);
            if (currentTarget.bSetNewSpeed)
            {
                EditorGUILayout.LabelField("Min random speed", currentTarget.newMinSpeed.ToString());
                EditorGUILayout.LabelField("Max random speed", currentTarget.newMaxSpeed.ToString());
                EditorGUILayout.MinMaxSlider(ref currentTarget.newMinSpeed, ref currentTarget.newMaxSpeed, 0, 100);
            }
        }
        else if(currentTarget.controllerType == ControllerType.Attach)
        {
            currentTarget.bSetNewSpeed = EditorGUILayout.ToggleLeft("Set new speed", currentTarget.bSetNewSpeed);
            if (currentTarget.bSetNewSpeed)
            {
                EditorGUILayout.LabelField("Min random speed", currentTarget.newMinSpeed.ToString());
                EditorGUILayout.LabelField("Max random speed", currentTarget.newMaxSpeed.ToString());
                EditorGUILayout.MinMaxSlider(ref currentTarget.newMinSpeed, ref currentTarget.newMaxSpeed, 0, 100);
            }
            currentTarget.durationTime = EditorGUILayout.Slider("During time(sec)", currentTarget.durationTime, 0.1f, 300);
        }
        else if(currentTarget.controllerType == ControllerType.TempleteExplose)
        {
            currentTarget.exploseForce =  EditorGUILayout.FloatField("Explose force", currentTarget.exploseForce);
            //currentTarget.exploseRadius =  EditorGUILayout.FloatField("Explose radius", currentTarget.exploseRadius);
            currentTarget.durationTime = EditorGUILayout.Slider("During time(sec)", currentTarget.durationTime, 0, 300);
        }
        else if(currentTarget.controllerType == ControllerType.TempleteColider)
        {
        }
        //base.DrawDefaultInspector();
    }
}

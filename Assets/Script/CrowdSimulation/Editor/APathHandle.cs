using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(APath))]
public class APathHandle : Editor {
    private void OnSceneGUI()
    {
        APath currentTarget = target as APath;

        for(int j = 0; j < currentTarget.ownPathInfoList.Count; j++)
        {
            for(int i = 0; i < currentTarget.ownPathInfoList[j].position.Length;i++) {
                currentTarget.ownPathInfoList[j].position[i] = Handles.PositionHandle(currentTarget.ownPathInfoList[j].position[i], Quaternion.identity);
                Handles.DrawPolyLine(currentTarget.ownPathInfoList[j].position);
            }
        }
    }
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
    }
}

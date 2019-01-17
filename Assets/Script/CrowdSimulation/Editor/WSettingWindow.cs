using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CrowdSimulationWindow
{
    public enum CrowdFormationType
    {
        Rect, Round
    }


    [ExecuteInEditMode]
    public class WSettingWindow : EditorWindow
    {
        public static string[] options = new string[]
        {
            "Rect", "Round"
        };
        
        Vector2 scrollPosition;
        CrowdFormationType m_currentFormationType = 0;

        /// <summary>
        /// 方阵信息
        /// </summary>
        private List<FormationItem> soldierInfoList = new List<FormationItem>();

        /// <summary>
        /// 创建节点
        /// </summary>
        internal GameObject rootObj;

        [MenuItem("Im30/Temp")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            WSettingWindow window = (WSettingWindow)EditorWindow.GetWindow(typeof(WSettingWindow), false, "Crowd Simulation Setting");
            window.Show();
        }

        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            //create formation select field
            createFormationSelect();
            EditorGUILayout.EndScrollView();
        }

        
        private void createFormationSelect()
        {
            GUI.skin.label.fontSize = 16;
            GUILayout.Label("Crowd Setting", EditorStyles.boldLabel);
            m_currentFormationType = (CrowdFormationType)EditorGUILayout.Popup("Crowd Formation", (int)m_currentFormationType, options);
            var selectObj = (GameObject)EditorGUILayout.ObjectField("Crowd Root", rootObj, typeof(GameObject), true);
            EditorGUILayout.Separator();
            FormationItem.createFormation(this, m_currentFormationType);
            if (GUILayout.Button("Create"))
            {
                createFormation();
            }
        }

        /// <summary>
        /// 创建编队
        /// </summary>
        private void createFormation()
        {
            var valid = checkValid();
            if (!valid)
            {
                ShowNotification(new GUIContent("参数错误， 无法生成"));
            }
            else
            {
                // start create item

                //clear old node


                List<Transform> allChildren = new List<Transform>();
                foreach (Transform child in rootObj.transform)
                {
                    allChildren.Add(child);
                }

                foreach (Transform child in allChildren)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }

                Vector3 startPoint = Vector3.zero; ;

                foreach (FormationItem itemInfo in soldierInfoList)
                {
                    if (!itemInfo.createFormation(ref startPoint))
                    {
                        ShowNotification(new GUIContent("未知错误， 暂停生成"));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 校验
        /// </summary>
        private bool checkValid()
        {
            bool valid = true;
            if (rootObj == null)
            {
                valid = false;
                ShowNotification(new GUIContent("无根节点，生成失败"));
            }
            

            return valid;
        }




    }


}
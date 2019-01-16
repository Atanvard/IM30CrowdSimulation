using System;
using System.Collections.Generic;
using System.Linq;
using FormationSetup;
using JetBrains.Annotations;
//using Unity.Collections;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace FormationSetup
{
    public enum FormationType
    {
        Trangle,
        Round,
        Rect,
    }
       
    
    public class FormationSetupWindow : EditorWindow
    {
        public static string[] options = new string[3]
        {
            "三角形方阵", "半弧形方阵", "矩形方阵", 
        };
        
        string myString = "1";
        bool groupEnabled;
        bool myBool = true;
        float myFloat = 1.23f;
        private Vector2 scrollPosition;
        
        /// <summary>
        /// 方阵类型选择
        /// </summary>
        private FormationType formationSelect = 0;
        
        /// <summary>
        ///方阵数量
        /// </summary>
        private int soldierListCount = 1;

        /// <summary>
        /// 方阵信息
        /// </summary>
        private List<FormationItem> soldierInfoList = new List<FormationItem>();

        /// <summary>
        /// 创建节点
        /// </summary>
        internal GameObject rootObj;
        // Add menu named "My Window" to the Window menu
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            FormationSetupWindow window = (FormationSetupWindow)EditorWindow.GetWindow(typeof(FormationSetupWindow), false, "方针配置");
            window.Show();
           
        }

        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            GUILayout.Label("方阵配置", EditorStyles.boldLabel);
            
            //create formation select field
            createFormationSelect();
            
            //add soldier list;
            createSoldierListField();
            
            //刷新soldierInfo;
            for(int i = 0; i < soldierListCount; i++)
                createSoldierItem(i);
            
            //清理info
            if (soldierInfoList.Count > soldierListCount)
            {
                soldierInfoList.RemoveRange(soldierListCount, soldierInfoList.Count - soldierListCount);
            }
            
            //生成方阵
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            var obj = Selection.activeGameObject;
            if (obj != null && !EditorUtility.IsPersistent(obj))
            {
                rootObj = obj;
            }

            var selectObj = (GameObject)EditorGUILayout.ObjectField("生成根节点", rootObj, typeof(GameObject), true);
            
            if (selectObj != null && !EditorUtility.IsPersistent(selectObj))
            {
                rootObj = selectObj;
            }

            if (GUILayout.Button("生成方阵"))
            {
                createFormation();
            }
            EditorGUILayout.EndScrollView();
        }


        
        /// <summary>
        /// 编队类型选择
        /// </summary>
        private void createFormationSelect()
        {
            formationSelect = (FormationType)EditorGUILayout.Popup("方阵类型", (int)formationSelect, options); 
        }
        
        /// <summary>
        /// 编队数量选择
        /// </summary>
        private void createSoldierListField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("方阵数量");
            
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                --soldierListCount;
                soldierListCount = Math.Max(0, soldierListCount);
            };
            
            GUILayout.TextArea(soldierListCount.ToString(), GUILayout.Width(60));
            
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                ++soldierListCount;
            }
            
            GUILayout.EndHorizontal();;
        }
        
        /// <summary>
        /// 编队条目选择
        /// </summary>
        /// <param name="i"></param>
        private void createSoldierItem(int i)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if(soldierInfoList.Count <= i)
                soldierInfoList.Add(FormationItem.createFormation(this, i, formationSelect));
            soldierInfoList[i].OnGUI();
            
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

                Vector3 startPoint = Vector3.zero;;
                
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

            for (int i = 0; i < soldierListCount; i++)
            {
                if (!soldierInfoList[i].checkValid())
                {
                    valid = false;
                }
            }

            return valid;
        }

        
        
        
    }

    
}
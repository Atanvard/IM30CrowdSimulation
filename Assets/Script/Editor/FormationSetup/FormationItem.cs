// create by liudi

using System;
using UnityEditor;
using UnityEngine;

namespace FormationSetup
{
    //编队条目

    public class FormationItem
    {
        private int mIndex = -1;
        protected FormationType mFormationType;
        protected GameObject itemPrefab = null;
        protected FormationSetupWindow mParent;
        protected float m_rowIntervalDis = 10;
        protected float m_columnIntervalDis = 10;
        protected Boolean m_bInOrder = true;


        static public FormationItem createFormation(FormationSetupWindow parent, int index, FormationType formationType)
        {
            switch (formationType)
            {
                    case FormationType.Trangle:
                        return new TrangleFormation(parent, index, formationType);
                    case FormationType.Round:
                        return new RoundFormation(parent, index, formationType);
                    case FormationType.Rect:
                        return new RectFormation(parent, index, formationType);
            }

            return null;
        }

        public FormationItem(FormationSetupWindow parent, int index, FormationType formationType)
        {
            mParent = parent;
            mIndex = index;
            mFormationType = formationType;
        }

        protected FormationItem()
        {
            mParent = null;
            mIndex = -1;
        }

        public void OnGUI()
        {

            createNormal();
            createSpecial();

        }

        private void createNormal()
        {
            GUILayout.Label("编队" + (mIndex + 1) + ": " + FormationSetupWindow.options[(int) mFormationType]);
            var obj = EditorGUILayout.ObjectField(
                "资源Prefab",
                itemPrefab,
                typeof(GameObject), false);
            if (obj != null && PrefabUtility.GetPrefabType(obj) == PrefabType.Prefab)
            {
                // prefab valid
                itemPrefab = (GameObject) obj;
            }
            else if (obj == null)
            {
                itemPrefab = null;
            }
            else
            {
                mParent.ShowNotification(new GUIContent("资源格式不正确"));
                itemPrefab = null;
            }

            m_rowIntervalDis = EditorGUILayout.FloatField("行间距", m_rowIntervalDis);
            m_columnIntervalDis = EditorGUILayout.FloatField("列间距", m_columnIntervalDis);
            m_bInOrder = EditorGUILayout.Toggle("整齐队列", m_bInOrder);
        }

        protected virtual void createSpecial()
        {
            
        }
        
        public virtual bool checkValid()
        {
            bool valid = true;
            if (itemPrefab == null)
            {
                valid = false;
                mParent.ShowNotification(new GUIContent("编队" + mIndex + "资源错误"));
            }

            return valid;
        }

        public virtual bool createFormation(ref Vector3 point)
        {
            return false;
        }
    }

}
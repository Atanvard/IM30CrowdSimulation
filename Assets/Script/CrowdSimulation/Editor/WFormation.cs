using System;
using UnityEditor;
using UnityEngine;

namespace CrowdSimulationWindow
{
    public class FormationItem
    {
        private int mIndex = -1;
        protected CrowdFormationType mFormationType;
        protected GameObject itemPrefab = null;
        protected WSettingWindow mParent;
        protected float m_rowIntervalDis = 10;
        protected float m_columnIntervalDis = 10;
        protected Boolean m_bInOrder = true;


        static public FormationItem createFormation(WSettingWindow parent,  CrowdFormationType formationType)
        {
            switch (formationType)
            {
                case CrowdFormationType.Rect:
                    return new RectFormation(parent, formationType);
                case CrowdFormationType.Round:
                    return new RoundFormation(parent, formationType);
            }

            return null;
        }

        public FormationItem(WSettingWindow parent,  CrowdFormationType formationType)
        {
            mParent = parent;
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
           var obj = EditorGUILayout.ObjectField(
                "Prefab",
                itemPrefab,
                typeof(GameObject), false);
            if (obj != null && PrefabUtility.GetPrefabType(obj) == PrefabType.Prefab)
            {
                // prefab valid
                itemPrefab = (GameObject)obj;
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

            m_bInOrder = EditorGUILayout.Toggle("In order", m_bInOrder);
            m_rowIntervalDis = EditorGUILayout.FloatField("Row interval distance", m_rowIntervalDis);
            m_columnIntervalDis = EditorGUILayout.FloatField("Column interval distance", m_columnIntervalDis);
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
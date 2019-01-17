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
        protected float mLineSpace = 10;
        protected float mColumnSpace = 10;
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
            GUILayout.Label("编队" + (mIndex + 1) + ": " + WSettingWindow.options[(int)mFormationType]);
            var obj = EditorGUILayout.ObjectField(
                "资源Prefab",
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

            mLineSpace = EditorGUILayout.FloatField("行间距", mLineSpace);
            mColumnSpace = EditorGUILayout.FloatField("列间距", mColumnSpace);
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
// create by liudi

using UnityEditor;
using UnityEngine;

namespace CrowdSimulationWindow
{
    public class RectFormation : FormationItem
    {
        private int mRectLineCount;
        private int mRectRow;
        public RectFormation(WSettingWindow parent, CrowdFormationType formationType) : base(parent, formationType)
        {
        }

        public override bool checkValid()
        {
            if (mRectRow == 0 || mRectLineCount == 0)
                return false;
            return base.checkValid();
        }

        public override bool createFormation(ref Vector3 startPoint)
        {
            if (itemPrefab == null)
                return false;

            
            for (int i = 0; i < mRectRow; i++)
            {
                Vector3 itemPoint = startPoint;
                itemPoint.x -= i * mColumnSpace;
                itemPoint.z -= mRectLineCount / 2 * mLineSpace;
                if (mRectLineCount % 2 == 0)
                {
                    itemPoint.z += mLineSpace / 2;
                }

                for (int j = 0; j < mRectLineCount; j++)
                {
                    var newObject = GameObject.Instantiate(itemPrefab);
                    newObject.transform.parent = mParent.rootObj.transform;
                    newObject.transform.localPosition = itemPoint + (m_bInOrder ? Vector3.zero : new Vector3(Random.Range(-mColumnSpace, mColumnSpace), 0, Random.Range(-mLineSpace, mLineSpace)));
                    newObject.name = itemPrefab.name;
                    itemPoint.z += mLineSpace;
                }
            }
            startPoint.x -= mRectRow * mColumnSpace;
            return true;
        }

        protected override void createSpecial()
        {
            mRectLineCount = EditorGUILayout.IntField("每行数量", mRectLineCount);
            mRectRow = EditorGUILayout.IntField("总行数", mRectRow);
            base.createSpecial();
        }
    }
}
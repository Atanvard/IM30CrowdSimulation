// create by liudi

using UnityEditor;
using UnityEngine;

namespace FormationSetup
{
    public class RectFormation : FormationItem
    {
        private int mRectLineCount;
        private int mRectRow;
        public RectFormation(FormationSetupWindow parent, int index, FormationType formationType) : base(parent, index, formationType)
        {
        }

        public override bool checkValid()
        {
            if (mRectRow == 0 || mRectLineCount == 0)
                return false;
            return base.checkValid();
        }

        //public override bool createFormation(ref Vector3 startPoint)
        //{
        //    if (itemPrefab == null)
        //        return false;

        //    if (mRectRow / 2 != 0)
        //    {
        //        mRectRow -= 1;
        //    }
        //    for (int i = 0; i < mRectRow/2; i++)
        //    {
        //        Vector3 itemPoint = startPoint;
        //        itemPoint.x -= i * m_columnIntervalDis;
        //        itemPoint.z += mRectLineCount / 2 * m_rowIntervalDis;
        //        if (mRectLineCount % 2 == 0)
        //        {
        //            itemPoint.z -= m_rowIntervalDis / 2;
        //        }

        //        for (int j = 0; j < mRectLineCount*2-1; j++)
        //        {
        //            var newObject = GameObject.Instantiate(itemPrefab);
        //            newObject.transform.parent = mParent.rootObj.transform;
        //            newObject.transform.localPosition = itemPoint + (m_bInOrder ? Vector3.zero:new Vector3(Random.Range(-m_columnIntervalDis, m_columnIntervalDis), 0, Random.Range(-m_rowIntervalDis, m_rowIntervalDis)));
        //            newObject.name = itemPrefab.name;
        //            itemPoint.z += m_rowIntervalDis;
        //        }
        //    }
        //    startPoint.x -= mRectRow * m_columnIntervalDis;
        //    return true;
        //}
        public override bool createFormation(ref Vector3 startPoint)
        {
            if (itemPrefab == null)
                return false;


            for (int i = 0; i < mRectRow; i++)
            {
                Vector3 itemPoint = startPoint;
                itemPoint.x -= i * m_columnIntervalDis;
                itemPoint.z -= mRectLineCount / 2 * m_rowIntervalDis;
                if (mRectLineCount % 2 == 0)
                {
                    itemPoint.z += m_rowIntervalDis / 2;
                }

                for (int j = 0; j < mRectLineCount; j++)
                {
                    var newObject = GameObject.Instantiate(itemPrefab);
                    newObject.transform.parent = mParent.rootObj.transform;
                    newObject.transform.localPosition = itemPoint + (m_bInOrder ? Vector3.zero : new Vector3(Random.Range(-m_columnIntervalDis, m_columnIntervalDis), 0, Random.Range(-m_rowIntervalDis, m_rowIntervalDis)));
                    newObject.name = itemPrefab.name;
                    itemPoint.z += m_rowIntervalDis;
                }
            }
            startPoint.x -= mRectRow * m_columnIntervalDis;
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
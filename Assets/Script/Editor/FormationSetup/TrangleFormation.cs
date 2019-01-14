// create by liudi

using UnityEditor;
using UnityEngine;

namespace FormationSetup
{
    public class TrangleFormation : FormationItem
    {
        //参数
        private int mTrangleStartCount = 0;
        private int mTrangleAddCount = 1;
        private int mTrangleTotalRows;

        public TrangleFormation(FormationSetupWindow parent, int index, FormationType formationType) : base(parent, index, formationType)
        {
        }

        protected override void createSpecial()
        {
            mTrangleStartCount = EditorGUILayout.IntField("首行数量", mTrangleStartCount);
            mTrangleAddCount = EditorGUILayout.IntField("每行递增", mTrangleAddCount);
            mTrangleTotalRows = EditorGUILayout.IntField("总行数", mTrangleTotalRows);
            base.createSpecial();
        }

        public override bool checkValid()
        {
            if (mTrangleStartCount == 0 && mTrangleAddCount == 0 || mTrangleTotalRows == 0)
                return false;
            return base.checkValid();
        }

        //方阵函数
        public override bool createFormation(ref Vector3 startPoint)
        {
            if (itemPrefab == null)
                    return false;

            var lineCount = mTrangleStartCount;
            Debug.Log("Test Create Data:" + mTrangleAddCount + mTrangleStartCount + mTrangleTotalRows);

            for (int i = 0; i < mTrangleTotalRows; i++)
            {
                Vector3 itemPoint = startPoint;
                itemPoint.x -= i * mColumnSpace;
                itemPoint.z -= lineCount / 2 * mLineSpace;
                if (lineCount % 2 == 0)
                {
                    itemPoint.z += mLineSpace / 2;
                }

                for (int j = 0; j < lineCount; j++)
                {
//                        var newObject =(GameObject)PrefabUtility.InstantiatePrefab(itemPrefab);
//                        if (!newObject)
//                            break;
                    var newObject = GameObject.Instantiate(itemPrefab);
                    newObject.transform.parent = mParent.rootObj.transform;
                    newObject.transform.localPosition = itemPoint;
                    newObject.name = itemPrefab.name;
                    itemPoint.z += mLineSpace;
                }

                lineCount += mTrangleAddCount;

            }

            startPoint.x -= mTrangleTotalRows * mColumnSpace;

            return true;
        }
    }
}
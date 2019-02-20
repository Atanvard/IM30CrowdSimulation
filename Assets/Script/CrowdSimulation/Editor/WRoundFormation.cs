// create by liudi

using UnityEditor;
using UnityEngine;

namespace CrowdSimulationWindow
{
    public class RoundFormation : FormationItem
    {
        private int mRoundRows;
        private int mRoundStartCount;
        private int mRoundAddCount;
        private float mRoundDegrees = 10;
        
        public RoundFormation(WSettingWindow parent, CrowdFormationType formationType) : base(parent, formationType)
        {
        }

        public override bool checkValid()
        {
            if (mRoundRows == 0 || mRoundStartCount == 0 && mRoundAddCount == 0)
                return false;
            return base.checkValid();
        }

        public override bool createFormation(ref Vector3 startPoint)
        {
            if (itemPrefab == null)
                return false;
            
            var lineCount = mRoundStartCount;
            
            for (int i = 0; i < mRoundRows; i++)
            {
                Vector3 itemPoint = startPoint;
                itemPoint.x -= i * m_columnIntervalDis;
                itemPoint.z -= lineCount / 2 * m_rowIntervalDis;
                var roundDer =  - mRoundDegrees * lineCount / 2;
                if (lineCount % 2 == 0)
                {
                    itemPoint.z += m_rowIntervalDis / 2;
                    roundDer += mRoundDegrees / 2;
                }
                


                for (int j = 0; j < lineCount; j++)
                {

                    var newObject = GameObject.Instantiate(itemPrefab);
                    newObject.transform.parent = mParent.rootObj.transform;
                    
                    //角度计算
                    var baseX = itemPoint.x;
                    var roundX = Mathf.Abs(itemPoint.z - 0) * Mathf.Tan(Mathf.Deg2Rad * (Mathf.Abs(roundDer) / 2));
                    itemPoint.x += roundX;
                    newObject.transform.localPosition = itemPoint;
                    itemPoint.x = baseX;
                    itemPoint.z += m_rowIntervalDis;
                    roundDer += mRoundDegrees;
                }

                lineCount += mRoundAddCount;

            }

            startPoint.x -= mRoundRows * m_columnIntervalDis;
            
            
            return true;
        }

        protected override void createSpecial()
        {
            mRoundStartCount = EditorGUILayout.IntField("首行数量", mRoundStartCount);
            mRoundAddCount = EditorGUILayout.IntField("每行递增", mRoundAddCount);
            mRoundRows = EditorGUILayout.IntField("总行数", mRoundRows);
            mRoundDegrees = EditorGUILayout.FloatField("弧度(0 为横向直线， 90为纵向直线 )", mRoundDegrees);
            base.createSpecial();
        }
    }
}
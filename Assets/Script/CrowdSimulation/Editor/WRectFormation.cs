// create by liudi

using UnityEditor;
using UnityEngine;

namespace CrowdSimulationWindow
{
    public class RectFormation : FormationItem
    {
        private int m_firstRowCount = 1;
        private int m_eachRowIncreate = 0;
        private int m_columnCount = 1;
        public RectFormation(WSettingWindow parent, CrowdFormationType formationType) : base(parent, formationType)
        {
        }

        public override bool checkValid()
        {
            if (m_columnCount == 0 || m_firstRowCount == 0)
                return false;
            return base.checkValid();
        }

        public override bool createFormation(ref Vector3 startPoint)
        {
            if (itemPrefab == null)
                return false;

            
            for (int i = 0; i < m_columnCount; i++)
            {
                Vector3 itemPoint = startPoint;
                itemPoint.x -= i * m_columnIntervalDis;
                itemPoint.z -= m_firstRowCount / 2 * m_rowIntervalDis;
                if (m_firstRowCount % 2 == 0)
                {
                    itemPoint.z += m_rowIntervalDis / 2;
                }

                for (int j = 0; j < m_firstRowCount; j++)
                {
                    var newObject = GameObject.Instantiate(itemPrefab);
                    newObject.transform.parent = mParent.rootObj.transform;
                    newObject.transform.localPosition = itemPoint + (m_bInOrder ? Vector3.zero : new Vector3(Random.Range(-m_columnIntervalDis, m_columnIntervalDis), 0, Random.Range(-m_rowIntervalDis, m_rowIntervalDis)));
                    newObject.name = itemPrefab.name;
                    itemPoint.z += m_rowIntervalDis;
                }
                m_firstRowCount += m_eachRowIncreate;
            }
            startPoint.x -= m_columnCount * m_columnIntervalDis;
            return true;
        }

        protected override void createSpecial()
        {
            m_firstRowCount = EditorGUILayout.IntField("First row count", m_firstRowCount);
            m_firstRowCount = EditorGUILayout.IntField("Each row increate", m_eachRowIncreate);
            m_columnCount = EditorGUILayout.IntField("Column count", m_columnCount);
            base.createSpecial();
        }
    }
}
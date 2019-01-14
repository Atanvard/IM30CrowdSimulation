using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct PathInfo
{
    public bool enable;
    public Vector3[] position;
}
public class APath : MonoBehaviour {
    public List<PathInfo> ownPathInfoList;
    int m_totalPathCount;
    int m_activedpathCount;
    public ACrowdElement[] affectCrowdElement;
    // Use this for initialization

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        foreach (var pathInfo in ownPathInfoList)
        {
            if (pathInfo.position.Length > 1)
            {
                for (int i = 0; i < pathInfo.position.Length - 1; i++)
                {
                    Gizmos.DrawWireSphere(pathInfo.position[i], 1f);
                    Gizmos.DrawLine(pathInfo.position[i], pathInfo.position[i+1]);
                }
                Gizmos.DrawWireSphere(pathInfo.position[pathInfo.position.Length-1], 1f);
            }
            else if(pathInfo.position.Length == 1)
            {
                Gizmos.DrawWireSphere(pathInfo.position[0], 1f);
            }
        }
    }
    void Start () {
        m_totalPathCount = ownPathInfoList.Count;
        m_activedpathCount = 0;

    }
	
	// Update is called once per frame
	void Update () {
		if(m_activedpathCount < m_totalPathCount)
        {
            if (ownPathInfoList[m_activedpathCount].enable)
            {
                foreach(var crowd in affectCrowdElement)
                {
                    crowd.SetNewCrowdDestination(ownPathInfoList[m_activedpathCount].position);
                    //Debug.Log(ownPathInfoList[m_activedpathCount].position[0]);
                    ++m_activedpathCount;
                }
            }
        }
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct PathInfo
{
    //public bool enable;
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
        for(int j=0;j< m_totalPathCount; j++)
        {
            var list = ownPathInfoList[j];
            int length = list.position.Length;
            for(int i = 0; i < length - 1; i++)
            {
                GameObject obj = new GameObject();
                obj.AddComponent<BoxCollider>();
                obj.GetComponent<BoxCollider>().isTrigger = true;
                obj.AddComponent<APathTrigger>();
                obj.GetComponent<APathTrigger>().ID = j;

                obj.transform.parent = this.transform;
                obj.transform.position = Vector3.Lerp(list.position[i], list.position[i+1], 0.5f);
                obj.transform.LookAt(list.position[i]);
                float tmpZ = Vector3.Distance(list.position[i], list.position[i + 1]);
                if (tmpZ != 0)
                {
                    obj.transform.localScale = new Vector3(3, 3, tmpZ);
                }
            }
        }
        Invoke("SetFirstDestination", 1f);

    }
    void SetFirstDestination()
    {
        foreach (var crowd in affectCrowdElement)
            crowd.SetNewCrowdDestination(ownPathInfoList[0].position);
        m_activedpathCount = 0;
    }
	public bool SetDeatination(int ID)
    {
        if(ID == m_activedpathCount)
        {
            foreach (var crowd in affectCrowdElement)
            {
                crowd.SetNewCrowdDestination(ownPathInfoList[ID+1].position);
            }
            if(m_activedpathCount<m_totalPathCount-2)
                m_activedpathCount++;
            return true;
        }
        else
            return false;
    }
	// Update is called once per frame
	void Update () {
		//if(m_activedpathCount < m_totalPathCount)
  //      {
  //          if (ownPathInfoList[m_activedpathCount].enable)
  //          {
  //              foreach(var crowd in affectCrowdElement)
  //              {
  //                  crowd.SetNewCrowdDestination(ownPathInfoList[m_activedpathCount].position);
  //              }
  //              ++m_activedpathCount;
  //          }
  //      }
	}
}

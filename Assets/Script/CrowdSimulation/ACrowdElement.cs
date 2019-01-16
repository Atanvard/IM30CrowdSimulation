using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RootMotion.Dynamics;
public class ACrowdElement : MonoBehaviour
{
    private AAgentElement[] m_ownAgents;
    private List<Vector3> m_currentTargetPositionList;
    private List<AAgentElement> m_liveAgentList;

    public bool bAnimator = false;
    public int ownAgentNum
    {
        get
        {
#if UNITY_EDITOR
            return this.GetComponentsInChildren<AAgentElement>().Length;
#else
            return m_ownAgents.Length;
#endif
        }
    }
    public float navStopDistance = 10f;
    public bool bRandomSpeed;
    public float minRandomSpeed;
    public float maxRandomSpeed;
    public float animationScale = 0.1f;
    // Use this for initialization
    void Start () {
        m_ownAgents = GetComponentsInChildren<AAgentElement>();
        m_currentTargetPositionList = new List<Vector3>();
        m_liveAgentList = new List<AAgentElement>(m_ownAgents);
        if (GetComponentInChildren<PuppetMaster>())
        {
            bAnimator = true;
        }
        //Initialization();
	}
    void Initialization()
    {

    }
    public bool SetNewCrowdDestination(Vector3[] p)
    {
        if (p.Length == 0)
        {
            Debug.Log("current path's position is null");
            return false;
        }
        m_currentTargetPositionList = new List<Vector3>(p);

        if (m_ownAgents.Length == 0)
        {
            Debug.Log("this crowd:" + this.name + " doesn't own agent");
            return false;
        }

        int destinationPointCount = m_currentTargetPositionList.Count;
        int totalNum = m_liveAgentList.Count;

        Vector3[] tmpDes = new Vector3[m_liveAgentList.Count];
        bool bLock = false;
        Loom.RunAsync(() =>
        {
            for (int i = 0; i < totalNum; i++)
            {
                float tmp = (float)i / (float)totalNum * (destinationPointCount - 1);
                int t = (int)tmp;
                if (destinationPointCount > 1)
                {
                    tmpDes[i] = Vector3.Lerp(m_currentTargetPositionList[t], m_currentTargetPositionList[t + 1], tmp - t);
                }
                else
                {
                    tmpDes[i] = m_currentTargetPositionList[0];
                }
            }
            bLock = true;
        }
        );
            Loom.QueueOnMainThread(() =>
            {
                while (!bLock)
                {
                    Debug.Log("waiting subThread");
                }
                Debug.Log(totalNum);
                for (int i = 0; i < totalNum; i++)
                {
                    m_liveAgentList[i].ChangeStateImmediate(AgentState.Move);
                    //m_liveAgentList[i].SetAnimationClip("Move");
                    m_liveAgentList[i].SetNavDestination(tmpDes[i]);
                    if (bRandomSpeed)
                        m_liveAgentList[i].SetNavMeshAgentSpeed(UnityEngine.Random.Range(minRandomSpeed, maxRandomSpeed));
                }
            }
            );

        return true;
    }

    public bool SetNewCrowdDestination(Vector3 p)
    {
        int totalNum = m_liveAgentList.Count;
        Debug.Log(totalNum);
        for (int i = 0; i < totalNum; i++)
        {
            m_liveAgentList[i].ChangeStateImmediate(AgentState.Move);
            //m_liveAgentList[i].SetAnimationClip("Move");
            m_liveAgentList[i].SetNavDestination(p);
            if (bRandomSpeed)
                m_liveAgentList[i].SetNavMeshAgentSpeed(UnityEngine.Random.Range(minRandomSpeed, maxRandomSpeed));
        }

        return true;
    }
    public void DoAttachOperation(AAgentElement agentElement, bool kill, AgentState nextState, float newVelocityScale)
    {
        if (kill)
        {
            m_liveAgentList.Remove(agentElement);
            agentElement.RemoveNavMeshAgent();
            agentElement.RemoveCollider();
        }
        else
        {
            agentElement.ChangeStateImmediate(nextState);
            agentElement.SetNavMeshAgentActive(false);
            agentElement.SetNavMeshAgentVelocity(newVelocityScale);
        }
    }
    public void DoRangeOperation(AAgentElement agentElement, float lethalPercent, float delayTime, float durationTime, AgentState agentNextState, float speed, bool kill) {
        StartCoroutine(IRangeOperation(agentElement, lethalPercent, delayTime, durationTime, agentNextState, speed, kill));
    }
    IEnumerator IRangeOperation(AAgentElement agentElement, float percent, float delay, float durating, AgentState state, float speed, bool kill)
    {
        yield return new WaitForSeconds(delay);
        int totalNum = m_liveAgentList.Count;
        float duratingUnitTime = durating / totalNum / percent;
        List<AAgentElement> unchangeAgentsList = new List<AAgentElement>(m_liveAgentList);
        List<AAgentElement> changeAgentsList = new List<AAgentElement>();
        bool bLock = false;
        Loom.RunAsync(() =>
        {
            System.Random rnd = new System.Random();
            for (int i = 0; i < totalNum * percent; i++)
            {
                int current = rnd.Next(0, unchangeAgentsList.Count);
                AAgentElement currentAgant = unchangeAgentsList[current];
                changeAgentsList.Add(currentAgant);
                unchangeAgentsList.Remove(currentAgant);
                if (kill)
                {
                    m_liveAgentList.Remove(currentAgant);
                }
            }
            bLock = true;
        }
        );
        Loom.QueueOnMainThread(() =>
        {
            while (!bLock)
            {
                Debug.Log("waiting subThread");
            }
            for(int i=0;i<changeAgentsList.Count;i++)
            {
                changeAgentsList[i].DoRangeOperation(state,duratingUnitTime*i, speed, kill);
            }           

        }
            );

        yield return null;
    }

}

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
    public bool bRandomAnimationStartFrame = true;
    void Start () {
        m_ownAgents = GetComponentsInChildren<AAgentElement>();
        m_currentTargetPositionList = new List<Vector3>();
        m_liveAgentList = new List<AAgentElement>(m_ownAgents);
        if (GetComponentInChildren<Animator>())
        {
            bAnimator = true;
        }
	}
    public bool SetNewCrowdDestination(Vector3[] p,bool bKeep)
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
        int[] perDesLineNum;
        perDesLineNum = new int[totalNum];
        float totalDesLineDis=0;
        if (destinationPointCount > 0)
        {
                for (int n = 0; n < destinationPointCount - 1; ++n)
                {
                    totalDesLineDis += (int)Vector3.Distance(m_currentTargetPositionList[n], m_currentTargetPositionList[n + 1]);
                }
                for (int n = 0; n < destinationPointCount - 1; ++n)
                {
                    perDesLineNum[n] = (int)((int)Vector3.Distance(m_currentTargetPositionList[n], m_currentTargetPositionList[n + 1]) / totalDesLineDis * totalNum);
                }
        }
        
            int currentLine = 0;
            int j = 0;
            for (int i = 0; i < totalNum; i++)
            {
                if (destinationPointCount > 1&& currentLine< destinationPointCount-1)
                {
                    float tmp = (float)j < perDesLineNum[currentLine] ? (float)j / perDesLineNum[currentLine] : 1;
                    tmpDes[i] = Vector3.Lerp(m_currentTargetPositionList[currentLine], m_currentTargetPositionList[currentLine + 1], tmp);
                //Debug.Log(j+".."+currentLine+"i:"+i);
                    ++j;
                    if (j > perDesLineNum[currentLine] - 1)
                    {
                        j = 0;
                        currentLine += 1;
                    }
                }
                //else
                //{
                //    tmpDes[i] = m_currentTargetPositionList[0];
                //}
                //float tmp = (float)i / (float)totalNum * (destinationPointCount - 1);
                //int t = (int)tmp;
                //if (destinationPointCount > 1)
                //{
                //    tmpDes[i] = Vector3.Lerp(m_currentTargetPositionList[t], m_currentTargetPositionList[t + 1], tmp - t);
                //}
                else
                {
                    tmpDes[i] = m_currentTargetPositionList[0];
                }

            }
            for (int i = 0; i < totalNum; i++)
            {
                m_liveAgentList[i].ChangeStateImmediate(AgentState.Move);
                m_liveAgentList[i].SetNavDestination(tmpDes[i], bKeep);
                if (bRandomSpeed)
                    m_liveAgentList[i].SetNavMeshAgentSpeed(UnityEngine.Random.Range(minRandomSpeed, maxRandomSpeed));
            }
        //Loom.RunAsync(() =>
        //{
        //    int currentLine = 0;
        //    int j = 0;
        //    for (int i = 0; i < totalNum; i++)
        //    {
        //        if (destinationPointCount > 1)
        //        {
        //            float tmp = (float)j < perDesLineNum[currentLine] ? (float)j / perDesLineNum[currentLine] : 1;
        //            tmpDes[i] = Vector3.Lerp(m_currentTargetPositionList[currentLine], m_currentTargetPositionList[currentLine + 1],tmp );

        //            ++j;
        //            if (j > perDesLineNum[currentLine] - 2)
        //            {
        //                j = 0;
        //                currentLine += 1;
        //            }
        //        }
        //        //else
        //        //{
        //        //    tmpDes[i] = m_currentTargetPositionList[0];
        //        //}
        //        //float tmp = (float)i / (float)totalNum * (destinationPointCount - 1);
        //        //int t = (int)tmp;
        //        //if (destinationPointCount > 1)
        //        //{
        //        //    tmpDes[i] = Vector3.Lerp(m_currentTargetPositionList[t], m_currentTargetPositionList[t + 1], tmp - t);
        //        //}
        //        else
        //        {
        //            tmpDes[i] = m_currentTargetPositionList[0];
        //        }

        //    }
        //    bLock = true;
        //}
        //);
        //Loom.QueueOnMainThread(() =>
        //{
        //        while (!bLock)
        //        {
        //            Debug.Log("waiting subThread");
        //        }
        //        for (int i = 0; i < totalNum; i++)
        //        {
        //            m_liveAgentList[i].ChangeStateImmediate(AgentState.Move);
        //            m_liveAgentList[i].SetNavDestination(tmpDes[i], bKeep);
        //            if (bRandomSpeed)
        //                m_liveAgentList[i].SetNavMeshAgentSpeed(UnityEngine.Random.Range(minRandomSpeed, maxRandomSpeed));
        //        }
        //}
        //);

        return true;
    }

    public bool SetNewCrowdDestination(Vector3[] p, bool setLineNumber, int rowNumber, int lineNumber)
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
        int[] perDesLineNum;
        perDesLineNum = new int[totalNum];
        float totalDesLineDis = 0;

        if (destinationPointCount > 0)
        {
            if (!setLineNumber)
            {
                for (int n = 0; n < destinationPointCount - 1; ++n)
                {
                    totalDesLineDis += (int)Vector3.Distance(m_currentTargetPositionList[n], m_currentTargetPositionList[n + 1]);
                }
                for (int n = 0; n < destinationPointCount - 1; ++n)
                {

                    perDesLineNum[n] = (int)((int)Vector3.Distance(m_currentTargetPositionList[n], m_currentTargetPositionList[n + 1]) / totalDesLineDis * totalNum);
                    if (perDesLineNum[n] == 0)
                    {
                        perDesLineNum[n] = 1;
                    }
                }
            }
            else
            {
                perDesLineNum[1] = perDesLineNum[3] = rowNumber;
                perDesLineNum[0] = perDesLineNum[2] = lineNumber;
            }
        }

        int currentLine = 0;
        int j = 0;
        for (int i = 0; i < totalNum; i++)
        {
            //Debug.Log("j:" + j+" i:"+i);
            if (destinationPointCount > 1 && currentLine < destinationPointCount - 1)
            {
                if (perDesLineNum[currentLine] == 0)
                {
                    currentLine += 1;
                }
                    float tmp = (float)j <= perDesLineNum[currentLine] ? (float)j / perDesLineNum[currentLine] : 1;
                    tmpDes[i] = Vector3.Lerp(m_currentTargetPositionList[currentLine], m_currentTargetPositionList[currentLine + 1], tmp);
                //Debug.Log((float)j / perDesLineNum[currentLine]+" position:"+tmpDes[i]);
                ++j;
                    if (j > perDesLineNum[currentLine]-1 )
                    {
                        j = 0;
                        currentLine += 1;
                    }
            }
            //else
            //{
            //    tmpDes[i] = m_currentTargetPositionList[0];
            //}
            //float tmp = (float)i / (float)totalNum * (destinationPointCount - 1);
            //int t = (int)tmp;
            //if (destinationPointCount > 1)
            //{
            //    tmpDes[i] = Vector3.Lerp(m_currentTargetPositionList[t], m_currentTargetPositionList[t + 1], tmp - t);
            //}
            else
            {
                tmpDes[i] = m_currentTargetPositionList[0];
            }

        }
        for (int i = 0; i < totalNum; i++)
        {
            m_liveAgentList[i].ChangeStateImmediate(AgentState.Move);
            m_liveAgentList[i].SetNavDestination(tmpDes[i], false);
            if (bRandomSpeed)
                m_liveAgentList[i].SetNavMeshAgentSpeed(UnityEngine.Random.Range(minRandomSpeed, maxRandomSpeed));
        }
        //Loom.RunAsync(() =>
        //{
        //    int currentLine = 0;
        //    int j = 0;
        //    for (int i = 0; i < totalNum; i++)
        //    {
        //        if (destinationPointCount > 1)
        //        {
        //            float tmp = (float)j < perDesLineNum[currentLine] ? (float)j / perDesLineNum[currentLine] : 1;
        //            tmpDes[i] = Vector3.Lerp(m_currentTargetPositionList[currentLine], m_currentTargetPositionList[currentLine + 1],tmp );

        //            ++j;
        //            if (j > perDesLineNum[currentLine] - 2)
        //            {
        //                j = 0;
        //                currentLine += 1;
        //            }
        //        }
        //        //else
        //        //{
        //        //    tmpDes[i] = m_currentTargetPositionList[0];
        //        //}
        //        //float tmp = (float)i / (float)totalNum * (destinationPointCount - 1);
        //        //int t = (int)tmp;
        //        //if (destinationPointCount > 1)
        //        //{
        //        //    tmpDes[i] = Vector3.Lerp(m_currentTargetPositionList[t], m_currentTargetPositionList[t + 1], tmp - t);
        //        //}
        //        else
        //        {
        //            tmpDes[i] = m_currentTargetPositionList[0];
        //        }

        //    }
        //    bLock = true;
        //}
        //);
        //Loom.QueueOnMainThread(() =>
        //{
        //        while (!bLock)
        //        {
        //            Debug.Log("waiting subThread");
        //        }
        //        for (int i = 0; i < totalNum; i++)
        //        {
        //            m_liveAgentList[i].ChangeStateImmediate(AgentState.Move);
        //            m_liveAgentList[i].SetNavDestination(tmpDes[i], bKeep);
        //            if (bRandomSpeed)
        //                m_liveAgentList[i].SetNavMeshAgentSpeed(UnityEngine.Random.Range(minRandomSpeed, maxRandomSpeed));
        //        }
        //}
        //);

        return true;
    }
    public bool SetNewCrowdDestination(Vector3 p,bool kep)
    {
        int totalNum = m_liveAgentList.Count;
        for (int i = 0; i < totalNum; i++)
        {
            m_liveAgentList[i].ChangeStateImmediate(AgentState.Move);
            //m_liveAgentList[i].SetAnimationClip("Move");
            m_liveAgentList[i].SetNavDestination(p, kep);
            if (bRandomSpeed)
                m_liveAgentList[i].SetNavMeshAgentSpeed(UnityEngine.Random.Range(minRandomSpeed, maxRandomSpeed));
        }

        return true;
    }
    public void DoAttachOperation(AAgentElement agentElement, bool kill, AgentState nextState, bool bSetNewSpeed, float minNewSpeed, float maxNewSpeed, float durTime, bool destroy, float delayTime)
    {
        float oldSpeed = agentElement.agentNavSpeed;
        agentElement.ChangeStateImmediate(nextState);
        agentElement.SetNavMeshAgentActive(false);
        
        agentElement.SetNavMeshAgentVelocity(0);
        DoKillAgent(agentElement, kill);
        DoDestroyAgent(agentElement, destroy, delayTime);
        if (!kill || !destroy)
        {
            if(bSetNewSpeed)
                StartCoroutine(IRecoverOrigin(durTime, agentElement, minNewSpeed, maxNewSpeed));
            else
                StartCoroutine(IRecoverOrigin(durTime, agentElement, oldSpeed));
        }

    }
    IEnumerator IRecoverOrigin(float deltaTime, AAgentElement agentElement, float oldSpeed)
    {
        yield return new WaitForSeconds(deltaTime);
        if (agentElement.bKill == false)
        {
            agentElement.SetNavMeshAgentActive(true);
            agentElement.SetNavMeshAgentSpeed(oldSpeed);
            agentElement.ChangeStateImmediate(AgentState.Move);
        }
    }
    IEnumerator IRecoverOrigin(float deltaTime, AAgentElement agentElement, float minSpeed, float maxSpeed)
    {
        yield return new WaitForSeconds(deltaTime);
        if (agentElement.bKill == false)
        {
            agentElement.SetNavMeshAgentActive(true);
            agentElement.SetNavMeshAgentSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
            agentElement.ChangeStateImmediate(AgentState.Move);
        }
    }
    public void DoKillAgent(AAgentElement agentElement, bool kill)
    {
        if (kill)
        {
            agentElement.bKill = true;
            m_liveAgentList.Remove(agentElement);
            agentElement.SetPuppetDead();
        }
    }
    public void DoDestroyAgent(AAgentElement agentElement, bool destroy, float delayTime)
    {
        if (destroy)
        {
            StartCoroutine(IDoDestroyAgent(agentElement, delayTime));
        }
    }
    IEnumerator IDoDestroyAgent(AAgentElement agentElement, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (m_liveAgentList.Contains(agentElement))
        {
            m_liveAgentList.Remove(agentElement);
        }
        if(agentElement!= null)
            Destroy(agentElement.gameObject);
    }
    public void DoRangeOperation(AAgentElement agentElement, float lethalPercent, float delayTime, float durationTime, AgentState agentNextState, bool bSet, float minNewSpeed, float maxNewSpeed, bool kill, bool destroy, float delayDesTime ) {
        StartCoroutine(IRangeOperation(agentElement, lethalPercent, delayTime, durationTime, agentNextState, bSet, minNewSpeed, maxNewSpeed, kill, destroy, delayDesTime));
    }
    IEnumerator IRangeOperation(AAgentElement agentElement, float percent, float delay, float durating, AgentState state, bool bSet, float minNewSpeed, float maxNewSpeed, bool kill, bool destroy, float delayDesTime)
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
                changeAgentsList[i].DoRangeOperation(state,duratingUnitTime*i, bSet,minNewSpeed, maxNewSpeed, kill, destroy, delayDesTime);
            }           

        }
            );

        yield return null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public enum AgentState { Idle, Move, Attack, Dead };
public class AAgentElement : MonoBehaviour {
    public float agentNavSpeed {
        get
        {
            return m_navMeshAgent.speed;
        }
    }

    public AgentState currentAgentState;
    public Vector3 agentNavDestination;
    public string currentAgentAnimation {
        get
        {
            return m_animationInstancing.GetCurrentAnimationInfo().animationName;
        }
    }

    private AnimationInstancing.AnimationInstancing m_animationInstancing;
    private NavMeshAgent m_navMeshAgent;
    private ACrowdElement m_crowdElement;
    private Rigidbody m_rigidbody;
    private Collider m_collider;

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Active)]
    void OnDrawGizmos()
    {
        if (Vector3.Distance(this.transform.position, Camera.current.transform.position) > 10f)
            Gizmos.DrawIcon(this.transform.position, "Agent.png", true);
    }

    void Start () {
        m_navMeshAgent = this.GetComponent<NavMeshAgent>();
        m_animationInstancing = this.GetComponent<AnimationInstancing.AnimationInstancing>();
        m_crowdElement = this.GetComponentInParent<ACrowdElement>();
        m_collider = this.GetComponent<Collider>();
        Invoke("Init",0.1f);
        InvokeRepeating("SetAnimationSpeed", 1f, 1f);
    }

	void Init()
    {
        //TODO : Using configuration file
        m_navMeshAgent.stoppingDistance = m_crowdElement.navStopDistance;
        SetAnimationClip("Idle");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("physicalController"))
        {
            AController tmp = other.GetComponent<AController>();
            if (tmp.controllerType == ControllerType.Range)
            {
                tmp.Reponse(m_crowdElement, this);
                PerformanceOptimization();
            }
            else if (tmp.controllerType == ControllerType.Attach)
            {
                tmp.Reponse(m_crowdElement,this);
                PerformanceOptimization();
            }
            else if (tmp.controllerType == ControllerType.TempleteExplose)
            {
                if (m_rigidbody == null)
                {
                    m_rigidbody = this.gameObject.AddComponent<Rigidbody>();
                    RemoveNavMeshAgent();
                    DoControllerExplose(tmp);
                }
                tmp.Reponse(m_crowdElement,this);
                ChangeStateImmediate(tmp.nextCrowdState);
            }
        }
    }

    void SetAnimationSpeed()
    {
        if (m_navMeshAgent && currentAgentState == AgentState.Move)
            m_animationInstancing.playSpeed = m_navMeshAgent.velocity.magnitude * m_crowdElement.animationScale;
    }
    void DoControllerExplose(AController aController)
    {
        if (this.gameObject.GetComponent<Rigidbody>())
        {
            m_collider.isTrigger = false;
            m_rigidbody.isKinematic = false;
            m_rigidbody.AddExplosionForce(aController.exploseForce, aController.transform.position - new Vector3(0, 30f, 0), aController.exploseRadius);
        }
    }
    void PerformanceOptimization()
    {
        if (this.gameObject.GetComponent<Rigidbody>())
        {
            m_rigidbody.Sleep();

        }
    }
    public void SetNavDestination(Vector3 tar)
    {
        NavMeshPath path = new NavMeshPath();
        if(m_navMeshAgent.CalculatePath(tar, path))
            m_navMeshAgent.SetPath(path);
        m_navMeshAgent.destination = tar;
        agentNavDestination = tar;
    }
    public void SetNavMeshAgentActive(bool t)
    {
        m_navMeshAgent.isStopped = !t;
    }
    public void RemoveNavMeshAgent()
    {
        if(m_navMeshAgent)
            Destroy(m_navMeshAgent);
    }
    public void RemoveCollider()
    {
        if (m_collider)
        {
            Destroy(m_collider);
        }
    }
    public void SetNavMeshAgentStopDistance(float t)
    {
        m_navMeshAgent.stoppingDistance = t;
    }
    public void SetAnimationClip(int i)
    {
        m_animationInstancing.PlayAnimation(i);
    }

    public void ChangeStateImmediate(AgentState newState)
    {
        switch (newState)
        {
            case AgentState.Dead:
                SetAnimationClip("Dead");
                currentAgentState = AgentState.Dead;
                break;
            case AgentState.Idle:
                SetAnimationClip("Idle");
                currentAgentState = AgentState.Idle;
                break;
            case AgentState.Move:
                SetAnimationClip("Move");
                currentAgentState = AgentState.Move;
                break;
            case AgentState.Attack:
                SetAnimationClip("Attack");
                currentAgentState = AgentState.Attack;
                break;
            default:
                break;
        }
    }
    public void SetAnimationClip(string s)
    {
        m_animationInstancing.playSpeed = 1f;
        m_animationInstancing.PlayAnimation(s);
    }
    public void SetNavMeshAgentSpeed(float t)
    { 
        m_navMeshAgent.speed = t;
    }
    public void SetNavMeshAgentVelocity(float t)
    {
        if (t == -1)
        {
            return;
        }
        m_navMeshAgent.velocity = t * m_navMeshAgent.velocity;

    }
    public void DoRangeOperation(AgentState agentState, float delayTime, float speed, bool kill) {
        StartCoroutine(IRangeOperation(agentState, delayTime, speed, kill));
    }
    IEnumerator IRangeOperation(AgentState agentState, float delayTime, float speed, bool kill)
    {
        yield return new WaitForSeconds(delayTime);
        SetNavMeshAgentVelocity(speed);
        ChangeStateImmediate(agentState);
        if(kill)
        {
            RemoveNavMeshAgent();
            RemoveCollider();
        }
    }



}

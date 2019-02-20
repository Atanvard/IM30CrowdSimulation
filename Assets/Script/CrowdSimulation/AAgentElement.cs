using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using RootMotion.Dynamics;
public enum AgentState { Idle, Move, Attack, Dead };
public class AAgentElement : MonoBehaviour {
    public float agentNavSpeed {
        get
        {
            return m_navMeshAgent.speed;
        }
    }
    public bool bKill = false;
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
    private Animator m_animator;
    private PuppetMaster m_puppetMaster;

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
        m_animator = this.GetComponentInChildren<Animator>();
        m_puppetMaster = this.GetComponentInChildren<PuppetMaster>();
        Invoke("Init",0.1f);
        InvokeRepeating("MatchMoveAnimationSpeed", 0.1f, 0.3f);
    }

	void Init()
    {
        //TODO : Using configuration file
        m_navMeshAgent.stoppingDistance = m_crowdElement.navStopDistance;
        if (m_crowdElement.bRandomAnimationStartFrame)
        {
            if (m_crowdElement.bAnimator)
            {
                m_animator.Play("Idle", 0, Random.Range(0f, 1f));
            }
            else
            {
                m_animationInstancing.PlayAnimation("Idle");
                m_animationInstancing.preAniFrame = Random.Range(0, m_animationInstancing.GetCurrentAnimationInfo().totalFrame) ;
            }
        }
        else
        {
            SetAnimationClip("Idle");
        }
        //m_animator.CrossFade("Move", 0.1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("physicalController"))
        {
            AController tmp = other.GetComponent<AController>();
            foreach( var crow in tmp.affectCrowds)
            {
                if(crow == this.m_crowdElement)
                {
                    if (tmp.controllerType == ControllerType.Range)
                    {
                        tmp.Reponse(m_crowdElement, this);
                        //PerformanceOptimization();
                    }
                    else if (tmp.controllerType == ControllerType.Attach)
                    {
                        tmp.Reponse(m_crowdElement, this);
                        //PerformanceOptimization();
                    }
                    else if (tmp.controllerType == ControllerType.TempleteExplose)
                    {
                        if (m_rigidbody == null)
                        {
                            m_rigidbody = this.gameObject.AddComponent<Rigidbody>();
                        }
                        RemoveNavMeshAgent();
                        DoControllerExplose(tmp);
                        tmp.Reponse(m_crowdElement, this);
                        ChangeStateImmediate(tmp.nextCrowdState);
                    }
                    else if (tmp.controllerType == ControllerType.TempleteColider)
                    {
                        tmp.Reponse(m_crowdElement, this);
                        ChangeStateImmediate(tmp.nextCrowdState);
                        RemoveCollider();
                        RemoveNavMeshAgent();  //TODO: not use remove
                        SetPuppetPinWeight(0);
                        if (tmp.bKill)
                        {
                            SetPuppetDead();
                        }
                    }
                }
            }
        }
    }

    void MatchMoveAnimationSpeed()
    {
        if (m_navMeshAgent && currentAgentState == AgentState.Move)
        {
            if (m_crowdElement.bAnimator)
            {
                m_animator.speed = m_navMeshAgent.velocity.magnitude * m_crowdElement.animationScale;
            }
            else
            {
                m_animationInstancing.playSpeed = m_navMeshAgent.velocity.magnitude * m_crowdElement.animationScale;
            }
        }
    }
    void DoControllerExplose(AController aController)
    {
        if (this.gameObject.GetComponent<Rigidbody>())
        {
            m_collider.isTrigger = false;
            m_rigidbody.isKinematic = false;
            m_rigidbody.mass = 100;
            m_rigidbody.AddExplosionForce(aController.exploseForce, aController.transform.position - new Vector3(0, 30f, 0), aController.exploseRadius);
        }
        //SetPuppetPinWeight(0);
        Invoke("SetPuppetDead",1f);
    }
    void PerformanceOptimization()
    {
        if (this.gameObject.GetComponent<Rigidbody>())
        {
            m_rigidbody.Sleep();

        }
    }
    void SetAnimationClip(string s)
    {
        if (m_crowdElement.bAnimator)
        {
            //m_animator.speed = 1;
            //m_animator.CrossFade(s, 0.1f);
            foreach (AnimatorControllerParameter p in m_animator.parameters)
            if (p.type == AnimatorControllerParameterType.Trigger)
                    m_animator.ResetTrigger(p.name);
            m_animator.SetTrigger(s);
        }
        else
        {
            m_animationInstancing.playSpeed = 1f;
            m_animationInstancing.PlayAnimation(s);
        }
    }
    void SetPuppetPinWeight(float t)
    {
        if (m_puppetMaster)
        {
            m_puppetMaster.pinWeight = t;
        }
    }
    public void SetPuppetDead()
    {
        if (m_puppetMaster)
        {
            m_puppetMaster.state = PuppetMaster.State.Dead;
        }
        if (m_rigidbody)
        {
            Destroy(m_rigidbody);
        }
    }
    public void SetNavDestination(Vector3 tar)
    {
        if (m_navMeshAgent)
        {
            NavMeshPath path = new NavMeshPath();
            if (m_navMeshAgent.CalculatePath(tar, path))
                m_navMeshAgent.SetPath(path);
            m_navMeshAgent.destination = tar;
            agentNavDestination = tar;
        }
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
    public void SetNavMeshAgentSpeed(float t)
    {
            if (m_navMeshAgent)
                m_navMeshAgent.speed = t;
    }
    public void SetNavMeshAgentVelocity(float t)
    {
        if (m_navMeshAgent)
        {
            m_navMeshAgent.velocity =  t * m_navMeshAgent.velocity;
        }

    }
    public void DoRangeOperation(AgentState agentState, float delayTime, bool bSet, float minNewSpeed, float maxNewSpeed, bool kill,bool destroy, float delayDesTime) {
        StartCoroutine(IRangeOperation(agentState, delayTime, bSet, minNewSpeed, maxNewSpeed, kill, destroy, delayDesTime));
    }
    IEnumerator IRangeOperation(AgentState agentState, float delayTime, bool bSet, float minNewSpeed, float maxNewSpeed, bool kill, bool destroy, float delayDesTime)
    {
        yield return new WaitForSeconds(delayTime);
        if(bSet)
        SetNavMeshAgentSpeed(Random.Range(minNewSpeed, maxNewSpeed));
        else if (kill)
        {
            SetNavMeshAgentSpeed(0);
            SetNavMeshAgentVelocity(0);
        }
        ChangeStateImmediate(agentState);
        m_crowdElement.DoKillAgent(this, kill);
        m_crowdElement.DoDestroyAgent(this, destroy, delayDesTime);

    }



}

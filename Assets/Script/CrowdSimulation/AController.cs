using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ControllerType { Range, Attach, TempleteExplose, TempleteColider };
[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class AController : MonoBehaviour {
    public ControllerType controllerType;
    public AgentState nextCrowdState;
    public float affectPercent=0;
    public float delayTime=0;
    public float durationTime=1;
    public float exploseForce = 10;
    public float exploseRadius = 10;
    public bool bKill = false;
    public bool bDestroy = false;
    public float destroyDelayTime = 0;
    public bool enable {
        get
        {
            return this.GetComponent<Collider>().enabled;
        }
        set
        {
            this.GetComponent<Collider>().enabled = value;
        }
    }
    /// <summary>
    /// speed after attach,if newSpeed.x = -1 means use origin speed.
    /// </summary>
    public float newSpeed = 1f;
    private Rigidbody m_rigidbody;
    private Color m_gizmoColor = Color.white;
    private Collider m_colider;

	void Start () {
        m_colider = GetComponent<Collider>();
        m_rigidbody = GetComponent<Rigidbody>();

        m_rigidbody.isKinematic = true;

        m_rigidbody.Sleep();
        if (controllerType == ControllerType.TempleteColider)
        {
            GameObject child = new GameObject();
            child.AddComponent<BoxCollider>();
            child.transform.parent = this.transform;
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;
        }
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 rotationMatrix = transform.localToWorldMatrix;
        Gizmos.matrix = rotationMatrix;
        if (enable)
        {
            Gizmos.color = Color.white;
        }
        else
        {
            Gizmos.color = Color.gray;
        }

        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 0.9f);
    }

    public void Reponse(ACrowdElement crowdElement,AAgentElement agentElement)
    {
        ChangeGizmoColor(Color.black);
        if (controllerType == ControllerType.Range) { 
            crowdElement.DoRangeOperation(agentElement, affectPercent / 100, delayTime, durationTime, nextCrowdState, newSpeed, bKill, bDestroy, destroyDelayTime);
            DisableController();
        }else if(controllerType == ControllerType.Attach)
        {
            crowdElement.DoAttachOperation(agentElement, bKill, nextCrowdState, newSpeed, durationTime, bDestroy, destroyDelayTime);
        }
        else if(controllerType == ControllerType.TempleteExplose)
        {
            Invoke("DisableController", durationTime);
            crowdElement.DoKillAgent(agentElement, bKill);
            crowdElement.DoDestroyAgent(agentElement, bDestroy,destroyDelayTime);
        }
        else if(controllerType == ControllerType.TempleteColider)
        {
            crowdElement.DoKillAgent(agentElement, bKill);
            crowdElement.DoDestroyAgent(agentElement, bDestroy, destroyDelayTime);
        }
    }

    void ChangeGizmoColor(Color c)
    {
        m_gizmoColor = c;
    }

	bool DisableController()
    {
        this.enable = false;
        return true;
    }
}

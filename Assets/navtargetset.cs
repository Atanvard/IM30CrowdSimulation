using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navtargetset : MonoBehaviour {
    public NavMeshAgent navMeshAgent;
    public Transform tar;
    // Use this for initialization
    void Start () {
        navMeshAgent.SetDestination(tar.position);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

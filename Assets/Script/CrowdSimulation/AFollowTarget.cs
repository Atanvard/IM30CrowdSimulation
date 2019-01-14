using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFollowTarget : MonoBehaviour {

    public ACrowdElement[] affectCrowdElement;
    public GameObject target;
    public float intervalTime = 5f;
    float m_currentTime = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(m_currentTime < intervalTime)
        {
            m_currentTime += Time.deltaTime;
        }
        else
        {
            foreach(var crowd in affectCrowdElement)
            {
                crowd.SetNewCrowdDestination(target.transform.position);
            }
            m_currentTime = 0;
        }
	}
}

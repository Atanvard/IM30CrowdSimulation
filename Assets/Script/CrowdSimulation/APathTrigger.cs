using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APathTrigger : MonoBehaviour {
    public int ID;
    private APath m_ownPath;
    private int m_bStay = 0;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("physicalAgent"))
        {
                ++m_bStay;
            if (ID < m_ownPath.allowMaxID&& m_bStay>0)
            {
                if (m_ownPath.SetDeatination(ID))
                {
                    Destroy(this.gameObject);
                    Debug.Log("kill");
                }
            }
        }
    }

    void Start () {
        m_ownPath = transform.parent.GetComponent<APath>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

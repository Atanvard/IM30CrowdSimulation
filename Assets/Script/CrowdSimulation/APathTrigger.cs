using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APathTrigger : MonoBehaviour {
    public int ID;
    private APath m_ownPath;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("physicalAgent"))
        {
            if (m_ownPath.SetDeatination(ID))
            {
                Destroy(this.gameObject);
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

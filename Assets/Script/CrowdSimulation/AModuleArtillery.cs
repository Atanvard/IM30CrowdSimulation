using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AModuleArtillery : MonoBehaviour {
    public Transform shootTrans;
    public Transform targetTrans;
    public float shootSpeed;
    public GameObject bulletPrefab;
    public bool play;
    public bool bLoop;
    public float loopIntervalTime;
    public Animator artilleryAnimator;
    public AController exlposeController;
    bool bInIEnumerator = false;
    AModuleArtilleryBullet m_moduleArtilleryBullet;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (bLoop && bInIEnumerator == false)
        {
            StartCoroutine(Shoot(loopIntervalTime));
        }
        else if (!bLoop && play)
        {
            artilleryAnimator.SetTrigger("Attack");
            GameObject obj = Instantiate(bulletPrefab,this.transform);
            m_moduleArtilleryBullet = obj.GetComponent<AModuleArtilleryBullet>();
            obj.transform.position = shootTrans.position;
            m_moduleArtilleryBullet.startPos = shootTrans;
            m_moduleArtilleryBullet.endPos = targetTrans;
            m_moduleArtilleryBullet.shootSpeed = shootSpeed;
            m_moduleArtilleryBullet.controller = exlposeController;
            play = false;
        }
	}
    IEnumerator Shoot(float interval)
    {
        bInIEnumerator = true;
        yield return new WaitForSeconds(interval);
        artilleryAnimator.SetTrigger("Attack");
        GameObject obj = Instantiate(bulletPrefab, this.transform);
        m_moduleArtilleryBullet = obj.GetComponent<AModuleArtilleryBullet>();
        obj.transform.position = shootTrans.position;
        m_moduleArtilleryBullet.startPos = shootTrans;
        m_moduleArtilleryBullet.endPos = targetTrans;
        m_moduleArtilleryBullet.shootSpeed = shootSpeed;
        m_moduleArtilleryBullet.controller = exlposeController;
        bInIEnumerator = false;

    }
}

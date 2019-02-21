using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AModuleArtilleryBullet : MonoBehaviour {

    public Transform startPos;
    public Transform endPos;
    public float shootSpeed;
    public AController controller;
    Vector3 m_velocity;
    float currentTime = 0;

    // Use this for initialization
    void Start () {

        float time = Vector3.Distance(startPos.position, endPos.position) / shootSpeed;
        m_velocity = new Vector3((endPos.position.x - startPos.position.x) / time, (endPos.position.y - startPos.position.y) / time + 0.5f * 9.8f * time, (endPos.position.z - startPos.position.z) / time);
    }

    // Update is called once per frame
    void Update () {
        currentTime += Time.deltaTime;
        float x = startPos.position.x + currentTime * m_velocity.x;
        float y = startPos.position.y + currentTime * m_velocity.y - 0.5f * 9.8f * currentTime * currentTime;
        float z = startPos.position.z + currentTime * m_velocity.z;
        this.transform.position = new Vector3(x, y, z);
        if(Vector3.Distance(this.transform.position, endPos.position)<1)
        {
            controller.enable = true;
            Destroy(this.gameObject);
            ParticleSystem[] systems = this.gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in systems)
            {
                particle.Play();
            }
        }
        //transform.position += (m_velocity + new Vector3(0, -9.8f, 0)) * Time.fixedDeltaTime;
        //this.transform.Translate(m_velocity);
	}
}

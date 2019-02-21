using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class AModuleArtilleryLine : MonoBehaviour {
    public AModuleArtillery moduleArtillery;
    public LineRenderer lineRenderer;
    int LINERENDERERCOUNTPOINT = 20;
	// Use this for initialization
	void Start () {
        lineRenderer.alignment = LineAlignment.Local;
        lineRenderer.positionCount = LINERENDERERCOUNTPOINT;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.cyan;
    }
    // Update is called once per frame
    void Update ()
    {
        float shootSpeed = moduleArtillery.shootSpeed;
        Transform startPos = moduleArtillery.shootTrans;
        Transform endPos = moduleArtillery.targetTrans;
        //startPos.LookAt(endPos);
        //float slope = startPos.transform.forward.y / new Vector3(startPos.transform.forward.x, 0, startPos.transform.forward.z).magnitude;
        float slope = startPos.transform.forward.y / startPos.transform.forward.z;
        float scale = 1f;
        float tmp = shootSpeed * slope * scale;
        float time = Vector3.Distance(startPos.position, endPos.position) / shootSpeed;
        Vector3 velocity = new Vector3((endPos.position.x - startPos.position.x) / time,  (endPos.position.y - startPos.position.y) / time + 0.5f * 9.8f * time, (endPos.position.z - startPos.position.z) / time);
        Vector3[] linePoints = new Vector3[LINERENDERERCOUNTPOINT];

        for (int i = 0; i < LINERENDERERCOUNTPOINT; i++)
        {
            float currentTime = (float)i / LINERENDERERCOUNTPOINT * time;
            float x = startPos.position.x + currentTime * velocity.x;
            float y = startPos.position.y + currentTime * velocity.y - 0.5f * 9.8f * currentTime * currentTime;
            float z = startPos.position.z + currentTime * velocity.z;
            linePoints[i] = new Vector3(x, y, z);
        }
        lineRenderer.SetPositions(linePoints);
    }
}

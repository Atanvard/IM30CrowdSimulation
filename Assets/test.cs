using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    private AnimationInstancing.AnimationInstancing m_animationInstancing;
    public int n;
    // Use this for initialization
    void Start () {
        m_animationInstancing = GetComponent<AnimationInstancing.AnimationInstancing>();

    }
	
	// Update is called once per frame
	void Update () {
        m_animationInstancing.PlayAnimation(n);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystem : MonoBehaviour {

    float time = 0.0f;
    float duration = 2.0f;

	// Use this for initialization
	void Start () {
        
		
	}

    public void Play()
    {
        GetComponent<ParticleSystem>().Play();
        Destroy(this.gameObject, 2.0f);
    }
	
	// Update is called once per frame
	void Update () {
	}
}

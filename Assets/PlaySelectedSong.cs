using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySelectedSong : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponentsInChildren<AudioSource>()[NetManager.songIndex].Play();
	}
	
	// Update is called once per frame
	void Update () {
		//if()
	}
}

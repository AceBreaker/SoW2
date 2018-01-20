using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum MatIndex
    {
        BuildSite = 0,
        City,
        Barracks,
        Airport,
        HQ,
        count
    };

public class ChangeMaterial : MonoBehaviour {

    public Material[] materials;
    public Renderer rend;


	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetNewMaterial(MatIndex pIndex)
    {
        rend.sharedMaterial = materials[(int)pIndex];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsIndexer : MonoBehaviour {

    public static int index = 0;

	// Use this for initialization
	void Start () {
        Unit[] units = GetComponentsInChildren<Unit>();

        foreach(Unit u in units)
        {
            //u.unitIndex = index++;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

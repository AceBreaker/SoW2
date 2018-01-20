using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour {

    Unit parent = null;
    Text display = null;

	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<Unit>();
        display = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHPDisplay()
    {
        display.text = parent.HitPoints.ToString();
    }
}

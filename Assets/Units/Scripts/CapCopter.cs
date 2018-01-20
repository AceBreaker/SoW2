using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapCopter : Unit {

	// Use this for initialization
	void Start () {
        HitPoints = 100;
        price = 6000;
        canCapture = true;
        type = UnitType.CAPCOPTER;
        faction = Unit.Faction.CHEAP;
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
        
    }

    public override void Initialize()
    {
        base.Initialize();
        
    }

    public override void MarkAsFriendly()
    {
        //GetComponent<Renderer>().material.color = LeadingColor + new Color(0.8f, 1, 0.8f);
    }

    public override void MarkAsReachableEnemy()
    {
        GetComponent<Renderer>().material.color = LeadingColor + Color.red;
    }

    public override void MarkAsSelected()
    {
        GetComponent<Renderer>().material.color = LeadingColor + Color.green;
    }

    public override void UnMark()
    {
        GetComponent<Renderer>().material.color = LeadingColor;
    }

    public override void MarkAsDestroyed()
    {
    }

    public override void MarkAsFinished()
    {
    }

    public override void MarkAsAttacking(Unit other)
    {
    }

    public override void MarkAsDefending(Unit other)
    {
    }
}

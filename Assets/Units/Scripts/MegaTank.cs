using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaTank : Unit {

	// Use this for initialization
	void Start () {
        HitPoints = 100;
        price = 22000;
        canCapture = false;
        type = UnitType.MEGATANK;
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

    public override void DealDamage(Unit other, bool fromNetwork = false)
    {
        base.DealDamage(other, fromNetwork);
        //find behiundOther
        AttackRange = 2;
        ActionPoints = 1;
        base.DealDamage(FindBehindUnit(other), fromNetwork);
        AttackRange = 1;
        ActionPoints = 0;
    }

    private Unit FindBehindUnit(Unit front)
    {
        Unit result = null;

        float differenceX = front.transform.position.x + (front.transform.position.x-this.transform.position.x);
        float differenceY = front.transform.position.y + (front.transform.position.y-this.transform.position.y);

        foreach(Unit u in grid.Units)
        {
            if(u.transform.position.x == differenceX && u.transform.position.y == differenceY)
            {
                result = u;
                break;
            }
        }

        return result;
    }
}

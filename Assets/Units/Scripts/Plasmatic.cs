﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasmatic : Unit {

    // Use this for initialization
    void Start()
    {
        HitPoints = 100;
        canCapture = true;
        type = UnitType.PLASMATIC;
        faction = Unit.Faction.POWER;
    }

    protected override void Update()
    {
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

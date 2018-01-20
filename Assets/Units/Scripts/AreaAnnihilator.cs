using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAnnihilator : RangedUnit
{

    // Use this for initialization
    void Start()
    {
        HitPoints = 100;
        canCapture = true;
        type = UnitType.AREAANNIHILATOR;
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

    public override void DealDamage(Unit other, bool fromNetwork = false)
    {
        base.DealDamage(other, fromNetwork);

        Unit[] secondaryTargets = FindSurroundingUnits(other);
        damageMultiplier /= 2;
        AttackRange += 1;
        for(int i  = 0; i < secondaryTargets.Length; i++)
        {
            ActionPoints = 1;
            if (secondaryTargets[i] != null)
                base.DealDamage(secondaryTargets[i], fromNetwork);
        }
        damageMultiplier *= 2;
        AttackRange -= 1;
    }

    private Unit[] FindSurroundingUnits(Unit mainTarget)
    {
        Unit[] units = new Unit[4];

        float mainX = mainTarget.transform.position.x;
        float mainY = mainTarget.transform.position.y;


        float differenceX1 = mainX + (1);
        float differenceY1 = mainY + (0);

        float differenceX2 = mainX + (-1);
        float differenceY2 = mainY + (0);

        float differenceX3 = mainX + (0);
        float differenceY3 = mainY + (1);

        float differenceX4 = mainX + (0);
        float differenceY4 = mainY + (-1);

        int foundCount = 0;
        foreach (Unit u in grid.Units)
        {
            if (FindOffsetTarget(ref units[0], u, differenceX1, differenceY1))
                foundCount++;
            else if (FindOffsetTarget(ref units[1], u, differenceX2, differenceY2))
                foundCount++;
            else if (FindOffsetTarget(ref units[2], u, differenceX3, differenceY3))
                foundCount++;
            else if (FindOffsetTarget(ref units[3], u, differenceX4, differenceY4))
                foundCount++;

            if (foundCount == 4)
                break;
        }
        //search for the 4 surrounding units

        return units;
    }

    public bool FindOffsetTarget(ref Unit target, Unit u, float diffx, float diffy)
    {
        if (u.transform.position.x == diffx && u.transform.position.y == diffy)
        {
            target = u;
            return true;
        }
        return false;
    }
}

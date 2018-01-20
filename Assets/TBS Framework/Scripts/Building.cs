using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit {

	// Use this for initialization
	void Start () {
        CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
        foreach (SampleSquare2 b in grid.Cells)
        {
            if (b.transform.position.x == this.transform.position.x && b.transform.position.y == this.transform.position.y)
            {
                if (this is BarracksUnit)
                    b.tileName = "Barracks";
                if (this is Airport)
                    b.tileName = "Airport";
                if (this is City)
                    b.tileName = "City";
                if (this is HeadQuarters)
                    b.tileName = "HQ";

                b.defenseValue = 3;
                b.FootCost = 1;
                b.MechCost = 1;
                b.TreadCost = 1;
                b.TiresCost = 1;
                break;
            }
        }

        
        faction = NetManager.factions[PlayerNumber];
    }

    public IEnumerator CapFlash()
    {
        //GetComponentInChildren<TextMesh>()

            bool sdf = false;
        string text = HitPoints.ToString();
        if (text == "20")
            yield return null;
        int i = 0;
        while(true)
        {
            if (i++ % 10 == 0)
                sdf = !sdf;
            GetComponentInChildren<TextMesh>().text = sdf?text:"";
            yield return null;

        }
    }

    protected virtual void OnMouseOver()
    {
        CellGrid.capPoints = HitPoints;
        StartCoroutine("CapFlash");

        GameObject.Find("CellGrid").GetComponent<CellGrid>().UpdateInfo();
    }
    protected override void OnMouseExit()
    {
        CellGrid.capPoints = -1;
        StopCoroutine("CapFlash");
        GameObject.Find("CellGrid").GetComponent<CellGrid>().UpdateInfo();
        GetComponentInChildren<TextMesh>().text = "";
        base.OnMouseExit();
    }

    public Unit GetOccupyingUnit()
    {
        Unit result = null;
        for(int i = 0; i < grid.Units.Count; i++)
        {
            if(!(grid.Units[i] is Building) &&
                grid.Units[i].transform.position.x == this.transform.position.x &&
                grid.Units[i].transform.position.y == this.transform.position.y)
            {
                return grid.Units[i];
            }
        }
        return result;
    }

    public override void HandleCapture()
    {
        Unit[] units = GameObject.Find("Units Parent").GetComponentsInChildren<Unit>();
        CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
        if (!Cell.IsTaken)
        {
            HitPoints = 20;
            return;
        }
        foreach (Unit u in units)
        {
            if (!(u is Building) && u.transform.position.x == this.transform.position.x && u.transform.position.y == this.transform.position.y)
            {
                if (u.canCapture && u.isAbleToCapture && u.PlayerNumber != this.PlayerNumber && grid.CurrentPlayerNumber == u.PlayerNumber)
                {
                    int capValue = (u.HitPoints / 10) + 1;
                    if (capValue > 10)
                        capValue = 10;

                    HitPoints -= capValue;
                    StartCoroutine("DamageFlash");

                    if(HitPoints <= 0)
                    {
                        PlayerNumber = u.PlayerNumber;
                        UpdateColor();
                        HitPoints = 20;
                        faction = u.faction;
                    }
                }
                else if(grid.CurrentPlayerNumber == u.PlayerNumber)
                {
                    HitPoints = 20;
                    if(u.PlayerNumber == this.PlayerNumber)
                    {
                        u.HitPoints += 20;
                        u.UpdateHpBar();
                    }
                }
                return;
            }
        }
        HitPoints = 20;
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

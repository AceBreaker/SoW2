using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeadQuarters : Building {
    
    public CellGrid cellGrid;

    public Infantry redUnit;

    public bool selected = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        bool spawnUnit = false;
        Unit newUnit = null;
	    if( Input.GetKeyDown(KeyCode.U))
        {
            newUnit = Instantiate(redUnit, cellGrid.Cells[0].transform.position, Quaternion.identity);
            //

        }

        if(spawnUnit)
        {
            SpawnUnit(newUnit);
        }
	}

    private void SpawnUnit(Unit unit)
    {
        unit.transform.parent = GameObject.Find("Units Parent").transform;
        List<Cell> cells = new List<Cell>();

        Cell cell = null;
        for (int i = 0; i < cellGrid.Cells.Count; i++)
        {
            if (cellGrid.Cells[i].transform.position.x == this.transform.position.x && cellGrid.Cells[i].transform.position.y == this.transform.position.y)
            {
                cell = cellGrid.Cells[i];
                break;
            }
        }

        if (cell != null)
        {
            cellGrid.GetComponent<CustomUnitGenerator>().SpawnUnit(unit, cell);
            cellGrid.GetComponent<CustomUnitGenerator>().SnapToGrid();
            unit.MovementPoints = 0;
        }
    }

    public override void OnTurnEnd()
    {
        if (Cell.IsTaken)
        {
            HandleCapture();
        }

        base.OnTurnEnd();
    }

    public override void HandleCapture()
    {
        //TODO  1 handle HQ capture
        int prevPlayerNumber = PlayerNumber;
        //playernumber before
        base.HandleCapture();
        //if playernumber different from playernumberbefore then we have been captured and we lose the game
        if(PlayerNumber != prevPlayerNumber)
        {
            for (int i = 0; i < grid.Players.Count; i++)
            {
                if (grid.Players[i].PlayerNumber == prevPlayerNumber)
                {
                    grid.Players[i].EliminatePlayer(true, GetOccupyingUnit());
                    break;
                }
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        transform.position += new Vector3(0, 0, -1);
        GetComponent<Renderer>().material.color = LeadingColor;
        Cell cell = null;
        for (int i = 0; i < cellGrid.Cells.Count; i++)
        {
            if (cellGrid.Cells[i].transform.position.x == this.transform.position.x && cellGrid.Cells[i].transform.position.y == this.transform.position.y)
            {
                cell = cellGrid.Cells[i];
                break;
            }
        }

        cell.IsTaken = false;
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

    public override void OnUnitSelected()
    {
        selected = true;
        base.OnUnitSelected();
    }

    public override void OnUnitDeselected()
    {
        selected = false;
        base.OnUnitDeselected();
    }
}

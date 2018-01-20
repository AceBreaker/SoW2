using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Production : Building {
    
    //public CellGrid cellGrid;
    
    
    public bool spawnUnit = false;
    public bool hasAlreadySpawned = false;

    public GameObject guiCamRef = null;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
            if(spawnUnit && selected)
            {
            }
            spawnUnit = false;
	}

    public bool CanSpawnUnit(Unit newUnit)
    {
        bool result = false;

        if(spawnUnit && newUnit.price <= PlayerController.currTurnMoney)
        {
            GameObject go = GameObject.Find("Players Parent");
            grid.Players[PlayerNumber].Money -= newUnit.price;
            //Player[] players = go.GetComponentsInChildren<Player>();
            //players[PlayerNumber].Money -= newUnit.price;
            PlayerController.currTurnMoney = grid.Players[PlayerNumber].Money;
            GameObject.Find("Canvas").GetComponentsInChildren<Text>()[PlayerNumber].text = PlayerController.currTurnMoney.ToString() + "G" +"(+"+ grid.Players[PlayerNumber].income.ToString()+"G)";
            newUnit.PlayerNumber = PlayerNumber;
            newUnit.transform.position = new Vector3(newUnit.transform.position.x, newUnit.transform.position.y, -1.0f);
            result = true;
            newUnit.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            
        }

        return result;
    }

    public void SpawnUnit(Unit unit)
    {
        unit.transform.parent = GameObject.Find("Units Parent").transform;
        List<Cell> cells = new List<Cell>();

        Cell cell = null;
        for (int i = 0; i < grid.Cells.Count; i++)
        {
            if (grid.Cells[i].transform.position.x == this.transform.position.x && grid.Cells[i].transform.position.y == this.transform.position.y)
            {
                cell = grid.Cells[i];
                break;
            }
        }

        if (cell != null)
        {
            grid.GetComponent<CustomUnitGenerator>().SpawnUnit(unit.GetComponent<Unit>(), cell);
            grid.GetComponent<CustomUnitGenerator>().SnapToGrid();
            
            unit.GetComponent<Unit>().MovementPoints = 0;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        transform.position += new Vector3(0, 0, -1);
        GetComponent<Renderer>().material.color = LeadingColor;
        Cell cell = null;
        for (int i = 0; i < grid.Cells.Count; i++)
        {
            if (grid.Cells[i].transform.position.x == this.transform.position.x && grid.Cells[i].transform.position.y == this.transform.position.y)
            {
                cell = grid.Cells[i];
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

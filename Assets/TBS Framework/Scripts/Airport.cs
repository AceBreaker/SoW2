using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Airport : Production
{

    public GameObject airportMenu = null;
    public GameObject airportCheapMenu = null;

    public Fighter unit1;
    public Bomber unit2;
    public Copter unit3;

    public CapCopter unit4;
    public AerialAce unit5;
    public AirBomb unit6;

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);
        HitPoints = 20;
    }

    // Update is called once per frame
    void Update()
    {
        Unit newUnit = null;
        UnitType type = UnitType.ERROR;
        if (!selected || faction != Faction.BASIC)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (faction == Faction.BASIC)
            {
                newUnit = Instantiate(unit1, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                type = UnitType.FIGHTER;
            }
            else if (faction == Faction.CHEAP)
            {
                newUnit = Instantiate(unit4, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                type = UnitType.CAPCOPTER;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (faction == Faction.BASIC)
            {
                newUnit = Instantiate(unit2, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                type = UnitType.BOMBER;
            }
            else if (faction == Faction.CHEAP)
            {
                newUnit = Instantiate(unit5, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                type = UnitType.AERIALACE;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (faction == Faction.BASIC)
            {
                newUnit = Instantiate(unit3, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                type = UnitType.COPTER;
            }
            else if (faction == Faction.CHEAP)
            {
                newUnit = Instantiate(unit6, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                type = UnitType.BOMB;
            }
        }

        if (!Cell.IsTaken && spawnUnit && selected && CanSpawnUnit(newUnit))
        {
            SpawnUnit(newUnit);
            UnitUpdate update = new UnitUpdate();
            update.newLocationX = this.transform.position.x;
            update.newLocationY = this.transform.position.y;
            update.command = UnitUpdateCommand.SPAWN;
            update.type = type;
            NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);
        }
        spawnUnit = false;
    }

    public void InstantiateUnit(UnitType type)
    {
        Unit newUnit = null;
        switch (type)
        {
            case UnitType.FIGHTER:
                newUnit = Instantiate(unit1, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.BOMBER:
                newUnit = Instantiate(unit2, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.COPTER:
                newUnit = Instantiate(unit3, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.CAPCOPTER:
                newUnit = Instantiate(unit4, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.AERIALACE:
                newUnit = Instantiate(unit5, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.BOMB:
                newUnit = Instantiate(unit6, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            default:
                break;
        }

        if (spawnUnit && CanSpawnUnit(newUnit))
        {
            SpawnUnit(newUnit);
        }
        spawnUnit = false;
    }

    private void SpawnUnit(Unit unit)
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
            grid.GetComponent<CustomUnitGenerator>().SpawnUnit(unit, cell);
            grid.GetComponent<CustomUnitGenerator>().SnapToGrid();
            unit.MovementPoints = 0;
        }
    }

    public void SpawnUnitWithButton(int intType)
    {
        Unit newUnit = null;
        UnitType type = (UnitType)intType;
        Airport rax = null;
        foreach (Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
        {
            if (b as Airport && (b as Airport).selected)
            {
                rax = b as Airport;
                break;
            }
        }
        if (rax == null)
            return;
        if (type == UnitType.FIGHTER)
        {
            newUnit = Instantiate(unit1, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.FIGHTER;
        }
        if (type == UnitType.BOMBER)
        {
            newUnit = Instantiate(unit2, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.BOMBER;
        }
        if (type == UnitType.COPTER)
        {
            newUnit = Instantiate(unit3, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.COPTER;
        }
        if (type == UnitType.CAPCOPTER)
        {
            newUnit = Instantiate(unit4, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.AERIALACE)
        {
            newUnit = Instantiate(unit5, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.BOMB)
        {
            newUnit = Instantiate(unit6, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }

        if (!rax.Cell.IsTaken && rax.spawnUnit && rax.selected && rax.CanSpawnUnit(newUnit))
        {
            rax.SpawnUnit(newUnit);
            UnitUpdate update = new UnitUpdate();
            update.newLocationX = rax.transform.position.x;
            update.newLocationY = rax.transform.position.y;
            update.command = UnitUpdateCommand.SPAWN;
            update.type = type;
            NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);
        }
        else
        {
            Destroy(newUnit.gameObject);
            newUnit = null;
        }
        rax.spawnUnit = false;
    }

    public override void OnTurnEnd()
    {
        if (Cell.IsTaken)
        {
            HandleCapture();
        }

        base.OnTurnEnd();
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
        guiCamRef.SetActive(true);
        if(faction == Faction.BASIC)
            airportMenu.SetActive(true);
        if (faction == Faction.CHEAP)
            airportCheapMenu.SetActive(true);
        selected = true;
        base.OnUnitSelected();
    }

    public override void OnUnitDeselected()
    {
        guiCamRef.SetActive(false);
        if (faction == Faction.BASIC)
            airportMenu.SetActive(false);
        if (faction == Faction.CHEAP)
            airportCheapMenu.SetActive(false);
        selected = false;
        base.OnUnitDeselected();
    }
}

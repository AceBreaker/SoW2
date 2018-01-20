using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarracksUnit : Production {

    public GameObject buildingMenu = null;
    public GameObject cheapMenu = null;

    //public Infantry redUnit;
    public Infantry unit1;
    public MechScript unit2;
    public Recon unit3;
    public Tank unit4;
    public MdTank unit5;
    public Artillery unit6;
    public Rocket unit7;
    public APC unit8;
    public AntiAir unit9;
    public Missile unit0;

    //public Infantry redUnit;
    public Android unit11;
    public Sniper unit12;
    public RepairBot unit13;
    public ScrapTank unit14;
    public SonicTank unit15;
    public MegaTank unit16;
    public Cannon unit17;
    public Turret unit18;
    public AAGun unit19;

    bool hasAlreadySpawned = false;

    // Use this for initialization
    void Start () {
        HitPoints = 20;
	}
	
	// Update is called once per frame
	void Update () {
        Unit newUnit = null;
        UnitType type = UnitType.ERROR;
        if(!selected || faction != Faction.BASIC)
        {
            return;
        }
	    if( Input.GetKeyDown(KeyCode.Alpha1))
        {
            newUnit = Instantiate(unit1, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.INFANTRY;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            newUnit = Instantiate(unit2, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.MECH;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            newUnit = Instantiate(unit3, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.RECON;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            newUnit = Instantiate(unit4, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.TANK;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            newUnit = Instantiate(unit5, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.MDTANK;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            newUnit = Instantiate(unit6, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.ARTILLERY;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            newUnit = Instantiate(unit7, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.ROCKETS;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            newUnit = Instantiate(unit0, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.MISSILES;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            newUnit = Instantiate(unit8, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.APC;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            newUnit = Instantiate(unit9, Cell.transform.position, Quaternion.identity);
            spawnUnit = true;
            type = UnitType.ANTIAIR;
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
            unit.SetDeselectedColor();
        }
        unit.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -1.0f);
    }

    public void SpawnUnitWithButton(int intType)
    {
        Unit newUnit = null;
        UnitType type = (UnitType)intType ;
        BarracksUnit rax = null;
        foreach(Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
        {
            if (b as BarracksUnit &&  (b as BarracksUnit).selected)
            {
                rax = b as BarracksUnit;
                break;
            }
        }
        if (rax == null)
            return;
        if (type == UnitType.INFANTRY)//Spawning units here.
        {
            newUnit = Instantiate(unit1, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.INFANTRY;
        }
        if (type == UnitType.MECH)
        {
            newUnit = Instantiate(unit2, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.MECH;
        }
        if (type == UnitType.RECON)
        {
            newUnit = Instantiate(unit3, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.RECON;
        }
        if (type == UnitType.TANK)
        {
            newUnit = Instantiate(unit4, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.TANK;
        }
        if (type == UnitType.MDTANK)
        {
            newUnit = Instantiate(unit5, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.MDTANK;
        }
        if (type == UnitType.ARTILLERY)
        {
            newUnit = Instantiate(unit6, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.ARTILLERY;
        }
        if (type == UnitType.ROCKETS)
        {
            newUnit = Instantiate(unit7, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.ROCKETS;
        }
        if (type == UnitType.MISSILES)
        {
            newUnit = Instantiate(unit0, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.MISSILES;
        }
        if (type == UnitType.APC)
        {
            newUnit = Instantiate(unit8, rax.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.APC;
        }
        if (type == UnitType.ANTIAIR)
        {
            newUnit = Instantiate(unit9, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
            type = UnitType.ANTIAIR;
        }
        if (type == UnitType.ANDROID)
        {
            newUnit = Instantiate(unit11, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.SNIPER)
        {
            newUnit = Instantiate(unit12, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.REPAIR)
        {
            newUnit = Instantiate(unit13, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.ROBOTANK)
        {
            newUnit = Instantiate(unit14, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.SPEEDTANK)
        {
            newUnit = Instantiate(unit15, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.MEGATANK)
        {
            newUnit = Instantiate(unit16, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.CANNON)
        {
            newUnit = Instantiate(unit17, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.AAGUN)
        {
            newUnit = Instantiate(unit19, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }
        if (type == UnitType.TURRET)
        {
            newUnit = Instantiate(unit18, rax.Cell.transform.position, Quaternion.identity);
            rax.spawnUnit = true;
        }

        if (!rax.Cell.IsTaken && rax.spawnUnit && rax.selected && rax.CanSpawnUnit(newUnit))
        {
            rax.hasAlreadySpawned = true;
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

    public void InstantiateUnit(UnitType type)
    {
        Unit newUnit = null;
        CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
        //grid.Units.Find(u => u.is)
            //b => b.Duration == 0
        switch (type)
        {
            case UnitType.INFANTRY:
                newUnit = Instantiate(unit1, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.MECH:
                newUnit = Instantiate(unit2, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.RECON:
                newUnit = Instantiate(unit3, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.TANK:
                newUnit = Instantiate(unit4, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.MDTANK:
                newUnit = Instantiate(unit5, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.ARTILLERY:
                newUnit = Instantiate(unit6, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.ROCKETS:
                newUnit = Instantiate(unit7, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.MISSILES:
                newUnit = Instantiate(unit0, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.APC:
                newUnit = Instantiate(unit8, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.ANTIAIR:
                newUnit = Instantiate(unit9, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;

            case UnitType.ANDROID:
                newUnit = Instantiate(unit11, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.SNIPER:
                newUnit = Instantiate(unit12, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.REPAIR:
                newUnit = Instantiate(unit13, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.ROBOTANK:
                newUnit = Instantiate(unit14, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.SPEEDTANK:
                newUnit = Instantiate(unit15, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.MEGATANK:
                newUnit = Instantiate(unit16, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.CANNON:
                newUnit = Instantiate(unit17, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.TURRET:
                newUnit = Instantiate(unit18, Cell.transform.position, Quaternion.identity);
                spawnUnit = true;
                break;
            case UnitType.AAGUN:
                newUnit = Instantiate(unit19, Cell.transform.position, Quaternion.identity);
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
        transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);
        GetComponent<Renderer>().material = Resources.Load("Barracks", typeof(Material)) as Material;

        City myParent = GetComponent<City>();
        if(myParent != null)
        {
            //unit1 = GetComponent<City>().unit1;

            Cell = myParent.Cell;
        }
        //unit1 = Instantiate()

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

        //HideThis go = GameObject.Find("GUICamera").GetComponent<HideThis>();
        //buildingMenu.SetActive(false);
        //guiCamRef.SetActive(false);
        //guiCamRef.SetActive(true);//
        //buildingMenu.SetActive(true);
        guiCamRef.SetActive(true);//
        if (faction == Faction.BASIC)
            buildingMenu.SetActive(true);
        if (faction == Faction.CHEAP)
            cheapMenu.SetActive(true);
        //guiCamRef.GetComponent<HideThis>().ShowBarracksMenu(true);
        selected = true;
        //GetComponentInChildren<GameObject>().SetActive(true);
        base.OnUnitSelected();
    }

    public override void OnUnitDeselected()
    {
        //HideThis go = GameObject.Find("GUICamera").GetComponent<HideThis>();
        //guiCamRef.GetComponent<HideThis>().HideAll();\
        if(faction == Faction.BASIC)
            buildingMenu.SetActive(false);
        if (faction == Faction.CHEAP)
            cheapMenu.SetActive(false);
        guiCamRef.SetActive(false);
        selected = false;
       // GetComponentInChildren<GameObject>().SetActive(false);
        base.OnUnitDeselected();
    }
}

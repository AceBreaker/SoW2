using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAirport : MonoBehaviour {

    public GameObject[] aStarUnits;
    public GameObject[] iFactionUnits;

    public int aStarIndexMod = 11;
    public int iFactIndexMod = 23;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnAStarWithButton(int unitType)
    {
        UnitType type = (UnitType)unitType;
        BuildSite site = null;
        foreach (Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
        {
            if (b is BuildSite && (b as BuildSite).selected)
            {
                site = b as BuildSite;
                break;
            }
        }

        if (site == null)
            return;

        GameObject newUnit = Instantiate(aStarUnits[unitType - aStarIndexMod], site.transform.position, Quaternion.identity);
        site.spawnUnit = true;

        if (!site.Cell.IsTaken && site.spawnUnit && site.selected && site.CanSpawnUnit(newUnit.GetComponent<Unit>()))
        {
            site.hasAlreadySpawned = true;
            site.SpawnUnit(newUnit.GetComponent<Unit>());
            UnitUpdate update = new UnitUpdate();
            update.newLocationX = site.transform.position.x;
            update.newLocationY = site.transform.position.y;
            update.command = UnitUpdateCommand.SPAWN;
            update.moving = site.unitIndex;
            update.type = type;
            NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);
        }
        else
        {
            Destroy(newUnit.gameObject);
            newUnit = null;
        }
        site.spawnUnit = false;
    }
    public void SpawnAStarFromNetwork(int unitType, int unitIndex)
    {
        UnitType type = (UnitType)unitType;
        BuildSite site = null;
        foreach (Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
        {
            if (b is BuildSite && (b as BuildSite).unitIndex == unitIndex)
            {
                site = b as BuildSite;
                break;
            }
        }

        if (site == null)
            return;

        GameObject newUnit = Instantiate(aStarUnits[unitType - aStarIndexMod], site.transform.position, Quaternion.identity);
        site.spawnUnit = true;

        if (!site.Cell.IsTaken && site.spawnUnit  && site.CanSpawnUnit(newUnit.GetComponent<Unit>()))
        {
            site.hasAlreadySpawned = true;
            site.SpawnUnit(newUnit.GetComponent<Unit>());
        }
        else
        {
            Destroy(newUnit.gameObject);
            newUnit = null;
        }
        site.spawnUnit = false;
    }

    public void SpawniFactionWithButton(int unitType)
    {
        UnitType type = (UnitType)unitType;
        BuildSite site = null;
        foreach (Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
        {
            if (b is BuildSite && (b as BuildSite).selected)
            {
                site = b as BuildSite;
                break;
            }
        }

        if (site == null)
            return;

        GameObject newUnit = Instantiate(iFactionUnits[unitType - iFactIndexMod], site.transform.position, Quaternion.identity);
        site.spawnUnit = true;

        if (!site.Cell.IsTaken && site.spawnUnit && site.selected && site.CanSpawnUnit(newUnit.GetComponent<Unit>()))
        {
            site.hasAlreadySpawned = true;
            site.SpawnUnit(newUnit.GetComponent<Unit>());
            UnitUpdate update = new UnitUpdate();
            update.newLocationX = site.transform.position.x;
            update.newLocationY = site.transform.position.y;
            update.command = UnitUpdateCommand.SPAWN;
            update.moving = site.unitIndex;
            update.type = type;
            NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);
        }
        else
        {
            Destroy(newUnit.gameObject);
            newUnit = null;
        }
        site.spawnUnit = false;
    }

    public void SpawniFactionFromNetwork(int unitType, int unitIndex)
    {
        UnitType type = (UnitType)unitType;
        BuildSite site = null;
        foreach (Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
        {
            if (b is BuildSite && (b as BuildSite).unitIndex == unitIndex)
            {
                site = b as BuildSite;
                break;
            }
        }

        if (site == null)
            return;

        GameObject newUnit = Instantiate(iFactionUnits[unitType - iFactIndexMod], site.transform.position, Quaternion.identity);
        site.spawnUnit = true;

        if (!site.Cell.IsTaken && site.spawnUnit  && site.CanSpawnUnit(newUnit.GetComponent<Unit>()))
        {
            site.hasAlreadySpawned = true;
            site.SpawnUnit(newUnit.GetComponent<Unit>());
        }
        else
        {
            Destroy(newUnit.gameObject);
            newUnit = null;
        }
        site.spawnUnit = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBarracks : MonoBehaviour {

    public GameObject[] aStarUnits;
    public GameObject[] iFactionUnits;
    public GameObject[] usbFactionUnits;

    public int aStarIndexMod = 1;
    public int iFactIndexMod = 14;
    public int usbIndexMod = (int)UnitType.ADVERSARY;

	// Use this for initialization
	void Start () {
        aStarIndexMod = (int)UnitType.INFANTRY;
        iFactIndexMod = (int)UnitType.ANDROID;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnUnitWithButton(int unitType, int factionIndexMod)
    {
        UnitType type = (UnitType)(unitType);
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

        Debug.Log(unitType.ToString() + " + " + factionIndexMod.ToString());
        GameObject newUnit = CreateNewUnit(factionIndexMod, site, type);
        //GameObject newUnit = Instantiate(aStarUnits[unitType - factionIndexMod], site.transform.position, Quaternion.identity);
        type = (UnitType)(unitType + factionIndexMod);
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

    public void SpawnUnitFromNetwork(int unitType, int unitIndex, int factionIndexMod)
    {
        UnitType type = (UnitType)(unitType - 1);
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
        GameObject newUnit = CreateNewUnit(factionIndexMod, site, type);
        
        site.spawnUnit = true;

        if (!site.Cell.IsTaken && site.spawnUnit && site.CanSpawnUnit(newUnit.GetComponent<Unit>()))
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

    GameObject CreateNewUnit(int factionIndex, BuildSite site, UnitType type)
    {
        int index = (int)type + (factionIndex);

        if (factionIndex >= usbIndexMod)
            return Instantiate(usbFactionUnits[index - usbIndexMod], site.transform.position, Quaternion.identity);
        else if (factionIndex >= iFactIndexMod)
            return Instantiate(iFactionUnits[index - iFactIndexMod], site.transform.position, Quaternion.identity);
        else if (factionIndex >= aStarIndexMod)
            return Instantiate(aStarUnits[index - aStarIndexMod], site.transform.position, Quaternion.identity);
        return null;
    }

    bool IsAStar(int indexMod)
    {
        return true;
    }

    public void SpawnAStarWithButton(int unitType)
    {
        UnitType type = (UnitType)unitType;
        BuildSite site = null;
        foreach(Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
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

        if (!site.Cell.IsTaken && site.spawnUnit && site.CanSpawnUnit(newUnit.GetComponent<Unit>()))
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
        newUnit.transform.position = new Vector3(newUnit.transform.position.x, newUnit.transform.position.y, -1.0f);

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
        newUnit.transform.position = new Vector3(newUnit.transform.position.x, newUnit.transform.position.y, -1.0f);

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

    public void SpawnusbFromNetwork(int unitType, int unitIndex)
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

        GameObject newUnit = Instantiate(usbFactionUnits[unitType - usbIndexMod], site.transform.position, Quaternion.identity);
        site.spawnUnit = true;
        newUnit.transform.position = new Vector3(newUnit.transform.position.x, newUnit.transform.position.y, -1.0f);

        if (!site.Cell.IsTaken && site.spawnUnit && site.CanSpawnUnit(newUnit.GetComponent<Unit>()))
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

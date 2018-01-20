using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SampleSquare2 : Square {

    public Color starting;
    public string tileName = "";
    public Building myBuilding = null;
	// Use this for initialization
	void Start () {
        float posX = transform.position.x;
        float posY = transform.position.y;
        starting = GetComponent<Renderer>().material.color;
        if (GetComponent<Renderer>().material.name == "Brown (Instance)")
            tileName = "Mountain";
        if (GetComponent<Renderer>().material.name == "Lime (Instance)")
            tileName = "Plains";
        if (GetComponent<Renderer>().material.name == "Green (Instance)")
            tileName = "Woods";
        if (GetComponent<Renderer>().material.name == "Gray (Instance)")
            tileName = "Road";
        if (GetComponent<Renderer>().material.name == "Blue (Instance)")
            tileName = "River";

        this.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public Unit GetMyOccupyingBuilding()
    {
        CellGrid grid = GetComponentInParent<CellGrid>();
        foreach (Unit b in grid.Units)
        {
            if (b.transform.position.x == this.transform.position.x && b.transform.position.y == this.transform.position.y && (b is Building))
            {
                return b;
            }
        }
        return null;
    }

    public Unit GetMyOccupyingUnit()
    {
        CellGrid grid = GetComponentInParent<CellGrid>();
        foreach (Unit b in grid.Units)
        {
            if (b.transform.position.x == this.transform.position.x && b.transform.position.y == this.transform.position.y && !(b is Building))
            {
                return b;
            }
        }
        return null;
    }

    public override Vector3 GetCellDimensions()
    {
        return GetComponent<Renderer>().bounds.size;
    }

    public override void MarkAsHighlighted()
    {
        ShowTileInfo();
        GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
        //GetComponent<Renderer>().material.color = new Color(0.75f, 0.75f, 0.75f);
    }

    public void ShowTileInfo()
    {
        Text tileInfo = GameObject.Find("TileInfo").GetComponent<Text>();
        tileInfo.text = "Tile name: " + tileName + "\n" +
            "Defense: " + defenseValue.ToString() +"\n" +
            (myBuilding != null ? "Capture: " + myBuilding.HitPoints.ToString() + "/20\n" :"")+ 
            "\nMove Costs: " + 
            "\nInf: " + (FootCost<100?FootCost.ToString():"n/a") +
            "\nMech: " +  (MechCost<100? MechCost.ToString():"n/a")+
            "\nTread: " + (TreadCost<100? TreadCost.ToString():"n/a") +
            "\nTires: " + (TiresCost < 100 ? TiresCost.ToString() : "n/a");  
    }

    public override void MarkAsPath()
    {
        GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 1.0f);
    }

    public override void MarkAsReachable()
    {
        GetComponent<Renderer>().material.color = Color.cyan;
    }

    public override void UnMark()
    {
        GetComponent<Renderer>().material.color = starting;
    }
}

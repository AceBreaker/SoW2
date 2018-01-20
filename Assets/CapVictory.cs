using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapVictory : Building {

    public CellGrid cellGrid = null;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    }

    public override void Initialize()
    {
        if(cellGrid == null)
            cellGrid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
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

    public override void HandleCapture()
    {
        base.HandleCapture();
        cellGrid.CheckForCapVictoryEndGame();
    }
}

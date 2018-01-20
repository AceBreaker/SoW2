using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIController2 : MonoBehaviour {
    public CellGrid cellGrid;
    public SampleUnit2 redUnit;
    public SampleUnit2 blueUnit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if((DarkRift.DarkRiftAPI.isConnected && DarkRift.DarkRiftAPI.id-1 == GameObject.Find("CellGrid").GetComponent<CellGrid>().CurrentPlayerNumber || Unit.debugoverride) &&
            Input.GetKeyDown(KeyCode.N))
        {
            UnitUpdate update = new UnitUpdate();
            update.command = UnitUpdateCommand.END_TURN;
            NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);
            cellGrid.EndTurn();//User ends hit turn by pressing the N key
        }
        //else if(Input.GetKeyDown(KeyCode.U))
        //{
        //    var o = Instantiate(redUnit, cellGrid.Cells[0].transform.position, Quaternion.identity);
        //    ((SampleUnit2)(o)).transform.parent = GameObject.Find("Units Parent").transform;
        //    //cellGrid.SpawnMoreUnits();
        //    List<Cell> cells = new List<Cell>();
        //    cells.Add(cellGrid.Cells[0]);
        //    cellGrid.GetComponent<CustomUnitGenerator>().SpawnUnit((Unit)o, cellGrid.Cells[0]);
        //    cellGrid.GetComponent<CustomUnitGenerator>().SnapToGrid() ;
        //}
	}

    public void EndTurn()
    {
        if (DarkRift.DarkRiftAPI.isConnected &&
            DarkRift.DarkRiftAPI.id - 1 == GameObject.Find("CellGrid").GetComponent<CellGrid>().CurrentPlayerNumber ||
            Unit.debugoverride)
        {
            UnitUpdate update = new UnitUpdate();
            update.command = UnitUpdateCommand.END_TURN;
            NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);
            cellGrid.EndTurn();//User ends hit turn by pressing the N key
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

class HumanPlayer : Player
{
    //List<Unit> units = null;
    //public override void OnStartLocalPlayer()
    //{
    //    GameObject playersParent = GameObject.Find("Players Parent");

    //    int i = 0;
    //    foreach(Player p in  playersParent.GetComponentsInChildren<Player>())
    //    {
    //        p.PlayerNumber = i++;
    //    }

    //    this.transform.parent = playersParent.transform;
    //    units = new List<Unit>();
    //    foreach(Unit go in GameObject.Find("Units Parent").GetComponentsInChildren<Unit>())
    //    {
    //        if(go.PlayerNumber == this.PlayerNumber)
    //        {
    //            units.Add(go);
    //        }
    //    }

    //    base.OnStartLocalPlayer();

    //    GameObject.Find("CellGrid").GetComponent<CellGrid>().InitializeGame();
    //}

    //public override void OnStartClient()
    //{
    //    GameObject playersParent = GameObject.Find("Players Parent");

    //    int i = 0;
    //    foreach (Player p in playersParent.GetComponentsInChildren<Player>())
    //    {
    //        p.PlayerNumber = i++;
    //    }

    //    this.transform.parent = playersParent.transform;
    //    units = new List<Unit>();
    //    foreach (Unit go in GameObject.Find("Units Parent").GetComponentsInChildren<Unit>())
    //    {
    //        if (go.PlayerNumber == this.PlayerNumber)
    //        {
    //            units.Add(go);
    //        }
    //    }
    //    base.OnStartClient();

    //    GameObject.Find("CellGrid").GetComponent<CellGrid>().InitializeGame();
    //}

    public override void Play(CellGrid cellGrid)
    {


        cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid);
        income = 0;
        GameObject.Find("CellGrid").GetComponent<CellGrid>().Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).ForEach(u => { if (u is BarracksUnit) income += 1000; });
        GameObject.Find("CellGrid").GetComponent<CellGrid>().Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).ForEach(u => { if (u is City) income += (u as City).moneyGen; });
        GameObject.Find("CellGrid").GetComponent<CellGrid>().Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).ForEach(u => { if (u is HeadQuarters) income += 1000; });
        GameObject.Find("CellGrid").GetComponent<CellGrid>().Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).ForEach(u => { if (u is Airport) income += 1000; });
        GameObject.Find("CellGrid").GetComponent<CellGrid>().Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).ForEach(u => { if (u is BuildSite && (u as BuildSite).buildingType == MatIndex.City) income += (u as BuildSite).moneyGen; });
        Money += income;
        GameObject.Find("moneyPanel").GetComponentsInChildren<Text>()[PlayerNumber].text = Money.ToString() + "G" +"(+"+income.ToString()+"G)";
        PlayerController.currTurnMoney = Money;
    }
}
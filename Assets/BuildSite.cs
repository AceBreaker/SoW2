using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSite : Production {

    public MatIndex buildingType = MatIndex.BuildSite;
    public GameObject basicRaxMenu = null;
    public GameObject cheapRaxMenu = null;

    public GameObject basicAirportMenu = null;
    public GameObject cheapAirportMenu = null;

    public GameObject cityMenu = null;

    public GameObject buildSiteMenu = null;

    public GameObject dynamicBuildingMenu = null;

    public int moneyGen = 1000;

    public GameObject playersList;

    void Awake()
    {
        dynamicBuildingMenu = GameObject.Find("DynamicBuildingMenu");
    }

    // Use this for initialization
    void Start () {
        TotalActionPoints = 1;
        playersList = GameObject.Find("Players Parent");

        
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void OnUnitSelected()
    {
        guiCamRef.SetActive(true);
        ShowCurrentMenu();
        

        base.OnUnitSelected();
    }

    public override void OnUnitDeselected()
    {
        HideMenu();
        base.OnUnitDeselected();
    }

    protected void ShowCurrentMenu()
    {
        HumanPlayer myPlayer = null;
        int playersCount = playersList.transform.childCount;
        for(int i = 0; i < playersCount; i++)
        {
            myPlayer = playersList.transform.GetChild(i).GetComponent<HumanPlayer>();
            if (myPlayer.PlayerNumber == this.PlayerNumber)
            {
                break;
            }
        }

        if (myPlayer == null)
            return;
        
        switch (buildingType)
        {
            case MatIndex.BuildSite:
                buildSiteMenu.SetActive(true);
                break;
            case MatIndex.City:
                cityMenu.SetActive(true);
                cityMenu.GetComponentInChildren<Text>().text = "Income Addition " + (moneyGen * (moneyGen / 1000)).ToString();
                selected = true;
                break;
            case MatIndex.Barracks:
                dynamicBuildingMenu.SetActive(true);
                Debug.LogError(myPlayer.name);
                Debug.LogError(myPlayer.transform.GetChild(0).name);
                Debug.LogError(myPlayer.transform.GetChild(0).GetChild(0).name);
                dynamicBuildingMenu.GetComponent<DynamicBuildingMenu>().CreateMenu(myPlayer.gameObject.transform.GetChild(0).GetChild(0).gameObject);
                break;
            case MatIndex.Airport:
                dynamicBuildingMenu.SetActive(true);
                dynamicBuildingMenu.GetComponent<DynamicBuildingMenu>().CreateMenu(myPlayer.gameObject.transform.GetChild(0).GetChild(1).gameObject);
                break;
            default:
                break;
        }
       
        //if(faction == Faction.BASIC)
        //{
        //    switch(buildingType)
        //    {
        //        case MatIndex.BuildSite:
        //            buildSiteMenu.SetActive(true);
        //            break;
        //        case MatIndex.City:
        //            cityMenu.SetActive(true);
        //            cityMenu.GetComponentInChildren<Text>().text = "Income Addition " + (moneyGen * (moneyGen / 1000)).ToString();
        //            selected = true;
        //            break;
        //        case MatIndex.Barracks:
        //            basicRaxMenu.SetActive(true);
        //            break;
        //        case MatIndex.Airport:
        //            basicAirportMenu.SetActive(true);
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //else if(faction == Faction.CHEAP)
        //{
        //    switch (buildingType)
        //    {
        //        case MatIndex.BuildSite:
        //            buildSiteMenu.SetActive(true);
        //            break;
        //        case MatIndex.City:
        //            cityMenu.SetActive(true);
        //            cityMenu.GetComponentInChildren<Text>().text = "Income Addition " + (moneyGen * (moneyGen / 1000)).ToString();
        //            selected = true;
        //            break;
        //        case MatIndex.Barracks:
        //            cheapRaxMenu.SetActive(true);
        //            break;
        //        case MatIndex.Airport:
        //            cheapAirportMenu.SetActive(true);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }

    public void HideMenu()
    {
        cityMenu.SetActive(false);
        basicRaxMenu.SetActive(false);
        cheapRaxMenu.SetActive(false);
        basicAirportMenu.SetActive(false);
        cheapAirportMenu.SetActive(false);
        buildSiteMenu.SetActive(false);
        if(dynamicBuildingMenu != null)
            dynamicBuildingMenu.SetActive(false);

        guiCamRef.SetActive(false);
    }

    public int IncreaseIncomeCost()
    {
        return moneyGen * (moneyGen / 1000);
    }

    public bool CanIncreaseMoneyGen()
    {
        bool result = false;
        CellGrid cg = GameObject.Find("CellGrid").GetComponent<CellGrid>();
        foreach (Player p in cg.Players)
        {
            if (p.PlayerNumber == this.PlayerNumber && p.Money >= IncreaseIncomeCost())
            {
                return true;
            }
        }

        return result;
    }

    public void SelfIncreaseMoneyGenIfAble()
    {
        CellGrid cg = GameObject.Find("CellGrid").GetComponent<CellGrid>();
        if (CanIncreaseMoneyGen())
        {
            foreach (Player p in cg.Players)
            {
                if (p.PlayerNumber == PlayerNumber)
                {
                    p.Money -= IncreaseIncomeCost();
                    PlayerController.currTurnMoney = p.Money;
                    cg.Players[PlayerNumber].income += 1000;
                    GameObject.Find("Canvas").GetComponentsInChildren<Text>()[PlayerNumber].text = PlayerController.currTurnMoney.ToString() + "G" + "(+" + cg.Players[PlayerNumber].income.ToString() + "G)";
                    moneyGen += 1000;
                    //send message to other players to let them know that the city has more money gen and this player has spent money

                    ActionPoints = 0;
                    break;
                }
            }
        }
        else
        {
            //play error sound
        }
    }

    public void SendIncreaseMoneyCommand()
    {
        UnitUpdate update = new UnitUpdate();
        update.command = UnitUpdateCommand.INCOME;
        update.moving = this.unitIndex;
        NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);
    }

    public override void OnTurnEnd()
    {
        if(Cell.IsTaken)
        {
            HandleCapture();
        }
            ActionPoints = 1;
        base.OnTurnEnd();
    }

    public override void OnTurnStart()
    {
        ActionPoints = 1;
        base.OnTurnStart();
    }
    
    public bool CanAffordToBuild(int cost)
    {
        if(PlayerController.currTurnMoney >= cost)
        {
            return true;
        }
        return false;
    }
    
}

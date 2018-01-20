using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSiteMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleBuildAction(int index)
    {
        BuildSite site = null;

        foreach(Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
        {
            if(b is BuildSite && (b as BuildSite).selected)
            {
                site = b as BuildSite;
                break;
            }
        }

        if (site == null)
            return;

        //handle cost
        if((DarkRift.DarkRiftAPI.isConnected && !site.CanAffordToBuild(NetManager.buildCosts[index-1])))
        {
            return;
        }

        site.grid.Players[site.PlayerNumber].Money -= NetManager.buildCosts[index-1];
        PlayerController.currTurnMoney = site.grid.Players[site.PlayerNumber].Money;
        SetAvailableFundsText(GameObject.Find("Canvas").GetComponentsInChildren<Text>(), site);
        //GameObject.Find("Canvas").GetComponentsInChildren<Text>()[site.PlayerNumber].text = PlayerController.currTurnMoney.ToString() + "G" + "(+" + site.grid.Players[site.PlayerNumber].income.ToString() + "G)";

        site.buildingType = (MatIndex)index;
        site.GetComponent<ChangeMaterial>().SetNewMaterial((MatIndex)index);

        UnitUpdate update = new UnitUpdate();
        update.command = UnitUpdateCommand.BUILDSITE;
        update.moving = site.unitIndex;
        update.buildingType = (MatIndex)index;
        NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);

        site.OnUnitDeselected();
    }

    public void SetAvailableFundsText(Text[] t, BuildSite site)
    {
        t[site.PlayerNumber].text = PlayerController.currTurnMoney.ToString() + "G" + "(+" + site.grid.Players[site.PlayerNumber].income.ToString() + "G)";
    }

    public void IncreaseMoneyGenIfAble()
    {
        BuildSite rax = null;
        foreach (Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
        {
            if ((b as BuildSite && (b as BuildSite).selected))
            {
                rax = b as BuildSite;
                break;
            }

        }
        if (rax == null)
            return;

        if (rax.ActionPoints > 0)
        {
            rax.SelfIncreaseMoneyGenIfAble();
            rax.SendIncreaseMoneyCommand();
        }
    }
}

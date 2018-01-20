using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class City : Building {
    
    public CellGrid cellGrid;

    public GameObject guiCamRef = null;
    public GameObject cityMenu = null;

    public bool selected = false;

    //public Infantry unit1;

    //public GameObject buildingMenu;
    //public GameObject guiCamRef;

    // Use this for initialization
    void Start () {
        HitPoints = 20;
        ActionPoints = 1;
	}
	
	// Update is called once per frame
	void Update () {
        bool spawnUnit = false;
        Unit newUnit = null;
        //if (Input.GetKeyDown(KeyCode.U) && selected)
        //{
        //    BarracksUnit bu = (gameObject.AddComponent(typeof(BarracksUnit)) as BarracksUnit);
        //    bu.Initialize();
        //    bu.buildingMenu = this.buildingMenu;
        //    bu.guiCamRef = this.guiCamRef;

        //    grid.Units.Remove(this);
        //    grid.Units.Add(bu);

        //    Destroy(this);
        //}
	}

    public override void OnTurnEnd()
    {
        if(Cell.IsTaken)
        {
            HandleCapture();
            ActionPoints = 1;
        }

        base.OnTurnEnd();
    }
    
    public override void Initialize()
    {
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

    public int moneyGen = 1000;

    public void IncreaseMoneyGenIfAble()
    {
        City rax = null;
        foreach (Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
        {
            if ((b as City && (b as City).selected))
            {
                rax = b as City;
                break;
            }

        }
        if (rax == null)
            return;

        if(rax.ActionPoints > 0)
        {
            rax.SelfIncreaseMoneyGenIfAble();
            rax.SendIncreaseMoneyCommand();
        }
    }

    public override string ReturnUnitInfo()
    {
        string result = unitName +
            "\nCurrent income: " + moneyGen.ToString()
            + "\nUnit index " + unitIndex.ToString();
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

    private void SendIncreaseMoneyCommand()
    {
        UnitUpdate update = new UnitUpdate();
        update.command = UnitUpdateCommand.INCOME;
        update.moving = this.unitIndex;
        NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);
    }

    public bool CanIncreaseMoneyGen()
    {
        bool result = false;
        CellGrid cg = GameObject.Find("CellGrid").GetComponent<CellGrid>();
        foreach (Player p in cg.Players)
        {
            if(p.PlayerNumber == this.PlayerNumber && p.Money >= IncreaseIncomeCost())
            {
                return true;
            }
        }

        return result;
    }

    public int IncreaseIncomeCost()
    {
        return moneyGen * (moneyGen / 1000);
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
        guiCamRef.SetActive(true);//
        cityMenu.SetActive(true);
        cityMenu.GetComponentInChildren<Text>().text = "Income Addition " + (moneyGen * (moneyGen / 1000)).ToString();
        selected = true;
        base.OnUnitSelected();
    }

    public override void OnUnitDeselected()
    {
        cityMenu.SetActive(false);
        guiCamRef.SetActive(false);
        selected = false;
        base.OnUnitDeselected();
    }
}

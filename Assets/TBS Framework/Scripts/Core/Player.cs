using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Networking;

public abstract class Player : MonoBehaviour
{
    public int PlayerNumber;
    
    public int income = 0;

    public bool eliminated = false;

    public int Money;

    public GameObject myFaction = null;

    public int myFactionIndex = -1;

    public int myFactionGroundIndexMod = -1;
    public int myFactionAirIndexMod = -1;

    /// <summary>
    /// Method is called every turn. Allows player to interact with his units.
    /// </summary>         
    public abstract void Play(CellGrid cellGrid);

    public void EliminatePlayer(bool capture, Unit defeater)
    {
        eliminated = true;
        if(capture)
        {
            EliminateByRout(defeater);
        }
        else
        {
            EliminateByArmyDefeat();
        }
        
        //TODO convert all buildings to white or new owner
    }

    private void EliminateByArmyDefeat()
    {
        CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
        foreach (Unit u in grid.Units)
        {
            if (u is Building && u.PlayerNumber == this.PlayerNumber)
            {
                u.PlayerNumber = -1;
            }
        }
    }

    private void EliminateByRout(Unit defeater)
    {
        CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
        foreach (Unit u in grid.Units)
        {
            if (u is Building && u.PlayerNumber == this.PlayerNumber)
            {
                u.PlayerNumber = defeater.PlayerNumber;
                (u as Building).UpdateColor();
                u.GetComponent<Renderer>().material.color = (u as Unit).LeadingColor;
            }
            else if(u.PlayerNumber == this.PlayerNumber)
            {
                u.DestroyUnit(defeater, 100);
            }
        }
        grid.CheckForEndGame();
    }
}
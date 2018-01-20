using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// CellGrid class keeps track of the game, stores cells, units and players objects. It starts the game and makes turn transitions. 
/// It reacts to user interacting with units or cells, and raises events related to game progress. 
/// </summary>
public class CellGrid : MonoBehaviour
{
    public event EventHandler GameStarted;
    public event EventHandler GameEnded;
    public event EventHandler TurnEnded;

    public GameObject woodsDecoration = null;

    string barracksInfo = "";
    public static int capPoints = 20;
    
    private CellGridState _cellGridState;//The grid delegates some of its behaviours to cellGridState object.
    public CellGridState CellGridState
    {
        private get
        {
            return _cellGridState;
        }
        set
        {
            if(_cellGridState != null)
                _cellGridState.OnStateExit();
            _cellGridState = value;
            _cellGridState.OnStateEnter();
        }
    }

    public int NumberOfPlayers { get; private set; }

    public Player CurrentPlayer
    {
        get { return Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)); }
    }
    public int CurrentPlayerNumber { get; private set; }

    public Transform PlayersParent;

    public List<Player> Players { get; private set; }
    public List<Cell> Cells { get; private set; }
    public List<Unit> Units { get; private set; }

    public static bool GameOver = false;

    string[] armyNames;

    void Start()
    {
        Players = new List<Player>();
        for (int i = 0; i < PlayersParent.childCount; i++)
        {
            var player = PlayersParent.GetChild(i).GetComponent<Player>();
            if (player != null)
                Players.Add(player);
            else
                Debug.LogError("Invalid object in Players Parent game object");
        }

        armyNames = new string[6];
        armyNames[0] = "RED";
        armyNames[1] = "BLUE";
        armyNames[2] = "YELLOW";
        armyNames[3] = "GREEN";
        armyNames[4] = "ORANGE";
        armyNames[5] = "PURPLE";


        //TODO rework to be as many players as I want... maybe
        //if (NumberOfPlayers < 2)
        //  return;
        NumberOfPlayers = Players.Count;
        CurrentPlayerNumber = Players.Min(p => p.PlayerNumber);

        Cells = new List<Cell>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cell = transform.GetChild(i).gameObject.GetComponent<Cell>();
            if (cell != null)
                Cells.Add(cell);
            else
                Debug.LogError("Invalid object in cells paretn game object");
        }
      
        foreach (var cell in Cells)
        {
            cell.CellClicked += OnCellClicked;
            cell.CellHighlighted += OnCellHighlighted;
            cell.CellDehighlighted += OnCellDehighlighted;
        }
             
        var unitGenerator = GetComponent<IUnitGenerator>();
        if (unitGenerator != null)
        {
            Units = unitGenerator.SpawnUnits(Cells);
            foreach (var unit in Units)
            {
                unit.UnitClicked += OnUnitClicked;
                unit.UnitDestroyed += OnUnitDestroyed;
                unit.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -1.0f);
            }
        }
        else
            Debug.LogError("No IUnitGenerator script attached to cell grid");

        GameOver = false;

        StartGame();

        barracksInfo = "With Barracks Selected:\n" +
            "1: Infantry 1000\n" +
            "2: Mech 3000\n" +
            "3: Recon 4000\n" +
            "4: Tank 7000\n" +
            "5: MdTank 16000\n" +
            "6: Artillery 6000\n" +
            "7: Rockets 15000\n" +
            "8: Missiles 12000\n" +
            "9: APC 5000\n" +
            "0: AAir 8000\n";

        foreach (SampleSquare2 c in Cells)
        {
            string blerg = c.GetComponent<Renderer>().material.name;
            if (c.GetComponent<Renderer>().material.name == "Lime (Instance)")
            {
                c.MovementCost = 1;
                c.FootCost = 1;
                c.MechCost = 1;
                c.TiresCost = 2;
                c.TreadCost = 1;
                c.defenseValue = 1;

                CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
                //foreach(Building b in grid.Units)
                for (int i = 0; i < Units.Count; i++)
                {
                    if (grid.Units[i].transform.position.x == c.transform.position.x && grid.Units[i].transform.position.y == c.transform.position.y)
                    {
                        if (Units[i] is BarracksUnit)
                            c.tileName = "Barracks";
                        if (grid.Units[i] is Airport)
                            c.tileName = "Airport";
                        if (grid.Units[i] is City)
                            c.tileName = "City";
                        if (grid.Units[i] is HeadQuarters)
                            c.tileName = "HQ";
                        if (grid.Units[i] is CapVictory)
                            c.tileName = "Capture this";

                        c.defenseValue = 3;
                        c.FootCost = 1;
                        c.MechCost = 1;
                        c.TreadCost = 1;
                        c.TiresCost = 1;
                        c.myBuilding = Units[i] as Building;
                        break;
                    }
                }
            }
            else if (c.GetComponent<Renderer>().material.name == "Green (Instance)")
            {
                c.MovementCost = 1;
                c.FootCost = 1;
                c.MechCost = 1;
                c.TiresCost = 3;
                c.TreadCost = 2;
                c.defenseValue = 2;

                Vector3 decorationPosition = new Vector3(c.transform.position.x, c.transform.position.y, -0.5f);
                if (woodsDecoration != null)
                {
                    GameObject caught = Instantiate(woodsDecoration, c.transform);
                    caught.transform.Rotate(90.0f, 0.0f, 0.0f);
                    caught.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, -0.5f);
                    caught.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                }

            }
            else if (c.GetComponent<Renderer>().material.name == "Brown (Instance)")
            {
                c.MovementCost = 1;
                c.FootCost = 2;
                c.MechCost = 1;
                c.TiresCost = 100;
                c.TreadCost = 100;
                c.defenseValue = 4;
            }
            else if (c.GetComponent<Renderer>().material.name == "Gray (Instance)")
            {
                c.MovementCost = 1;
                c.FootCost = 1;
                c.MechCost = 1;
                c.TiresCost = 1;
                c.TreadCost = 1;
                c.defenseValue = 0;
            }
            else if (c.GetComponent<Renderer>().material.name == "Blue (Instance)")
            {
                c.MovementCost = 1;
                c.FootCost = 2;
                c.MechCost = 1;
                c.TiresCost = 100;
                c.TreadCost = 100;
                c.defenseValue = 0;
            }
        }

        GameObject.Find("Info").GetComponent<Text>().text = "you are " + (DarkRift.DarkRiftAPI.isConnected?armyNames[DarkRift.DarkRiftAPI.id - 1]:"debug") + "\n" + "currTurn: " + armyNames[CurrentPlayerNumber] + "\n" + barracksInfo;
    }

    void Update()
    {
        if(GameOver && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            SceneManager.LoadScene("CSS");
        }
        if(Input.GetKeyDown(KeyCode.F9))
        {
            Unit.debugoverride = !Unit.debugoverride;
        }

        //FogTiles();
        //List<Cell> cellsInVision;
        //foreach (Unit u in Units)
        //{
        //    if ((DarkRift.DarkRiftAPI.isConnected && u.PlayerNumber == DarkRift.DarkRiftAPI.id - 1) ||
        //            !DarkRift.DarkRiftAPI.isConnected && u.PlayerNumber == 0)
        //    {
        //        cellsInVision = u.GetCellsInVision();
        //        foreach (Cell c in cellsInVision)
        //        {
        //            c.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
        //        }
        //    }
        //}
    }

    public void FogTiles()
    {
        foreach(Cell c in Cells)
        {
            c.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void InitializeGame()
    {
        Players = new List<Player>();
        for (int i = 0; i < PlayersParent.childCount; i++)
        {
            var player = PlayersParent.GetChild(i).GetComponent<Player>();
            if (player != null)
                Players.Add(player);
            else
                Debug.LogError("Invalid object in Players Parent game object");
        }
        NumberOfPlayers = Players.Count;
        CurrentPlayerNumber = Players.Min(p => p.PlayerNumber);

        Cells = new List<Cell>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cell = transform.GetChild(i).gameObject.GetComponent<Cell>();
            if (cell != null)
                Cells.Add(cell);
            else
                Debug.LogError("Invalid object in cells paretn game object");
        }

        foreach (var cell in Cells)
        {
            cell.CellClicked += OnCellClicked;
            cell.CellHighlighted += OnCellHighlighted;
            cell.CellDehighlighted += OnCellDehighlighted;
        }

        var unitGenerator = GetComponent<IUnitGenerator>();
        if (unitGenerator != null)
        {
            Units = unitGenerator.SpawnUnits(Cells);
            foreach (var unit in Units)
            {
                unit.UnitClicked += OnUnitClicked;
                unit.UnitDestroyed += OnUnitDestroyed;
            }
        }
        else
            Debug.LogError("No IUnitGenerator script attached to cell grid");

        StartGame();

        foreach(Cell c in Cells)
        {
            if(c.GetComponent<Renderer>().material.name == "Lime")
            {
                c.MovementCost = 1;
                c.FootCost = 1;
                c.MechCost = 1;
                c.TiresCost = 2;
                c.TreadCost = 1;
            }
            else if (c.GetComponent<Renderer>().material.name == "Green")
            {
                c.MovementCost = 1;
                c.FootCost = 1;
                c.MechCost = 1;
                c.TiresCost = 3;
                c.TreadCost = 2;
            }
        }
    }

    public void SpawnMoreUnits()
    {
        var unitGenerator = GetComponent<IUnitGenerator>();
        if (unitGenerator != null)
        {
            Units = unitGenerator.SpawnUnits(Cells);
            foreach (var unit in Units)
            {
                unit.UnitClicked += OnUnitClicked;
                unit.UnitDestroyed += OnUnitDestroyed;
            }
        }
    }

    private void OnCellDehighlighted(object sender, EventArgs e)
    {
       // if(CellGridState != null)
            CellGridState.OnCellDeselected(sender as Cell);
    }
    private void OnCellHighlighted(object sender, EventArgs e)
    {
        //if(CellGridState != null)
            CellGridState.OnCellSelected(sender as Cell);
    } 
    private void OnCellClicked(object sender, EventArgs e)
    {
        CellGridState.OnCellClicked(sender as Cell);
    }

    private void OnUnitClicked(object sender, EventArgs e)
    {
        CellGridState.OnUnitClicked(sender as Unit);
    }

    public void SpawnNewUnit(Unit unit)
    {
        unit.UnitClicked += OnUnitClicked;
        unit.UnitDestroyed += OnUnitDestroyed;
    }

    private void OnUnitDestroyed(object sender, AttackEventArgs e)
    {
        Units.Remove(sender as Unit);
        var totalPlayersAlive = Units.Select(u => u.PlayerNumber).Distinct().ToList(); //Checking if the game is over
        if (totalPlayersAlive.Count == 1)
        {
            if(GameEnded != null)
                GameEnded.Invoke(this, new EventArgs());
        }
    }
    
    /// <summary>
    /// Method is called once, at the beggining of the game.
    /// </summary>
    public void StartGame()
    {
        if(GameStarted != null)
            GameStarted.Invoke(this, new EventArgs());
        
        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnStart(); });
        Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)).Play(this);

        if (NetManager.playerCount == 1)
        {
            
        }
    }
    /// <summary>
    /// Method makes turn transitions. It is called by player at the end of his turn.
    /// </summary>
    public void EndTurn()
    {
        if (Units.Select(u => u.PlayerNumber).Distinct().Count() == 1)
        {
            return;
        }
        CellGridState = new CellGridStateTurnChanging(this);

        Units.FindAll(u => (!u.PlayerNumber.Equals(CurrentPlayerNumber))).ForEach(u => { u.HandleCapture(); });
        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnEnd(); });

        CurrentPlayerNumber = (CurrentPlayerNumber + 1) % NumberOfPlayers;
        while(Players.FindAll(p => p.PlayerNumber.Equals(CurrentPlayerNumber))[0].eliminated)
        {
            CurrentPlayerNumber = (CurrentPlayerNumber + 1)%NumberOfPlayers;
        }
        //while (Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).Count == 0)
        //{
        //    CurrentPlayerNumber = (CurrentPlayerNumber + 1)%NumberOfPlayers;
        //}//Skipping players that are defeated.

        if (TurnEnded != null)
            TurnEnded.Invoke(this, new EventArgs());

        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnStart(); });
        Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)).Play(this);
        GameObject.Find("TurnChange").GetComponent<AudioSource>().Play();
        string info = GameObject.Find("Info").GetComponent<Text>().text;
        GameObject.Find("Info").GetComponent<Text>().text = "you are " + (DarkRift.DarkRiftAPI.isConnected? armyNames[DarkRift.DarkRiftAPI.id - 1]:"RED")
            + "\n" + "currTurn: " + armyNames[CurrentPlayerNumber] 
            + "\n" + barracksInfo 
            + "\n" + "Current Capture: " + (capPoints >= 0?capPoints.ToString():"")
            + "\nmessagecount: " + NetManager.messageIndex;
    }

    public void CheckForEndGame()
    {
        if(Players.FindAll(p => p.eliminated == false).Count == 1)
        {
            GameObject.Find("Info").GetComponent<Text>().text = "game over " + armyNames[(DarkRift.DarkRiftAPI.isConnected? DarkRift.DarkRiftAPI.id - 1:0)] + " Wins!\nPress enter to exit";
            GameOver = true;
        }
        else
        {
            CheckForCapVictoryEndGame();
        }
    }

    public void CheckForCapVictoryEndGame()
    {
        int currentOwnerPlayerNumber = -1;
        int victory = -1;
        List<Unit> asdf = Units.FindAll(p => p is CapVictory);

        for(int i = 0; i < Players.Count; i++)
        {
            if(asdf.FindAll(p => p.PlayerNumber.Equals(i)).Count == asdf.Count)
            {
                victory = i;
                break;
            }
        }

        if (victory == -1)
            return;
        GameObject.Find("Info").GetComponent<Text>().text = "game over " + armyNames[victory] + " Wins!\nPress enter to exit";
        GameOver = true;
    }

    public void UpdateInfo()
    {
        string info = GameObject.Find("Info").GetComponent<Text>().text;
        GameObject.Find("Info").GetComponent<Text>().text = "you are " + (DarkRift.DarkRiftAPI.isConnected ? armyNames[DarkRift.DarkRiftAPI.id - 1] : "RED")
            + "\n" + "currTurn: " + armyNames[CurrentPlayerNumber]
            + "\n" + barracksInfo
            + "\n" + "Current Capture: " + (capPoints >= 0 ? capPoints.ToString() : "")
            + "\nmessagecount: " + NetManager.messageIndex;
    }

    public void GenerateMoney()
    {

    }


}

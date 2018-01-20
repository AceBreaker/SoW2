using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Base class for all units in the game.
/// </summary>
[System.Serializable]
public abstract class Unit : MonoBehaviour
{
    public CellGrid grid = null;
    public string unitName = "";

    public bool hasMoved = false;

    public string description = "";

    /// <summary>
    /// UnitClicked event is invoked when user clicks the unit. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitClicked;
    /// <summary>
    /// UnitSelected event is invoked when user clicks on unit that belongs to him. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitSelected;
    public event EventHandler UnitDeselected;
    /// <summary>
    /// UnitHighlighted event is invoked when user moves cursor over the unit. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitHighlighted;
    public event EventHandler UnitDehighlighted;
    public event EventHandler<AttackEventArgs> UnitAttacked;
    public event EventHandler<AttackEventArgs> UnitDestroyed;
    public event EventHandler<MovementEventArgs> UnitMoved;

    public CustomUnitGenerator unitGenerator;

    public bool canCapture = false;
    public bool isAbleToCapture = true;

    public UnitState UnitState { get; set; }
    public void SetState(UnitState state)
    {
        UnitState.MakeTransition(state);
    }

    public List<Buff> Buffs { get; private set; }

    public int TotalHitPoints { get; private set; }
    protected int TotalMovementPoints;
    public int TotalActionPoints;

    public bool inStasis = false;

    /// <summary>
    /// Cell that the unit is currently occupying.
    /// </summary>
    public Cell Cell { get; set; }

    public int hitPoints;
    public int HitPoints
    {
        get
        {
            return hitPoints;
        }
        set
        {
            hitPoints = value;
            if (hitPoints > 100)
                hitPoints = 100;
        }
    }
    public int AttackRange;
    public int AttackFactor;
    public int DefenceFactor;
    public int MovementType = 0;//foot, mech, tires, tread, air
    /// <summary>
    /// Determines how far on the grid the unit can move.
    /// </summary>
    public int MovementPoints;
    /// <summary>
    /// Determines speed of movement animation.
    /// </summary>
    public float MovementSpeed;
    /// <summary>
    /// Determines how many attacks unit can perform in one turn.
    /// </summary>
    public int ActionPoints;

    /// <summary>
    /// Indicates the player that the unit belongs to. Should correspoond with PlayerNumber variable on Player script.
    /// </summary>
    public int PlayerNumber;

    public int price = 0;

    public bool hasAttacked = false;

    /// <summary>
    /// Indicates if movement animation is playing.
    /// </summary>
    public bool isMoving { get; set; }

    public int unitIndex = -1;

    private static IPathfinding _pathfinder = new AStarPathfinding();

    protected Dictionary<string, Dictionary<string, int>> damageValues = new Dictionary<string, Dictionary<string, int>>();

    public Color LeadingColor;

    public UnitType type = UnitType.ERROR;

    public float simpleAmusement = 0.0f;
    public static bool debugoverride = false;

    public Color myColor;

    public Vector3 prevPosition;
    public Cell prevCell;
    public bool selected = false;

    public enum Faction
    {
        BASIC = 0,
        CHEAP,
        POWER,
        COUNT
    }
    public Faction faction = Faction.BASIC;

    public Target typeOfTargets = Target.ENEMY;

    public StatsType myDamageType = StatsType.ERROR;
    public StatsType myArmorType = StatsType.ERROR;
    public float damageMultiplier = 1;
    public float armorMultiplier = 1;

    protected virtual void Update()
    {
        if(faction == Faction.BASIC)
            transform.Rotate(new Vector3(0.0f, 0.0f, simpleAmusement));
        else if (faction == Faction.CHEAP)
            transform.Rotate(new Vector3(simpleAmusement, simpleAmusement, simpleAmusement));

        if (selected && Input.GetKeyDown(KeyCode.Escape) && prevPosition != transform.position && ActionPoints != 0)
        {
            //var path = FindPath(grid.Cells, prevCell);
            //ResetMovementPoints();
            //Move(prevCell, path);
            //SendMoveInformation(prevCell);
            //grid.CellGridState = new CellGridStateUnitSelected(grid, this);
            //ResetMovementPoints();
            //hasMoved = false;
            //SetState(new UnitStateMarkedAsSelected(this));


            //if (UnitSelected != null)
            //    UnitSelected.Invoke(this, new EventArgs());

            //AdjustCellMovementCosts();
            //selected = true;
        }

        //foreach (Cell c in grid.Cells)
        //{
        //    if (Cell.GetDistance(c) <= 2)
        //    {
        //        c.GetComponent<Renderer>().material.color =  Color.white;
        //    }
        //}
    }

    public List<Cell> GetCellsInVision()
    {
        List<Cell> cellsInVision = new List<Cell>();
        foreach(Cell c in grid.Cells)
        {
            if (Cell.GetDistance(c) <= 2)
            {
                cellsInVision.Add(c);
            }
        }
        return cellsInVision;
    }

    public void ResetMovementPoints()
    {
        MovementPoints = TotalMovementPoints;
    }

    /// <summary>
    /// Method called after object instantiation to initialize fields etc. 
    /// </summary>
    public virtual void Initialize()
    {
        grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();

        GameObject.Find("SpawnUnitSFX").GetComponent<AudioSource>().Play();

        Buffs = new List<Buff>();

        UnitState = new UnitStateNormal(this);

        TotalHitPoints = HitPoints;
        TotalMovementPoints = MovementPoints;
        TotalActionPoints = ActionPoints;

        //Dictionary<string, int> targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 55);
        //targets.Add("MechScript", 45);
        //targets.Add("Recon", 12);
        //targets.Add("Tank", 5);
        //targets.Add("MdTank", 1);
        //targets.Add("Artillery", 15);
        //targets.Add("Rocket", 25);
        //targets.Add("APC", 14);
        //targets.Add("AntiAir", 5);
        //targets.Add("Missile", 25);
        //targets.Add("Copter", 7);
        //damageValues.Add("Infantry", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 65);
        //targets.Add("MechScript", 55);
        //targets.Add("Recon", 85);
        //targets.Add("Tank", 55);
        //targets.Add("MdTank", 15);
        //targets.Add("Artillery", 70);
        //targets.Add("Rocket", 85);
        //targets.Add("APC", 75);
        //targets.Add("AntiAir",65);
        //targets.Add("Missile", 85);
        //targets.Add("Copter", 9);
        //damageValues.Add("MechScript", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 90);
        //targets.Add("MechScript", 85);
        //targets.Add("Recon", 80);
        //targets.Add("Tank", 70);
        //targets.Add("MdTank", 45);
        //targets.Add("Artillery", 75);
        //targets.Add("Rocket", 80);
        //targets.Add("APC", 70);
        //targets.Add("AntiAir", 75);
        //targets.Add("Missile", 80);
        //damageValues.Add("Artillery", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry",     75);
        //targets.Add("MechScript",   70);
        //targets.Add("Recon",        85);
        //targets.Add("Tank",         55);
        //targets.Add("MdTank",       15);
        //targets.Add("Artillery",    70);
        //targets.Add("Rocket",       85);
        //targets.Add("APC",          75);
        //targets.Add("AntiAir",      65);
        //targets.Add("Missile",      85);
        //targets.Add("Copter", 10);
        //damageValues.Add("Tank", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 105);
        //targets.Add("MechScript", 95);
        //targets.Add("Recon", 105);
        //targets.Add("Tank", 85);
        //targets.Add("MdTank", 55);
        //targets.Add("Artillery", 105);
        //targets.Add("Rocket", 105);
        //targets.Add("APC", 105);
        //targets.Add("AntiAir", 105);
        //targets.Add("Missile", 105);
        //targets.Add("Copter", 12);
        //damageValues.Add("MdTank", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 70);
        //targets.Add("MechScript", 65);
        //targets.Add("Recon", 35);
        //targets.Add("Tank", 6);
        //targets.Add("MdTank", 1);
        //targets.Add("Artillery", 45);
        //targets.Add("Rocket", 55);
        //targets.Add("APC", 45);
        //targets.Add("AntiAir", 4);
        //targets.Add("Missile", 28);
        //targets.Add("Copter", 12);
        //damageValues.Add("Recon", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 95);
        //targets.Add("MechScript", 90);
        //targets.Add("Recon", 90);
        //targets.Add("Tank", 80);
        //targets.Add("MdTank", 55);
        //targets.Add("Artillery", 80);
        //targets.Add("Rocket", 85);
        //targets.Add("APC", 80);
        //targets.Add("AntiAir", 85);
        //targets.Add("Missile", 90);
        //damageValues.Add("Rocket", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 105);
        //targets.Add("MechScript", 105);
        //targets.Add("Recon", 60);
        //targets.Add("Tank", 25);
        //targets.Add("MdTank", 10);
        //targets.Add("Artillery", 50);
        //targets.Add("Rocket", 55);
        //targets.Add("APC", 50);
        //targets.Add("AntiAir", 45);
        //targets.Add("Missile", 55);
        //targets.Add("Fighter", 65);
        //targets.Add("Bomber", 75);
        //targets.Add("Copter", 105);
        //damageValues.Add("AntiAir", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 0);
        //targets.Add("MechScript", 0);
        //targets.Add("Recon", 0);
        //targets.Add("Tank", 0);
        //targets.Add("MdTank", 0);
        //targets.Add("Artillery", 0);
        //targets.Add("Rocket", 0);
        //targets.Add("APC", 0);
        //targets.Add("AntiAir", 0);
        //targets.Add("Missile", 0);
        //targets.Add("Fighter", 100);
        //targets.Add("Bomber", 100);
        //targets.Add("Copter", 115);
        //damageValues.Add("Missile", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 0);
        //targets.Add("MechScript", 0);
        //targets.Add("Recon", 0);
        //targets.Add("Tank", 0);
        //targets.Add("MdTank", 0);
        //targets.Add("Artillery", 0);
        //targets.Add("Rocket", 0);
        //targets.Add("APC", 0);
        //targets.Add("AntiAir", 0);
        //targets.Add("Missile", 0);
        //targets.Add("Fighter", 55);
        //targets.Add("Bomber", 75);
        //targets.Add("Copter", 105);
        //damageValues.Add("Fighter", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 110);
        //targets.Add("MechScript", 110);
        //targets.Add("Recon", 105);
        //targets.Add("Tank", 105);
        //targets.Add("MdTank", 95);
        //targets.Add("Artillery", 105);
        //targets.Add("Rocket", 105);
        //targets.Add("APC", 105);
        //targets.Add("AntiAir", 95);
        //targets.Add("Missile", 105);
        //targets.Add("Fighter", 0);
        //targets.Add("Bomber", 0);
        //targets.Add("Copter", 0);
        //damageValues.Add("Bomber", targets);

        //targets = new Dictionary<string, int>();
        //targets.Add("Infantry", 75);
        //targets.Add("MechScript", 75);
        //targets.Add("Recon", 55);
        //targets.Add("Tank", 55);
        //targets.Add("MdTank", 25);
        //targets.Add("Artillery", 65);
        //targets.Add("Rocket", 65);
        //targets.Add("APC", 60);
        //targets.Add("AntiAir", 25);
        //targets.Add("Missile", 65);
        //targets.Add("Fighter", 0);
        //targets.Add("Bomber", 0);
        //targets.Add("Copter", 0);
        //damageValues.Add("Copter", targets);


        if (this is Building)
        {
            HitPoints = 20;
            TotalHitPoints = 20;
        }
        else
        {
            HitPoints = 100;
            TotalHitPoints = 100;
        }

        UpdateColor();

        

        GetComponent<Renderer>().material.color = LeadingColor;
        myColor = GetComponent<Renderer>().material.color;

        unitIndex = UnitsIndexer.index++;

        if(this.transform.position.z == 0.0f)
        {
            this.transform.position -= new Vector3(0.0f, 0.0f, 1.0f);
        }

        //transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);
    }

    public SampleSquare2 GetMyTile()
    {
        foreach (SampleSquare2 b in grid.Cells)
        {
            if (b.transform.position.x == this.transform.position.x && b.transform.position.y == this.transform.position.y)
            {
                return b;
            }
        }
        return null;
    }

    public virtual void ResetToNeutralState()
    {
        hasAttacked = false;
        isAbleToCapture = true;
        GetComponent<Renderer>().material.color = LeadingColor;
        StopCoroutine("MyTurnColor");
    }

    public bool IsOwnedByMe()
    {
        bool result = false;

        if (DarkRift.DarkRiftAPI.isConnected && DarkRift.DarkRiftAPI.id - 1 == PlayerNumber)
            result = true;

        return result;
    }

    protected virtual void OnMouseDown()
    {
        int asdf = GameObject.Find("CellGrid").GetComponent<CellGrid>().CurrentPlayerNumber;
        if (!(this is Building) && !IsOwnedByMe() && DarkRift.DarkRiftAPI.isConnected && DarkRift.DarkRiftAPI.id - 1 != PlayerNumber
            &&  DarkRift.DarkRiftAPI.id - 1 != GameObject.Find("CellGrid").GetComponent<CellGrid>().CurrentPlayerNumber)
        {
            simpleAmusement += 0.1f;
        }
            if (UnitClicked != null /*&& NetManager.playerNumber-1 == PlayerNumber*/)
                UnitClicked.Invoke(this, new EventArgs());
    }

    protected virtual void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            simpleAmusement = 0.0f;
            transform.rotation = Quaternion.identity;
            transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
        }
    }

    protected virtual void OnMouseEnter()
    {
        if (UnitHighlighted != null)
            UnitHighlighted.Invoke(this, new EventArgs());
        Text unitInfo = GameObject.Find("UnitInfo").GetComponent<Text>();
        unitInfo.text = ReturnUnitInfo();
    }
    protected virtual void OnMouseExit()
    {
        if (UnitDehighlighted != null)
            UnitDehighlighted.Invoke(this, new EventArgs());
        Text unitInfo = GameObject.Find("UnitInfo").GetComponent<Text>();
        unitInfo.text = "No unit hovered";
    }

    public virtual void UpdateColor()
    {
        if (PlayerNumber == -1)
            LeadingColor = Color.white;
        else if (PlayerNumber == 0)
            LeadingColor = Color.red;
        else if (PlayerNumber == 1)
            LeadingColor = Color.blue;
        else if (PlayerNumber == 2)
            LeadingColor = Color.yellow;
        else if (PlayerNumber == 3)
            LeadingColor = Color.green;
        else if (PlayerNumber == 4)
            LeadingColor = new Color(0.8f, 0.3f, 0.0f);//orange
        else if (PlayerNumber == 5)
            LeadingColor = new Color(0.5f, 0.0f, 1.0f);//purple

        float r = (1.0f - LeadingColor.r) / 2.5f;
        float g = (1.0f - LeadingColor.g) / 2.5f;
        float b = (1.0f - LeadingColor.b) / 2.5f;
        float a = (1.0f - LeadingColor.a) / 2.5f;
        LeadingColor += new Color(r, g, b, 1.0f);
    }

    public int FindDisplayHP()
    {
        int hp = HitPoints / 10 + 1;
        if (hp > 10)
            hp = 10;
        return hp;
    }

    public virtual string ReturnUnitInfo()
    {
        string result = unitName +
            "\nHP: " + FindDisplayHP().ToString() +
            "\nCan Capture: " + canCapture.ToString() +
            "\nMove type: " + MovementType.ToString() +
            "\nRange: 1" +
            "\nMovement range: " + TotalMovementPoints.ToString() +
            "\n" + description;
        return result;
    }

    public IEnumerator MyTurnColor()
    {
        float i = 0;
        Color c = GetComponent<Renderer>().material.color;// myColor;
        bool flip = false;
        while (true)
        {
            if (i < 0.5f)
                i += Time.deltaTime;
            else
            {
                i = 0.0f;
                flip = !flip;
            }
            
            if(!flip)
                GetComponent<Renderer>().material.color = Color.Lerp(new Color(c.r - 0.2f, c.g - 0.2f, c.b - 0.2f), new Color(c.r + 0.2f, c.g + 0.2f, c.b + 0.2f),2.0f*i);
            else
                GetComponent<Renderer>().material.color = Color.Lerp(new Color(c.r + 0.2f, c.g + 0.2f, c.b + 0.2f), new Color(c.r - 0.2f, c.g - 0.2f, c.b - 0.2f),2.0f*i);
            //*= Math.Sin(i);
            yield return null;

        }
    }

    /// <summary>
    /// Method is called at the start of each turn.
    /// </summary>
    public virtual void OnTurnStart()
    {
        MovementPoints = TotalMovementPoints;
        if (!inStasis)
        {
            ActionPoints = TotalActionPoints;
            hasMoved = false;
            prevPosition = this.transform.position;
            prevCell = Cell;

            StartCoroutine("MyTurnColor");
        }
        else
        {
            ActionPoints = 0;
            hasMoved = true;
            GetComponent<Renderer>().material.color = LeadingColor / 6.0f;
            inStasis = false;
        }

        SetState(new UnitStateMarkedAsFriendly(this));
    }
    /// <summary>
    /// Method is called at the end of each turn.
    /// </summary>
    public virtual void OnTurnEnd()
    {
        Buffs.FindAll(b => b.Duration == 0).ForEach(b => { b.Undo(this); });
        Buffs.RemoveAll(b => b.Duration == 0);
        Buffs.ForEach(b => { b.Duration--; });
        
        //handlecapture in this method
        if(canCapture && isAbleToCapture)
        {

        }

        ResetToNeutralState();
        prevPosition = this.transform.position;
        prevCell = Cell;
        SetState(new UnitStateNormal(this));

    }
    /// <summary>
    /// Method is called when units HP drops below 1.
    /// </summary>
    protected virtual void OnDestroyed()
    {
        Cell.IsTaken = false;
        for (int i = 0; i < grid.Units.Count; i++)
        {
            Building b = grid.Units[i] as Building;
            if (b != null && b.transform.position.x == Cell.transform.position.x && b.transform.position.y == Cell.transform.position.y)
            {
                b.HitPoints = 20;
                break;
            }
        }
        GameObject.Find("ParticleHandler").GetComponent<ParticleHandler>().SpawnExplosion(this.transform.position);
        
        MarkAsDestroyed();
        Destroy(gameObject);
    }

    public virtual void HandleCapture()
    {

    }

    /// <summary>
    /// Method is called when unit is selected.
    /// </summary>
    public virtual void OnUnitSelected()
    {
        
        if (IsOwnedByMe() || debugoverride)
        {
            StopCoroutine("MyTurnColor");
            SetState(new UnitStateMarkedAsSelected(this));
            if (UnitSelected != null)
                UnitSelected.Invoke(this, new EventArgs());

            AdjustCellMovementCosts();
            selected = true;
        }
    }

    public void AdjustCellMovementCosts()
    {
        var sqr = GameObject.Find("CellGrid").GetComponentsInChildren<SampleSquare2>();
        if (MovementType == 0)//Foot
        {
            for (int i = 0; i < sqr.Count(); i++)
            {
                sqr[i].MovementCost = sqr[i].FootCost;
            }
        }
        if (MovementType == 1)//Mech
        {
            for (int i = 0; i < sqr.Count(); i++)
            {
                sqr[i].MovementCost = sqr[i].MechCost;
            }
        }
        if (MovementType == 2)//Tires
        {
            for (int i = 0; i < sqr.Count(); i++)
            {
                sqr[i].MovementCost = sqr[i].TiresCost;
            }
        }
        if (MovementType == 3)//Tread
        {
            for (int i = 0; i < sqr.Count(); i++)
            {
                sqr[i].MovementCost = sqr[i].TreadCost;
            }
        }
        if (MovementType == 4)//air
        {
            for (int i = 0; i < sqr.Count(); i++)
            {
                sqr[i].MovementCost = sqr[i].AirCost;
            }
        }
    }

    /// <summary>
    /// Method is called when unit is deselected.
    /// </summary>
    public virtual void OnUnitDeselected()
    {
        SetState(new UnitStateMarkedAsFriendly(this));
        if (UnitDeselected != null)
            UnitDeselected.Invoke(this, new EventArgs());

        if(hasMoved)
        {
            MovementPoints = 0;
        }

        if (ActionPoints > 0)
            GetComponent<Renderer>().material.color = LeadingColor;
        else if (ActionPoints == 0)
            SetDeselectedColor();

        selected = false;
    }

    public virtual void SetDeselectedColor()
    {
        StopCoroutine("MyTurnColor");
        GetComponent<Renderer>().material.color = LeadingColor / 6.0f;
    }

    /// <summary>
    /// Method indicates if it is possible to attack unit given as parameter, from cell given as second parameter.
    /// </summary>
    public virtual bool IsUnitAttackable(Unit other, Cell sourceCell)
    {
        if (sourceCell.GetDistance(other.Cell) <= AttackRange)
            return true;

        return false;
    }

    public StatsType GetDamageType()
    {
        return myDamageType;
    }

    public StatsType GetArmorType()
    {
        return myArmorType;
    }

    /// <summary>
    /// Method deals damage to unit given as parameter.
    /// </summary>
    public virtual void DealDamage(Unit other, bool fromNetwork = false)
    {
        if (isMoving)
            return;
        if (ActionPoints == 0)
            return;
        if (!IsUnitAttackable(other, Cell) && !fromNetwork)
            return;

        MarkAsAttacking(other);
        ActionPoints--;
        GetComponent<Renderer>().material.color = LeadingColor / 6.0f;
        string a = this.GetType().ToString();
            string b = other.GetType().ToString();
        //AttackFactor = DamageChart.damageValues[this.GetType().ToString()][other.GetType().ToString()] ;
        AttackFactor = (int)(DamageChart.damageValues[this.GetDamageType().ToString().ToUpper()][other.GetArmorType().ToString().ToUpper()] * damageMultiplier * other.armorMultiplier);
        AttackFactor *= (typeOfTargets == Target.FRIENDLY ? -1 : 1);
        other.Defend(this, AttackFactor);
        UpdateHpBar();
        if (ActionPoints == 0)
        {
            SetState(new UnitStateMarkedAsFinished(this));
            MovementPoints = 0;
        }

        hasAttacked = true;
        isAbleToCapture = false;

        if (HitPoints <= 0)
        {
            CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
            Player myPlayer = grid.Players.Find(p => p.PlayerNumber.Equals(other.PlayerNumber));
            if (grid.Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber) && !(u is Building)).Count <= 1)
            {
                myPlayer.EliminatePlayer(false, this);
                grid.CheckForEndGame();
            }
            
            DestroyUnit(other, 100);
            return;
        }
    }
    /// <summary>
    /// Attacking unit calls Defend method on defending unit. 
    /// </summary>
    protected virtual void Defend(Unit other, int damage)
    {
        MarkAsDefending(other);
        HitPoints -= DamageCalculator(other, damage, 0, Cell.defenseValue);
        GameObject.Find("DamageSound").GetComponent<AudioSource>().Play();

        StartCoroutine("DamageFlash");

        //(55 * ((100 / (100 + 0 * 10))) / (10 - Math.Ceiling(10)))
        //HitPoints -= Mathf.Clamp(damage - DefenceFactor, 1, damage);  //Damage is calculated by subtracting attack factor of attacker and defence factor of defender. If result is below 1, it is set to 1.
                                                                      //This behaviour can be overridden in derived classes.
        if (UnitAttacked != null)
            UnitAttacked.Invoke(this, new AttackEventArgs(other, this, damage));

        if (HitPoints <= 0)
        {
            CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
            Player myPlayer = grid.Players.Find(p => p.PlayerNumber.Equals(this.PlayerNumber));
            if(grid.Units.FindAll(u => u.PlayerNumber.Equals(this.PlayerNumber) && !(u is Building)).Count <= 1)
            {
                myPlayer.EliminatePlayer(false, other);
                grid.CheckForEndGame();
            }

            DestroyUnit(other, damage);

            return;
        }
        if(CanCounterAttack(other))
        {
            AttackFactor = (int)(DamageChart.damageValues[this.GetDamageType().ToString().ToUpper()][other.GetArmorType().ToString().ToUpper()] * damageMultiplier * other.armorMultiplier);
            //AttackFactor = DamageChart.damageValues[this.GetType().ToString()][other.GetType().ToString()];
            other.HitPoints -= other.DamageCalculator(this, AttackFactor, 0, 0);
            
                other.StartCoroutine("DamageFlash");
        }

        UpdateHpBar();
    }

    public void DestroyUnit(Unit other, int damage)
    {
        if (UnitDestroyed != null)
            UnitDestroyed.Invoke(this, new AttackEventArgs(other, this, damage));
        OnDestroyed();
    }

    protected virtual bool CanCounterAttack(Unit other)
    {
        return (other.AttackRange == 1 && this.AttackRange == 1 && other.typeOfTargets == Target.ENEMY);
    }

    protected virtual void UpdateHPDisplay()
    {
        GetComponentInChildren<HealthDisplay>().UpdateHPDisplay();
    }

    protected virtual int DamageCalculator(Unit attacker, int damage, int damageBuff, int defenseBonus)
    {
        //'this' is defending
        if (this.MovementType == 4)//air
            defenseBonus = 0;
        int a1 = damage;
        int a2 = ((100) / (100 + (defenseBonus * 10)));
        double a3 = (Math.Ceiling((float)attacker.HitPoints / 10.0f) / 10.0f);
        double a4 = Math.Ceiling((float)attacker.HitPoints / 10.0f);
        int dmg =(int)(damage * ((100.0f) / (100 + (defenseBonus * 10))) * (Math.Ceiling((float)attacker.HitPoints / 10.0f) / 10.0f));
        return dmg;
    }

    public void UpdateHpBar()
    {
        if (GetComponentInChildren<Image>() != null)
        {
            GetComponentInChildren<Image>().transform.localScale = new Vector3((float)((float)HitPoints / (float)TotalHitPoints), 1, 1);
            GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green,
                (float)((float)HitPoints / (float)TotalHitPoints));

            int displayHitPoints = HitPoints / 10 + 1;
            if (displayHitPoints > 10)
                displayHitPoints = 10;
            GetComponentInChildren<TextMesh>().text = displayHitPoints == 10 ? "" : displayHitPoints.ToString();
        }
    }

    public virtual void Move(Cell destinationCell, List<Cell> path, bool fromNetwork = false)
    {
        if (isMoving)
            return;
       // if(!fromNetwork)
          //  SendMoveInformation(path[0]);

        var totalMovementCost = path.Sum(h => h.MovementCost);
        if (MovementPoints < totalMovementCost)
            return;

        MovementPoints -= totalMovementCost;
        hasMoved = true;
        
        for(int i = 0; i < grid.Units.Count; i++)
        {
            Building b = grid.Units[i] as Building;
            if(b != null && b.transform.position.x == Cell.transform.position.x && b.transform.position.y == Cell.transform.position.y)
            {
                b.HitPoints = 20;
                break;
            }
        }
        Cell.IsTaken = false;
        Cell = destinationCell;
        destinationCell.IsTaken = true;

        if (MovementSpeed > 0)
            StartCoroutine(MovementAnimation(path));
        else
            transform.position = Cell.transform.position;

        if (UnitMoved != null)
            UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path));    
    }
    protected virtual IEnumerator MovementAnimation(List<Cell> path)
    {
        isMoving = true;
        
        

        path.Reverse();
        foreach (var cell in path)
        {
            while (new Vector2(transform.position.x,transform.position.y) != new Vector2(cell.transform.position.x,cell.transform.position.y))
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(cell.transform.position.x,cell.transform.position.y,transform.position.z), Time.deltaTime * MovementSpeed);
                yield return 0;
            }
        }
        if(prevPosition == transform.position)
        {
            SetState(new UnitStateMarkedAsSelected(this));
            if (UnitSelected != null)
                UnitSelected.Invoke(this, new EventArgs());

            AdjustCellMovementCosts();
            selected = true;
        }
        else
            isMoving = false;
        //TagIndex.Controller, TagIndex.PlayerUpdate, startgame
        
    }

    public virtual void SendMoveInformation(Cell newCell)
    {
        UnitUpdate update = new UnitUpdate();
        update.command = UnitUpdateCommand.MOVE;
        update.moving = this.unitIndex;
        update.newLocationX = newCell.transform.position.x;
        update.newLocationY = newCell.transform.position.y;
        NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, update);
    }

    ///<summary>
    /// Method indicates if unit is capable of moving to cell given as parameter.
    /// </summary>
    public virtual bool IsCellMovableTo(Cell cell)
    {
        return !cell.IsTaken;
    }
    /// <summary>
    /// Method indicates if unit is capable of moving through cell given as parameter.
    /// </summary>
    public virtual bool IsCellTraversable(Cell cell)
    {
        //return !cell.IsTaken;
        Unit blockingUnit = grid.Units.Find(u => (!(u is Building) && u.transform.position.x == cell.transform.position.x && u.transform.position.y == cell.transform.position.y));
        return (!cell.IsTaken || (blockingUnit != null && cell.IsTaken && blockingUnit.PlayerNumber == this.PlayerNumber ));
    }
    /// <summary>
    /// Method returns all cells that the unit is capable of moving to.
    /// </summary>
    public List<Cell> GetAvailableDestinations(List<Cell> cells)
    {
        var ret = new List<Cell>();
        var cellsInMovementRange = cells.FindAll(c => IsCellMovableTo(c) && c.GetDistance(Cell) <= MovementPoints);

        var traversableCells = cells.FindAll(c => IsCellTraversable(c) && c.GetDistance(Cell) <= MovementPoints);
        traversableCells.Add(Cell);

        foreach (var cellInRange in cellsInMovementRange)
        {
            if (cellInRange.Equals(Cell)) continue;

            var path = FindPath(traversableCells, cellInRange);
            var pathCost = path.Sum(c => c.MovementCost);
            if (pathCost > 0 && pathCost <= MovementPoints)
                ret.AddRange(path);
        }
        return ret.FindAll(IsCellMovableTo).Distinct().ToList();
    }

    public List<Cell> FindPath(List<Cell> cells, Cell destination)
    {
        return _pathfinder.FindPath(GetGraphEdges(cells), Cell, destination);
    }

    public IEnumerator DamageFlash()
    {
        bool sdf = false;
        for(int i = 0; i < 110; i++)
        {
            if (i % 10 == 0)
                sdf = !sdf;
        GetComponent<Renderer>().enabled = sdf;
        yield return null;

        }
    }

    /// <summary>
    /// Method returns graph representation of cell grid for pathfinding.
    /// </summary>
    protected virtual Dictionary<Cell, Dictionary<Cell, int>> GetGraphEdges(List<Cell> cells)
    {
        Dictionary<Cell, Dictionary<Cell, int>> ret = new Dictionary<Cell, Dictionary<Cell, int>>();
        foreach (var cell in cells)
        {
            if (IsCellTraversable(cell) || cell.Equals(Cell))
            {
                ret[cell] = new Dictionary<Cell, int>();
                foreach (var neighbour in cell.GetNeighbours(cells).FindAll(IsCellTraversable))
                {
                    ret[cell][neighbour] = neighbour.MovementCost;
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// Gives visual indication that the unit is under attack.
    /// </summary>
    /// <param name="other"></param>
    public abstract void MarkAsDefending(Unit other);
    /// <summary>
    /// Gives visual indication that the unit is attacking.
    /// </summary>
    /// <param name="other"></param>
    public abstract void MarkAsAttacking(Unit other);
    /// <summary>
    /// Gives visual indication that the unit is destroyed. It gets called right before the unit game object is
    /// destroyed, so either instantiate some new object to indicate destruction or redesign Defend method. 
    /// </summary>
    public abstract void MarkAsDestroyed();

    /// <summary>
    /// Method marks unit as current players unit.
    /// </summary>
    public abstract void MarkAsFriendly();
    /// <summary>
    /// Method mark units to indicate user that the unit is in range and can be attacked.
    /// </summary>
    public abstract void MarkAsReachableEnemy();
    /// <summary>
    /// Method marks unit as currently selected, to distinguish it from other units.
    /// </summary>
    public abstract void MarkAsSelected();
    /// <summary>
    /// Method marks unit to indicate user that he can't do anything more with it this turn.
    /// </summary>
    public abstract void MarkAsFinished();
    /// <summary>
    /// Method returns the unit to its base appearance
    /// </summary>
    public abstract void UnMark();
}

public class MovementEventArgs : EventArgs
{
    public Cell OriginCell;
    public Cell DestinationCell;
    public List<Cell> Path;

    public MovementEventArgs(Cell sourceCell, Cell destinationCell, List<Cell> path)
    {
        OriginCell = sourceCell;
        DestinationCell = destinationCell;
        Path = path;
    }
}
public class AttackEventArgs : EventArgs
{
    public Unit Attacker;
    public Unit Defender;

    public int Damage;

    public AttackEventArgs(Unit attacker, Unit defender, int damage)
    {
        Attacker = attacker;
        Defender = defender;

        Damage = damage;
    }
}

[Serializable]
public enum UnitType
{
    ERROR = 0,
    INFANTRY,
    MECH,
    RECON,
    TANK,
    MDTANK,
    ARTILLERY,
    ROCKETS,
    MISSILES,
    APC,
    ANTIAIR,
    FIGHTER,
    BOMBER,
    COPTER,

    ANDROID,
    SNIPER,
    REPAIR,
    ROBOTANK,
    SPEEDTANK,
    MEGATANK,
    CANNON,
    AAGUN,
    TURRET,
    CAPCOPTER,
    AERIALACE,
    BOMB,

    ADVERSARY,
    PLASMATIC,
    RECLAIMER,
    EQUALIZER,
    PROTECTOR,
    AREAANNIHILATOR,
    DMCANNON,
    COLLECTOR,
    STASISGUN,
    ORBITALSTRIKER,
    IRRADIATOR,
    NULLSTAR,


    COUNT
};

public enum StatsType
{
    ERROR = 0,
    INFANTRY,
    MECH,
    RECON,
    TANK,
    MDTANK,
    ARTILLERY,
    ROCKETS,
    MISSILES,
    APC,
    ANTIAIR,
    FIGHTER,
    BOMBER,
    COPTER,
    ANTIINFANTRY,
    REPAIRDRONE,
    AERIALACE,
    SUICIDEAA,
    CONSTANTGROUND
};



public enum Target
{
    ENEMY,
    FRIENDLY,
    COUNT
};
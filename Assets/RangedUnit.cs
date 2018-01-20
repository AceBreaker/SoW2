using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class RangedUnit : Unit
{

    public int minRange = 2;


    bool canFire = true;

    List<Cell> cellsInRange = new List<Cell>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected virtual void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var asdf = GameObject.Find("CellGrid").GetComponent<CellGrid>().Cells;
            foreach(Cell c in asdf)
            {
                if(Cell.GetDistance(c) <= AttackRange && Cell.GetDistance(c) >= minRange)
                {
                    c.GetComponent<Renderer>().material.color = Color.red * 0.8f;
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            var asdf = GameObject.Find("CellGrid").GetComponent<CellGrid>().Cells;
            foreach (Cell c in asdf)
            {
                if (Cell.GetDistance(c) <= AttackRange && Cell.GetDistance(c) >= minRange && c is SampleSquare2)
                {
                    c.GetComponent<Renderer>().material.color = (c as SampleSquare2).starting;
                }
            }
        }

        base.OnMouseOver();
        Text unitInfo = GameObject.Find("UnitInfo").GetComponent<Text>();
        unitInfo.text = ReturnUnitInfo();
    }

    public override string ReturnUnitInfo()
    {
        string result = unitName +
            "\nHP: " + FindDisplayHP().ToString() +
            "\nCan Capture: " + canCapture.ToString() +
            "\nMove type: " + MovementType.ToString() +
            "\nRange: " + minRange.ToString() + "-" + AttackRange.ToString() +
            "\nMovement range: " + TotalMovementPoints.ToString() +
            "\n" + description;
        return result;
    }

    protected override void OnMouseExit()
    {
        var asdf = GameObject.Find("CellGrid").GetComponent<CellGrid>().Cells;
        foreach (Cell c in asdf)
        {
            if (Cell.GetDistance(c) <= AttackRange && Cell.GetDistance(c) >= minRange && c is SampleSquare2)
            {
                c.GetComponent<Renderer>().material.color = (c as SampleSquare2).starting;
            }
        }
        base.OnMouseExit();
    }

    public override void Initialize()
    {
        base.Initialize();
        //transform.position += new Vector3(0, 0, -1);
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

    public override void ResetToNeutralState()
    {
        canFire = true;
        base.ResetToNeutralState();
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

    public override void Move(Cell destinationCell, List<Cell> path, bool fromNetwork = false)
    {
        base.Move(destinationCell, path);
        canFire = false;

    }

    /// <summary>
    /// Method indicates if it is possible to attack unit given as parameter, from cell given as second parameter.
    /// </summary>
    public override bool IsUnitAttackable(Unit other, Cell sourceCell)
    {
        if (sourceCell.GetDistance(other.Cell) <= AttackRange && sourceCell.GetDistance(other.Cell) >= minRange)
            return true;

        return false;
    }

    public override void OnUnitSelected()
    {
        base.OnUnitSelected();
    }

    public override void DealDamage(Unit other, bool fromNetwork = false)
    {
        if(canFire)
            base.DealDamage(other, fromNetwork);
    }

}

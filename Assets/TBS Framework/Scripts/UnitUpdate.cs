using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using System.IO;

/// <summary>
/// This will be the object we send when we update a unit's status including new location, target, damage, random value?
/// </summary>

    [Serializable]
public class UnitUpdate
{ 
    public int moving = -1000;
    public int target = -1000;
    public float newLocationX = -1000.0f;
    public float newLocationY = -1000.0f;
    public UnitType type = UnitType.ERROR;
    public MatIndex buildingType = MatIndex.BuildSite;
    public UnitUpdateCommand command = UnitUpdateCommand.NONE;
    public int index = -1;

    public void Clear()
    {
        moving = -1000;
        target = -1000;
        newLocationX = -1000.0f;
        newLocationY = -1000.0f;
        type = UnitType.ERROR;
        command = UnitUpdateCommand.NONE;
        index = -1;
    }
}

[Serializable]
public enum UnitUpdateCommand
{
    NONE = 0,
    MOVE,
    ATTACK,
    END_TURN,
    SPAWN,
    CAPTURE,
    INCOME,
    BUILDSITE,
    COUNT
};


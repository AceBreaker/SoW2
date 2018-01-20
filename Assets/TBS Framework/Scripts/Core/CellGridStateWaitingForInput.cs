class CellGridStateWaitingForInput : CellGridState
{
    public CellGridStateWaitingForInput(CellGrid cellGrid) : base(cellGrid)
    {
    }

    public override void OnUnitClicked(Unit unit)
    {
        if(unit.PlayerNumber.Equals(_cellGrid.CurrentPlayerNumber) && DarkRift.DarkRiftAPI.isConnected && DarkRift.DarkRiftAPI.id - 1 == _cellGrid.CurrentPlayerNumber || Unit.debugoverride)
            _cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, unit); 
    }
}

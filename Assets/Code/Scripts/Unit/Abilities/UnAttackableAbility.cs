using TbsFramework.Grid;
using TbsFramework.Units.Abilities;

public class UnAttackableAbility : Ability
{
    private bool _isAttackable;
    private int _startPlayerNumber;

    #region Properties

    public bool IsAttackable
    {
        get => _isAttackable;
    }

    #endregion

    private void Start()
    {
        _startPlayerNumber = UnitReference.PlayerNumber;
    }

    public override void OnTurnStart(CellGrid cellGrid)
    {
        if (IsAttackable) return;
        base.OnTurnStart(cellGrid);
        if (cellGrid.CurrentPlayerNumber != 0) return;
        UnitReference.PlayerNumber = 0;
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        if (IsAttackable) return;
        base.OnTurnEnd(cellGrid);
        if (!IsAttackable && cellGrid.CurrentPlayerNumber == 0)
        {
            UnitReference.PlayerNumber = 1;
            return;
        }

        UnitReference.PlayerNumber = 0;
    }

    public void MakeAttackable()
    {
        _isAttackable = true;
        UnitReference.PlayerNumber = _startPlayerNumber;
    }
}
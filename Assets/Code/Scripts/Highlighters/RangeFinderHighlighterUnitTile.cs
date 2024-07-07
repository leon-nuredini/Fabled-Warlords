using TbsFramework.Units;
using TbsFramework.Units.Highlighters;
using UnityEngine;

public class RangeFinderHighlighterUnitTile : UnitHighlighter
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private RangeFinderAnimatorController _rangeFinderAnimatorController;
    [SerializeField] private Color _color;
    [SerializeField] private RangeFinderType _rangeFinderType;

    public override void Apply(Unit unit, Unit otherUnit)
    {
        if (_spriteRenderer != null) _spriteRenderer.color = _color;
        _rangeFinderAnimatorController.PlayAnimation(_rangeFinderType);
    }
}
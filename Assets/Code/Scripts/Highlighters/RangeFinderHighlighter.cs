using TbsFramework.Cells;
using TbsFramework.Cells.Highlighters;
using UnityEngine;

public class RangeFinderHighlighter : CellHighlighter
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private RangeFinderAnimatorController _rangeFinderAnimatorController;
    [SerializeField] private Color _color;
    [SerializeField] private RangeFinderType _rangeFinderType;
    
    public override void Apply(Cell cell)
    {
        if (_spriteRenderer != null) _spriteRenderer.color = _color;
        _rangeFinderAnimatorController.gameObject.SetActive(true);
        _rangeFinderAnimatorController.PlayAnimation(_rangeFinderType);
    }
}
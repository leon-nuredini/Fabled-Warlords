using TbsFramework.Cells;
using TbsFramework.Cells.Highlighters;
using UnityEngine;

public class PathHighlighter : CellHighlighter
{
    [SerializeField] private SpriteRenderer _pathSpriteRenderer;
    [SerializeField] private Color _fadedColor;

    public override void Apply(Cell cell)
    {
        _pathSpriteRenderer.gameObject.SetActive(false);
        _pathSpriteRenderer.sprite = null;
        _pathSpriteRenderer.color  = _fadedColor;
    }
}
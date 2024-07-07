using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Highlighters;
using UnityEngine;

public class UpdateCursorUnitHighlighter : UnitHighlighter
{
    [SerializeField] private bool _activateCursor;
    [SerializeField] private SpriteRenderer _cursorRenderer;

    [ShowIf("_activateCursor")] [SerializeField]
    private Color _cursorColor = Color.white;

    private void OnEnable() => CellGrid.Instance.TurnEnded += OnTurnEnd;
    private void OnDisable() => CellGrid.Instance.TurnEnded -= OnTurnEnd;

    public override void Apply(Unit unit, Unit otherUnit)
    {
        _cursorRenderer.gameObject.SetActive(_activateCursor);
        if (_activateCursor) _cursorRenderer.color = _cursorColor;
    }

    private void OnTurnEnd(object sender, bool isNetworkConnection) => _cursorRenderer.gameObject.SetActive(false);
}
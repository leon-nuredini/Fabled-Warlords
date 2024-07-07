using Lean.Pool;
using Singleton;
using TbsFramework.Cells;
using UnityEngine;

public class SelectionCursorController : SceneSingleton<SelectionCursorController>
{
    [SerializeField] private GameObject _selectionCursorGameObject;
    [SerializeField] private bool       _enableCursor;

    private GameObject _selectionCursor;

    private void SpawnCursor(Vector3 spawnPosition)
    {
        if (!_enableCursor) return;
        _selectionCursor = LeanPool.Spawn(_selectionCursorGameObject, spawnPosition, Quaternion.identity);
    }

    public void DespawnCursor()
    {
        if (_selectionCursor == null) return;
        LeanPool.Despawn(_selectionCursor);
        _selectionCursor = null;
    }

    public void ShowCursorAtCellPosition(Cell cell)
    {
        if (_selectionCursor != null)
            _selectionCursor.transform.localPosition = cell.transform.localPosition;
        else
            SpawnCursor(cell.transform.localPosition);
    }
}
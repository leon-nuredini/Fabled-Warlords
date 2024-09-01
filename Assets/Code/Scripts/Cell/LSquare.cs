using System.Collections.Generic;
using NaughtyAttributes;
using TbsFramework.Cells;
using TbsFramework.Cells.Highlighters;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TbsFramework.Grid;
using TbsFramework.Players;

public class LSquare : Square
{
    public static event Action<Cell, Player> OnAnyRecruitUnit;
    public static event Action<LSquare> OnAnyClickCell;
    public static event Action<LSquare> OnAnyRightClickCell;

    [BoxGroup("Highlight")] public List<CellHighlighter> MarkAsRecruitZoneFn;
    [BoxGroup("Highlight")] public List<CellHighlighter> MarkAsHighlightedRecruitZoneFn;

    [BoxGroup("Terrain Description")] [SerializeField]
    private TerrainDescription _terrainDescription;

    public bool IsRecruitmentZone;
    public bool IsRecruitmentZoneHighlighted;

    public                  string TileType;
    [Range(-50, 50)] public int    EvasionFactor;
    [Range(-30, 30)] public int    HitChance;

    private PaintPath _paintPath;

    Vector3 dimensions = new Vector3(1.6f, 1.6f, 0f);

    private bool _isHoveringOnTile;

    #region Properties

    public PaintPath PaintPath => _paintPath;
    public TerrainDescription TerrainDescription => _terrainDescription;

    #endregion

    private void Awake() => _paintPath = GetComponentInChildren<PaintPath>(true);

    private void OnEnable()
    {
        UIRecruitment.OnAnyOpenRecruitmentPanel += UnMarkCurrentSelectedUnit;
        UIRecruitment.OnAnyClickRecruitButton += UnMarkCurrentSelectedUnit;
    }

    private void OnDisable()
    {
        UIRecruitment.OnAnyOpenRecruitmentPanel -= UnMarkCurrentSelectedUnit;
        UIRecruitment.OnAnyClickRecruitButton -= UnMarkCurrentSelectedUnit;
    }

    public override Vector3 GetCellDimensions() => dimensions;

    protected override void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (IsRecruitmentZone)
        {
            OnAnyRecruitUnit?.Invoke(this, CellGrid.Instance.CurrentPlayer);
            UnMark();
        }

        UnMarkCurrentSelectedUnit();
        base.OnMouseDown();
        OnAnyClickCell?.Invoke(this);
    }

    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(1))
            OnAnyRightClickCell?.Invoke(this);
    }

    private void UnMarkCurrentSelectedUnit()
    {
        if (ObjectHolder.Instance != null && ObjectHolder.Instance.CurrSelectedUnit != null && !IsMarkedReachable)
        {
            ObjectHolder.Instance.CurrSelectedUnit.UnmarkSelection();
            ObjectHolder.Instance.CurrSelectedUnit = null;
        }

        base.OnMouseDown();
    }

    protected override void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (IsRecruitmentZone)
        {
            HighlightAvailableRecruitCell();
            return;
        }

        if (SelectionCursorController.Instance != null)
            SelectionCursorController.Instance.ShowCursorAtCellPosition(this);

        if (ObjectHolder.Instance != null && PathPainter.Instance != null && !IsMarkedReachable)
            PathPainter.Instance.DeletePath();

        base.OnMouseEnter();
    }

    protected override void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (IsRecruitmentZone)
        {
            if (IsRecruitmentZoneHighlighted)
            {
                IsRecruitmentZoneHighlighted = false;
                MarkAsRecruitZone();
            }

            return;
        }

        if (IsMarkedReachable && !IsMarkedPath) return;
        base.OnMouseExit();
    }

    public override void SetColor(Color color)
    {
        var highlighter = transform.Find("marker");
        var spriteRenderer = highlighter.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = color;
    }

    public virtual void MarkAsRecruitZone()
    {
        if (IsTaken) return;
        MarkAsRecruitZoneFn?.ForEach(o => o.Apply(this));
        IsRecruitmentZone = true;
    }

    private void HighlightAvailableRecruitCell()
    {
        MarkAsHighlightedRecruitZoneFn?.ForEach(o => o.Apply(this));
        IsRecruitmentZoneHighlighted = true;
    }

    public override void UnMark()
    {
        IsRecruitmentZoneHighlighted = false;
        IsRecruitmentZone = false;
        base.UnMark();
    }
}
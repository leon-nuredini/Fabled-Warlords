using System;
using TbsFramework.Grid;
using UnityEngine;

public class LStructure : LUnit, ICapturable
{
    public static event Action OnAnyCapturedStructure;

    public event Action<LUnit> OnCaptured;
    public event Action        OnAbandoned;

    private bool _isCaptured;

    protected override void DefenceActionPerformed()
    {
        if (Agressor is LUnit aggressor)
            TryCaptureStructure(aggressor);
        SpawnDamageText();
    }

    private bool TryCaptureStructure(LUnit aggressor)
    {
        if (HitPoints > 0) return false;
        if (aggressor.CapturerSkill != null)
        {
            HitPoints = TotalHitPoints;
            Capture(aggressor);
            _isCaptured = true;
            return true;
        }
        else { HitPoints = 1; }

        return false;
    }

    public virtual void Capture(LUnit aggressor)
    {
        if (aggressor.CapturerSkill == null) return;
        OnCapturedActionPerformed(aggressor);
        if (CellGrid.Instance != null)
            CellGrid.Instance.UpdateCurrentPlayerUnits();

        if (ObjectHolder.Instance != null && ObjectHolder.Instance.CurrentEnemyMarkedUnit == this)
            ObjectHolder.Instance.CurrentEnemyMarkedUnit = null;
    }

    protected override void DisplayUnitInformation()
    {
        string factionName = "";

        if (Agressor is LUnit lUnit)
            factionName = lUnit.UnitDetails.Faction;

        UnitDetails unitDetails = ScriptableObject.CreateInstance<UnitDetails>();
        unitDetails.Initialize(UnitDetails.UnitName, factionName, MaskSpriteRenderer.sprite, UnitDetails.Description);

        UnitDetails = unitDetails;
        base.DisplayUnitInformation();
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();
        if (_isCaptured)
        {
            if (ObjectHolder.Instance.CurrSelectedUnit != null)
            {
                ObjectHolder.Instance.CurrSelectedUnit.UnmarkSelection();
                ObjectHolder.Instance.CurrSelectedUnit = null;
            }

            _isCaptured = false;
        }
    }

    protected virtual void OnCapturedActionPerformed(LUnit aggressor)
    {
        UnitFaction = aggressor.UnitFaction;
        OnCaptured?.Invoke(aggressor);
        OnAnyCapturedStructure?.Invoke();
        CellGrid.Instance.CheckGameFinished();
    }

    public void AbandonStructure()
    {
        UnitFaction = UnitFaction.None;
        HitPoints   = TotalHitPoints;
        _isCaptured = false;
        OnAbandoned?.Invoke();
    }
}
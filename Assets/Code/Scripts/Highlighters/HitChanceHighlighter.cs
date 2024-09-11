using TbsFramework.Units;
using TbsFramework.Units.Highlighters;
using UnityEngine;

public class HitChanceHighlighter : UnitHighlighter
{
    [SerializeField] private bool _showHitChance;
    
    private HitChanceText _hitChanceText;
    
    private void Awake() => _hitChanceText = GetComponentInParent<HitChanceText>();
    
    public override void Apply(Unit unit, Unit otherUnit)
    {
        if (_hitChanceText == null) return;
        if (unit is LUnit lUnit)
        {
            if (_showHitChance)
                _hitChanceText.UpdateHitChanceText(lUnit);
            else
                _hitChanceText.HideHitChanceText();
        }
    }
}

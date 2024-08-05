using TbsFramework.Units.Abilities;
using UnityEngine;

public class SwapBuildingTypeAbility : Ability
{
    [SerializeField] private Sprite _ruinsSprite;
    [SerializeField] private StructureStats _newStructureStats;
    [SerializeField] private UnitDetails _newStructureDetails;

    private bool _isBuildingTypeSwapped;

    private ICapturable _capturable;

    private void Awake()
    {
        _capturable = GetComponent<ICapturable>();
    }
    
    private void OnEnable()
    {
        _capturable.OnCaptured += CaptureStructure;
    }

    private void OnDisable()
    {
        _capturable.OnCaptured -= CaptureStructure;
    }

    private void CaptureStructure(LUnit capturer)
    {
        if (_isBuildingTypeSwapped) return;
        
        LStructure lStructure = _capturable as LStructure;
        if (lStructure == null) return;

        if (lStructure is Stronghold stronghold)
        {
            stronghold.IsRuined = true;
            stronghold.MaskSpriteRenderer.sprite = _ruinsSprite;
            stronghold.IncomeGenerationAbility.IncomeAmount = 1;
            stronghold.UnitStats = _newStructureStats;
            stronghold.UnitDetails = _newStructureDetails;
        }

        UnitReference.PlayerNumber = capturer.PlayerNumber;
        _isBuildingTypeSwapped = true;
    }
}

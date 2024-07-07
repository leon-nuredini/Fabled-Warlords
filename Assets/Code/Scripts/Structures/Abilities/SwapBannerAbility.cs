using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class SwapBannerAbility : Ability
{
    [BoxGroup("Sprites")] [SerializeField] private GameObject _redBanner;
    [BoxGroup("Sprites")] [SerializeField] private GameObject _greenBanner;
    [BoxGroup("Sprites")] [SerializeField] private GameObject _blueBanner;

    private ICapturable _capturable;
    private LUnit _capturer;

    private void Awake() => _capturable = GetComponent<ICapturable>();
    private void OnEnable() => _capturable.OnCaptured += CaptureStructure;
    private void OnDisable() => _capturable.OnCaptured -= CaptureStructure;

    private void CaptureStructure(LUnit capturer)
    {
        _capturer = capturer;
        LStructure _lStructure = _capturable as LStructure;
        if (_lStructure == null) return;
        
        UnitReference.PlayerNumber = _capturer.PlayerNumber;
        _redBanner.SetActive(_capturer.Faction == UnitFaction.Red);
        _greenBanner.SetActive(_capturer.Faction == UnitFaction.Green);
        _blueBanner.SetActive(_capturer.Faction == UnitFaction.Blue);

        CellGrid.Instance.CheckGameFinished();
    }
}
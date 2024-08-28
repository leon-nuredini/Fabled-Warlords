using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class BaseUnitStatsPresenter : MonoBehaviour, IUnitPresenter
{
    [BoxGroup("Position")] [SerializeField]
    private RectTransform _statsPosition;

    [BoxGroup("Position")] [SerializeField]
    private Vector2 _unitStatPosition;

    [BoxGroup("Position")] [SerializeField]
    private Vector2 _structureStatPosition;

    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _atkText;
    [SerializeField] private TextMeshProUGUI _rangeText;
    [SerializeField] private TextMeshProUGUI _evasionText;
    [SerializeField] private TextMeshProUGUI _apText;
    [SerializeField] private TextMeshProUGUI _incomeText;

    [BoxGroup("Stat Colors")] [SerializeField]
    private StatColors _statColors;

    private void Awake()
    {
        if (_statsPosition == null) return;
        _unitStatPosition.x = _statsPosition.localPosition.x;
        _structureStatPosition.x = _statsPosition.localPosition.x;
    }

    protected void UpdateUnitStats(LUnit lUnit)
    {
        UpdatePosition(lUnit);
        UpdateStatsStatus(lUnit);

        string hpHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.HpColor)}";
        string atkHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.AtkColor)}";
        string rngHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.RngColor)}";
        string evaHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.EvaColor)}";
        string apHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.APColor)}";
        string incHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.IncColor)}";
        _hpText.text = $"HP: <b><color={hpHex}>{lUnit.HitPoints}</color>/<color={hpHex}>{lUnit.TotalHitPoints}</color>";
        _atkText.text = $"ATK: <b><color={atkHex}>{lUnit.AttackFactor}</color>";
        _rangeText.text = $"RNG: <b><color={rngHex}>{lUnit.AttackRange}</color>";
        _evasionText.text = $"EVA: <b><color={evaHex}>{lUnit.EvasionFactor}</color>";
        _apText.text = $"AP: <b><color={apHex}>{lUnit.ActionPoints}</color>";

        if (_incomeText == null) return;
        if (lUnit is LStructure lStructure && lStructure.UnitStats is StructureStats structureStats)
            _incomeText.text = $"INC: <b><color={incHex}>{structureStats.Income}</color>";
    }

    private void UpdatePosition(LUnit lUnit)
    {
        if (_statsPosition == null) return;
        _statsPosition.localPosition = _unitStatPosition;
        if (lUnit is LStructure)
            _statsPosition.localPosition = _structureStatPosition;
    }

    private void UpdateStatsStatus(LUnit lUnit)
    {
        _hpText.gameObject.SetActive(true);
        _atkText.gameObject.SetActive(true);
        _rangeText.gameObject.SetActive(true);
        _evasionText.gameObject.SetActive(true);
        _apText.gameObject.SetActive(true);
        _incomeText?.gameObject.SetActive(false);

        if (lUnit is LStructure)
        {
            _atkText.gameObject.SetActive(false);
            _rangeText.gameObject.SetActive(false);
            _evasionText.gameObject.SetActive(false);
            _apText.gameObject.SetActive(false);
            _incomeText?.gameObject.SetActive(true);
        }
    }
}
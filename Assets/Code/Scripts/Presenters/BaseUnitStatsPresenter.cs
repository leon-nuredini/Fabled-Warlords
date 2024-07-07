using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class BaseUnitStatsPresenter : MonoBehaviour, IUnitPresenter
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _defText;
    [SerializeField] private TextMeshProUGUI _atkText;
    [SerializeField] private TextMeshProUGUI _rangeText;
    [SerializeField] private TextMeshProUGUI _evasionText;
    [SerializeField] private TextMeshProUGUI _apText;

    [BoxGroup("Stat Colors")] [SerializeField] private StatColors _statColors;

    protected void UpdateUnitStats(LUnit lUnit)
    {
        string hpHex  = $"#{ColorUtility.ToHtmlStringRGB(_statColors.HpColor)}";
        string defHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.DefColor)}";
        string atkHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.AtkColor)}";
        string rngHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.RngColor)}";
        string evaHex = $"#{ColorUtility.ToHtmlStringRGB(_statColors.EvaColor)}";
        string apHex  = $"#{ColorUtility.ToHtmlStringRGB(_statColors.APColor)}";
        _hpText.text = $"HP: <b><color={hpHex}>{lUnit.HitPoints}</color>/<color={hpHex}>{lUnit.TotalHitPoints}</color>";
        _atkText.text = $"ATK: <b><color={atkHex}>{lUnit.AttackFactor}</color>";
        _rangeText.text = $"RNG: <b><color={rngHex}>{lUnit.AttackRange}</color>";
        _evasionText.text = $"EVA: <b><color={evaHex}>{lUnit.EvasionFactor}</color>";
        _apText.text = $"AP: <b><color={apHex}>{lUnit.ActionPoints}</color>";
    }
}
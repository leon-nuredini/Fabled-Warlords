using TMPro;
using UnityEngine;

public class HitChanceText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _hitChanceText;
    [SerializeField] private HitChanceTextColor _hitChanceTextColor;

    private LUnit _lUnit;

    private void Awake()
    {
        if (_hitChanceText != null) _hitChanceText.gameObject.SetActive(false);
        _lUnit = GetComponent<LUnit>();
    }

    public void UpdateHitChanceText(LUnit attacker)
    {
        if (_hitChanceText is null) return;
        int evasionChance = 100 - _lUnit.GetEvasionChance(attacker);
        evasionChance = Mathf.Clamp(evasionChance, 0, 100);
        _hitChanceText.text = $"{evasionChance}%";
        _hitChanceText.color = _hitChanceTextColor.GetHitChanceColor(evasionChance);
        _hitChanceText.gameObject.SetActive(true);
    }

    public void HideHitChanceText() => _hitChanceText.gameObject.SetActive(false);
}
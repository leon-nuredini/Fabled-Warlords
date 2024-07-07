using TbsFramework.Units;
using TMPro;
using UnityEngine;

public class HPUpdate : MonoBehaviour
{
    public TextMeshPro hitPointsText;

    private Unit _unit;

    private void Awake() => _unit = GetComponent<Unit>();
    private void Start() => UpdateHpText();
    private void OnEnable() => _unit.OnHitpointsChange += UpdateHpText;
    private void OnDisable() => _unit.OnHitpointsChange -= UpdateHpText;

    private void UpdateHpText()
    {
        int hitPoints = _unit.HitPoints;
        if (hitPoints < 0) hitPoints = 0;
        hitPointsText.text = hitPoints.ToString();
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "UnitCounterColors", menuName = "UnitCounterColors/UnitCounterColors", order = 0)]
public class UnitCounterColors : ScriptableObject
{
    [SerializeField] private Color _strongCounterColor;
    [SerializeField] private Color _mediumCounterColor;
    [SerializeField] private Color _neutralCounterColor;
    [SerializeField] private Color _weakCounterColor;
    [SerializeField] private Color _veryWeakCounterColor;

    public Color StrongCounterColor => _strongCounterColor;
    public Color MediumCounterColor => _mediumCounterColor;
    public Color NeutralCounterColor => _neutralCounterColor;
    public Color WeakCounterColor => _weakCounterColor;
    public Color VeryWeakCounterColor => _veryWeakCounterColor;
}
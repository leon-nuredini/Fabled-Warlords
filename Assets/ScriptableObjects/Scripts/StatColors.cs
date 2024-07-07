using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Colors", menuName = "StatColors/StatColors", order = 0)]
public class StatColors : ScriptableObject
{
    [BoxGroup("Stats Colors")] [SerializeField] private Color _hpColor;
    [BoxGroup("Stats Colors")] [SerializeField] private Color _defColor;
    [BoxGroup("Stats Colors")] [SerializeField] private Color _atkColor;
    [BoxGroup("Stats Colors")] [SerializeField] private Color _rngColor;
    [BoxGroup("Stats Colors")] [SerializeField] private Color _evaColor;
    [BoxGroup("Stats Colors")] [SerializeField] private Color _apColor;

    public Color HpColor => _hpColor;
    public Color DefColor => _defColor;
    public Color AtkColor => _atkColor;
    public Color RngColor => _rngColor;
    public Color EvaColor => _evaColor;
    public Color APColor => _apColor;
}

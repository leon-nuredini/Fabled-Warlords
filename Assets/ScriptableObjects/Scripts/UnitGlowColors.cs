using UnityEngine;

[CreateAssetMenu(fileName = "UnitGlowColors", menuName = "UnitGlowColors/UnitGlowColors", order = 0)]
public class UnitGlowColors : ScriptableObject
{
    [SerializeField] private Color _playerGlowColor;
    [SerializeField] private Color _enemyGlowColor;
    [SerializeField] private Color _prisonerGlowColor;
    [SerializeField] private float _glowValue;

    public Color PlayerGlowColor => _playerGlowColor;
    public Color EnemyGlowColor => _enemyGlowColor;
    public Color PrisonerGlowColor => _prisonerGlowColor;
    public float GlowValue => _glowValue;
}

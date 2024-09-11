using UnityEngine;

[CreateAssetMenu(fileName = "HitChanceTextColor", menuName = "HitChanceTextColor/HitChanceTextColor", order= 0)]
public class HitChanceTextColor : ScriptableObject
{
    [SerializeField] private Color _color100;
    [SerializeField] private Color _color90;
    [SerializeField] private Color _color80;
    [SerializeField] private Color _color70;
    [SerializeField] private Color _color60;
    [SerializeField] private Color _color50;
    [SerializeField] private Color _color40;
    [SerializeField] private Color _color30;
    [SerializeField] private Color _color20;
    [SerializeField] private Color _color10;

    public Color GetHitChanceColor(int hitChance)
    {
        if (hitChance > 90) return _color100;
        if (hitChance > 80) return _color90;
        if (hitChance > 70) return _color80;
        if (hitChance > 60) return _color70;
        if (hitChance > 50) return _color60;
        if (hitChance > 40) return _color50;
        if (hitChance > 30) return _color40;
        if (hitChance > 20) return _color30;
        if (hitChance > 10) return _color20;
        return _color10;
    }
}

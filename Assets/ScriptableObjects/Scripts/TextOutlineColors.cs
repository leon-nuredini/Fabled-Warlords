using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextOutlineColors", menuName = "TextOutlineColors/TextOutlineColors", order = 0)]
public class TextOutlineColors : ScriptableObject
{
    [ColorUsage(true, true)] [SerializeField] private Color _backstabColor;
    [ColorUsage(true, true)] [SerializeField] private Color _chargingColor;
    [ColorUsage(true, true)] [SerializeField] private Color _guardianAuraColor;
    [ColorUsage(true, true)] [SerializeField] private Color _ironWillColor;
    [ColorUsage(true, true)] [SerializeField] private Color _parryColor;
    [ColorUsage(true, true)] [SerializeField] private Color _poisonColor;
    [ColorUsage(true, true)] [SerializeField] private Color _smiteColor;
    [ColorUsage(true, true)] [SerializeField] private Color _spiritWardColor;
    [ColorUsage(true, true)] [SerializeField] private Color _stillStrikeColor;
    [ColorUsage(true, true)] [SerializeField] private Color _stunColor;
    [ColorUsage(true, true)] [SerializeField] private Color _weakenedColor;

    public Color BackstabColor => _backstabColor;
    public Color ChargingColor => _chargingColor;
    public Color GuardianAuraColor => _guardianAuraColor;
    public Color IronWillColor => _ironWillColor;
    public Color ParryColor => _parryColor;
    public Color PoisonColor => _poisonColor;
    public Color SmiteColor => _smiteColor;
    public Color SpiritWardColor => _spiritWardColor;
    public Color StillStrikeColor => _stillStrikeColor;
    public Color StunColor => _stunColor;
    public Color WeakenedColor => _weakenedColor;
}
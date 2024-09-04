using System;
using System.Collections;
using DG.Tweening;
using TbsFramework.Grid;
using TbsFramework.Players;
using UnityEngine;

public class GlowAction : MonoBehaviour
{
    [SerializeField] private Color _playerGlowColor;
    [SerializeField] private Color _enemyGlowColor;
    [SerializeField] private Color _prisonerGlowColor;
    [SerializeField] private float _glowValue = 0.5f;

    private LUnit _lUnit;
    private PrisonerAbility _prisonerAbility;
    private Material _material;

    private bool _isPlayerUnit;

    private readonly string _glowColor = "_GlowColor";
    private readonly string _glowAmount = "_Glow";

    private void Awake()
    {
        _lUnit = GetComponent<LUnit>();
        _prisonerAbility = GetComponent<PrisonerAbility>();
        _material = _lUnit.MaskSpriteRenderer.material;
    }

    private void OnEnable()
    {
        UnitGlow.OnAnyEnableGlow += Glow;
        UnitGlow.OnAnyDisableGlow += DisableGlow;
    }

    private void OnDisable()
    {
        UnitGlow.OnAnyEnableGlow -= Glow;
        UnitGlow.OnAnyDisableGlow -= DisableGlow;
    }

    private void Start()
    {
        int unitPlayerNumber = _lUnit.PlayerNumber;
        foreach (var player in CellGrid.Instance.Players)
        {
            if (player is HumanPlayer && player.PlayerNumber == unitPlayerNumber)
                _isPlayerUnit = true;
        }
    }

    private void Glow()
    {
        Color glowColor = _isPlayerUnit ? _playerGlowColor : _enemyGlowColor;
        if (_prisonerAbility != null && _prisonerAbility.IsPrisoner) glowColor = _prisonerGlowColor;
        _material.SetColor(_glowColor, glowColor);
        _material.SetFloat(_glowAmount, _glowValue);
    }

    private void DisableGlow()
    {
        _material.SetFloat(_glowAmount, 0f);
    }
}
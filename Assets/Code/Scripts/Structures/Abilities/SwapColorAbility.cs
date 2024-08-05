using System.Collections.Generic;
using NaughtyAttributes;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class SwapColorAbility : Ability
{
    [BoxGroup("Unit Sprite Renderer")] [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [BoxGroup("Colors")] [SerializeField] private Color _colorNeutral;
    [BoxGroup("Colors")] [SerializeField] private List<Color> _colorList;

    private ICapturable _capturable;
    private LUnit _capturer;
    private LUnit _lUnit;

    private void Awake()
    {
        _lUnit = GetComponent<LUnit>();
        _capturable = GetComponent<ICapturable>();

        UpdateFlagColor(_lUnit.PlayerNumber);
    }

    private void OnEnable()
    {
        _capturable.OnCaptured += CaptureStructure;
        _capturable.OnAbandoned += AbandonStructure;
    }

    private void OnDisable()
    {
        _capturable.OnCaptured -= CaptureStructure;
        _capturable.OnAbandoned -= AbandonStructure;
    }

    private void CaptureStructure(LUnit capturer)
    {
        _capturer = capturer;
        LStructure lStructure = _capturable as LStructure;
        if (lStructure == null) return;

        UnitReference.PlayerNumber = _capturer.PlayerNumber;
        UpdateFlagColor(_capturer.PlayerNumber);
    }

    private void UpdateFlagColor(int playerNumber)
    {
        if (_colorList.Count < playerNumber)
            _spriteRenderer.color = _colorNeutral;
        else
            _spriteRenderer.color = _colorList[playerNumber];
    }

    private void AbandonStructure()
    {
        UnitReference.PlayerNumber = 99;
        _spriteRenderer.color = _colorNeutral;
    }
}
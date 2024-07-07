using System.Collections;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class SwapColorAbility : Ability
{
    [BoxGroup("Unit Sprite Renderer")] [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [BoxGroup("Sprites")] [SerializeField] private Sprite _spriteRed;
    [BoxGroup("Sprites")] [SerializeField] private Sprite _spriteGreen;
    [BoxGroup("Sprites")] [SerializeField] private Sprite _spriteBlue;
    [BoxGroup("Sprites")] [SerializeField] private Sprite _spriteNeutral;

    private ICapturable _capturable;
    private LUnit       _capturer;

    private void Awake() => _capturable = GetComponent<ICapturable>();

    private void OnEnable()
    {
        _capturable.OnCaptured  += CaptureStructure;
        _capturable.OnAbandoned += AbandonStructure;
    }

    private void OnDisable()
    {
        _capturable.OnCaptured  -= CaptureStructure;
        _capturable.OnAbandoned -= AbandonStructure;
    }

    private void CaptureStructure(LUnit capturer)
    {
        _capturer = capturer;
        LStructure _lStructure = _capturable as LStructure;
        if (_lStructure == null) return;

        UnitReference.PlayerNumber = _capturer.PlayerNumber;
        switch (_capturer.Faction)
        {
            case UnitFaction.Red:
                _spriteRenderer.sprite = _spriteRed;
                break;
            case UnitFaction.Green:
                _spriteRenderer.sprite = _spriteGreen;
                break;
            case UnitFaction.Blue:
                _spriteRenderer.sprite = _spriteBlue;
                break;
        }
    }

    private void AbandonStructure()
    {
        UnitReference.PlayerNumber = 99;
        _spriteRenderer.sprite     = _spriteNeutral;
    }
}
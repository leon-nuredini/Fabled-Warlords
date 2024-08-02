using NaughtyAttributes;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class SwapColorAbility : Ability
{
    [BoxGroup("Unit Sprite Renderer")] [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [BoxGroup("Sprites")] [SerializeField] private Color _colorBlue;
    [BoxGroup("Sprites")] [SerializeField] private Color _colorRed;
    [BoxGroup("Sprites")] [SerializeField] private Color _colorGreen;
    [BoxGroup("Sprites")] [SerializeField] private Color _colorNeutral;

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
            case UnitFaction.Blue:
                _spriteRenderer.color = _colorBlue;
                break;
            case UnitFaction.Red:
                _spriteRenderer.color = _colorRed;
                break;
            case UnitFaction.Green:
                _spriteRenderer.color = _colorGreen;
                break;
        }
    }

    private void AbandonStructure()
    {
        UnitReference.PlayerNumber = 99;
        _spriteRenderer.color     = _colorNeutral;
    }
}
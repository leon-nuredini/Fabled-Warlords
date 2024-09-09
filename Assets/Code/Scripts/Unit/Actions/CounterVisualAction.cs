using UnityEngine;

public class CounterVisualAction : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _counterVisualSpriteRenderer;
    [SerializeField] private UnitCounterColors _unitCounterColors;

    private void Start() => DisableCounterIcon();

    public void UpdateCounterIcon(float damageFactor)
    {
        if (damageFactor >= 1.5f)
            UpdateCounterVisual(true, _unitCounterColors.StrongCounterColor, 1.5f);
        else if (damageFactor >= 1.25f)
            UpdateCounterVisual(true, _unitCounterColors.MediumCounterColor, 1.25f);
        else if (damageFactor <= .5f)
            UpdateCounterVisual(true, _unitCounterColors.VeryWeakCounterColor, .5f);
        else if (damageFactor <= .75f)
            UpdateCounterVisual(true, _unitCounterColors.WeakCounterColor, .75f);
        else
            DisableCounterIcon();
    }

    public void DisableCounterIcon() => UpdateCounterVisual(false, Color.white, 1f);

    private void UpdateCounterVisual(bool isEnabled, Color color, float damageFactor)
    {
        _counterVisualSpriteRenderer.gameObject.SetActive(isEnabled);
        _counterVisualSpriteRenderer.color = color;
        _counterVisualSpriteRenderer.flipY = damageFactor < 1f;
    }
}
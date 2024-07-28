using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UnitCounterPresenter : MonoBehaviour
{
    [BoxGroup("Images")] [SerializeField] private Image _spearImage;
    [BoxGroup("Images")] [SerializeField] private Image _swordImage;
    [BoxGroup("Images")] [SerializeField] private Image _rangedImage;
    [BoxGroup("Images")] [SerializeField] private Image _mountedImage;
    [BoxGroup("Images")] [SerializeField] private Image _mageImage;
    [BoxGroup("Images")] [SerializeField] private Image _monsterImage;

    [BoxGroup("Colors")] [SerializeField] private Color _strongEffectiveColor;
    [BoxGroup("Colors")] [SerializeField] private Color _mediumEffectiveColor;
    
    private void OnEnable() => LUnit.OnAnyDisplayUnitInformation += UpdateCounterImages;
    private void OnDisable() => LUnit.OnAnyDisplayUnitInformation -= UpdateCounterImages;

    private void UpdateCounterImages(LUnit lUnit)
    {
        UpdateColor(_spearImage, lUnit.UnitClassCounter.VSSpearInfantryCounter);
        UpdateColor(_swordImage, lUnit.UnitClassCounter.VSSwordInfantryCounter);
        UpdateColor(_rangedImage, lUnit.UnitClassCounter.VSRangedFactor);
        UpdateColor(_mountedImage, lUnit.UnitClassCounter.VSCavalryFactor);
        UpdateColor(_mageImage, lUnit.UnitClassCounter.VSMageFactor);
        UpdateColor(_monsterImage, lUnit.UnitClassCounter.VSMonsterFactor);
    }

    private void UpdateColor(Image image, float counterValue)
    {
        image.gameObject.SetActive(counterValue > 1f);
        if (counterValue >= 1.5f) image.color = _strongEffectiveColor;
        else if (counterValue >= 1.25f) image.color = _mediumEffectiveColor;
    }
}

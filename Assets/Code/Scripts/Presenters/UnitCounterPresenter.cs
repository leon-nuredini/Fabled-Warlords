using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UnitCounterPresenter : MonoBehaviour
{
    [BoxGroup("Section")] [SerializeField] private GameObject _effectivePanel;

    [BoxGroup("Images")] [SerializeField] private Image _spearImage;
    [BoxGroup("Images")] [SerializeField] private Image _swordImage;
    [BoxGroup("Images")] [SerializeField] private Image _rangedImage;
    [BoxGroup("Images")] [SerializeField] private Image _mountedImage;
    [BoxGroup("Images")] [SerializeField] private Image _mageImage;
    [BoxGroup("Images")] [SerializeField] private Image _monsterImage;

    [BoxGroup("Colors")] [SerializeField] private UnitCounterColors _unitCounterColor;

    protected virtual void OnEnable() => LUnit.OnAnyDisplayUnitInformation += UpdateCounterImages;
    protected virtual void OnDisable() => LUnit.OnAnyDisplayUnitInformation -= UpdateCounterImages;

    protected void UpdateCounterImages(LUnit lUnit)
    {
        if (_effectivePanel != null)
        {
            _effectivePanel.SetActive(true);

            if (lUnit is LStructure)
            {
                _effectivePanel.SetActive(false);
                return;
            }
        }

        UpdateColor(_spearImage, lUnit.UnitClassCounter.VSSpearInfantryCounter);
        UpdateColor(_swordImage, lUnit.UnitClassCounter.VSSwordInfantryCounter);
        UpdateColor(_rangedImage, lUnit.UnitClassCounter.VSRangedFactor);
        UpdateColor(_mountedImage, lUnit.UnitClassCounter.VSCavalryFactor);
        UpdateColor(_mageImage, lUnit.UnitClassCounter.VSMageFactor);
        UpdateColor(_monsterImage, lUnit.UnitClassCounter.VSMonsterFactor);
    }

    protected void UpdateColor(Image image, float counterValue)
    {
        if (counterValue >= 1.5f) image.color = _unitCounterColor.StrongCounterColor;
        else if (counterValue >= 1.25f) image.color = _unitCounterColor.MediumCounterColor;
        else if (counterValue >= 1f) image.color = _unitCounterColor.NeutralCounterColor;
        else if (counterValue >= .75f) image.color = _unitCounterColor.WeakCounterColor;
        else if (counterValue >= .5f) image.color = _unitCounterColor.VeryWeakCounterColor;
    }
}
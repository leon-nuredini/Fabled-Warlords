using UnityEngine;
using UnityEngine.UI;

public class UnitStatusEffectsPresenter : MonoBehaviour
{
    [SerializeField] private Image _weaknessImage;
    [SerializeField] private Image _stunImage;
    [SerializeField] private Image _poisonImage;
    
    private void OnEnable() => LUnit.OnAnyDisplayUnitInformation += UpdateStatusEffectImages;
    private void OnDisable() => LUnit.OnAnyDisplayUnitInformation -= UpdateStatusEffectImages;

    private void UpdateStatusEffectImages(LUnit lUnit)
    {
        _weaknessImage.gameObject.SetActive(lUnit.StatusEffectsController.IsStatusApplied<Weaken>());
        _stunImage.gameObject.SetActive(lUnit.StatusEffectsController.IsStatusApplied<Stun>());
        _poisonImage.gameObject.SetActive(lUnit.StatusEffectsController.IsStatusApplied<Poison>());
    }
}

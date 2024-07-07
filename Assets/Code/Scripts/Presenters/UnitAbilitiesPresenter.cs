public class UnitAbilitiesPresenter : BaseUnitAbilitiesPresenter
{
    private void OnEnable()  => LUnit.OnAnyDisplayUnitInformation += UpdateUnitAbilities;
    private void OnDisable() => LUnit.OnAnyDisplayUnitInformation -= UpdateUnitAbilities;
}
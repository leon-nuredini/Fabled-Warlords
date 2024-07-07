public class UnitStatsPresenter : BaseUnitStatsPresenter
{
    private void OnEnable()  => LUnit.OnAnyDisplayUnitInformation += UpdateUnitStats;
    private void OnDisable() => LUnit.OnAnyDisplayUnitInformation -= UpdateUnitStats;
}
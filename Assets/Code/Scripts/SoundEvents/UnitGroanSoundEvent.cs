public class UnitGroanSoundEvent : BaseSoundEvent
{
    private LUnit _lUnit;

    private void Awake() => _lUnit = GetComponent<LUnit>();
    
    private void OnEnable()  => _lUnit.OnDie += OnUnitDie;
    private void OnDisable() => _lUnit.OnDie -= OnUnitDie;

    private void OnUnitDie(UnitDirection unitDirection) => InvokeSoundEvent();
}

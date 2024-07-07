public class UnitAttackSoundEvent : BaseSoundEvent
{
    private LUnit _lUnit;

    private void Awake()     => _lUnit = GetComponent<LUnit>();
    private void OnEnable()  => _lUnit.OnAttack += PlayAttackSound;
    private void OnDisable() => _lUnit.OnAttack -= PlayAttackSound;

    private void PlayAttackSound(UnitDirection unitDirection) => InvokeSoundEvent();
}
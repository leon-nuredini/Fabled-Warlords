public class SelectUnitSoundEvent : BaseSoundEvent
{
    private ObjectHolder _objectHolder;

    private void Awake() => _objectHolder = GetComponent<ObjectHolder>();
    private void OnEnable() => _objectHolder.OnSelectUnit += InvokeSoundEvent;
    private void OnDisable() => _objectHolder.OnSelectUnit -= InvokeSoundEvent;
}
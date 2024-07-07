public class RecruitUnitSoundEvent : BaseSoundEvent
{
    private void OnEnable()  => RecruitmentController.OnAnyNewUnitRecruited += InvokeSoundEvent;
    private void OnDisable() => RecruitmentController.OnAnyNewUnitRecruited += InvokeSoundEvent;
}
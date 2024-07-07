using UnityEngine;
using Sonity;

public abstract class BaseSoundEvent : MonoBehaviour
{
    [SerializeField] private SoundEvent _soundEvent;

    public SoundEvent SoundEvent => _soundEvent;
    
    protected virtual void InvokeSoundEvent() => _soundEvent.Play2D();
}

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSoundEvent : BaseSoundEvent
{
    [BoxGroup("Buttons"), SerializeField] private Button[] _buttonArray;

    private void Awake()
    {
        for (int i = 0; i < _buttonArray.Length; i++)
            _buttonArray[i].onClick.AddListener(InvokeSoundEvent);
    }
}
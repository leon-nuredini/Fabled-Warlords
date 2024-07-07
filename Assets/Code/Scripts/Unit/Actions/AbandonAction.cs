using TbsFramework.Units;
using UnityEngine;

public class AbandonAction : MonoBehaviour
{
    private LStructure _lStructure;
    private AbandonTweener _tween;

    private void Awake()
    {
        _tween       = GetComponentInChildren<AbandonTweener>();
        _lStructure = GetComponent<LStructure>();
    }
    private void OnEnable()
    {
        if (_lStructure == null) return;
        _lStructure.OnCaptured  += Execute;
        _lStructure.OnAbandoned += Execute;
    }

    private void OnDisable()
    {
        if (_lStructure == null) return;
        _lStructure.OnCaptured  += Execute;
        _lStructure.OnAbandoned -= Execute;
    }

    private void Execute(Unit aggressor)
    {
        if (_tween != null)
            _tween.Execute();
    }
    
    private void Execute()
    {
        if (_tween != null)
            _tween.Execute();
    }
}
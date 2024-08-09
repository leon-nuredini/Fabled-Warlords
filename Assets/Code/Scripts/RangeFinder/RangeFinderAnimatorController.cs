using UnityEngine;

public class RangeFinderAnimatorController : MonoBehaviour
{
    private Animator _animator;

    private void Awake() => _animator = GetComponent<Animator>();

    public void PlayAnimation(RangeFinderType rangeFinderType)
    {
        switch (rangeFinderType)
        {
            case RangeFinderType.None:
                _animator.SetInteger("int", 3);
                break;
            case RangeFinderType.Red:
                _animator.SetInteger("int", 0);
                break;
            case RangeFinderType.Green:
                _animator.SetInteger("int", 1);
                break;
            case RangeFinderType.Blue:
                _animator.SetInteger("int", 2);
                break;
        }
    }
}
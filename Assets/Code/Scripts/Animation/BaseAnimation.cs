using System;
using UnityEngine;

public class BaseAnimation
{
    [SerializeField] private AnimationClip _animationClip;

    private bool _hasIntId;
    private int  _animationClipId;

    public void PlayAnimation(Animator animator)
    {
        if (_animationClip == null) return;
        if (!_hasIntId)
        {
            _hasIntId        = true;
            _animationClipId = Animator.StringToHash(_animationClip.name);
        }

        animator.CrossFade(_animationClipId, 0f);
    }
}
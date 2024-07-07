using System;
using UnityEngine;

[Serializable]
public class PathGraphics
{
    [Header("Move Once")] [SerializeField] private Sprite _moveUpOnceSprite;
    [SerializeField]                       private Sprite _moveDownOnceSprite;
    [SerializeField]                       private Sprite _moveLeftOnceSprite;
    [SerializeField]                       private Sprite _moveRightOnceSprite;

    [Header("Start")] [SerializeField] private Sprite _startUpSprite;
    [SerializeField]                   private Sprite _startDownSprite;
    [SerializeField]                   private Sprite _startRightSprite;
    [SerializeField]                   private Sprite _startLeftSprite;

    [Header("Continue")] [SerializeField] private Sprite _continueVerticalSprite;
    [SerializeField]                      private Sprite _continueHorizontalSprite;

    [Header("Angles")] [SerializeField] private Sprite _angleUpRightSprite;
    [SerializeField]                    private Sprite _angleUpLeftSprite;
    [SerializeField]                    private Sprite _angleDownRightSprite;
    [SerializeField]                    private Sprite _angleDownLeftSprite;

    [Header("End")] [SerializeField] private Sprite _endUpSprite;
    [SerializeField]                 private Sprite _endDownSprite;
    [SerializeField]                 private Sprite _endRightSprite;
    [SerializeField]                 private Sprite _endLeftSprite;

    public Sprite MoveUpOnceSprite         => _moveUpOnceSprite;
    public Sprite MoveDownOnceSprite       => _moveDownOnceSprite;
    public Sprite MoveLeftOnceSprite       => _moveLeftOnceSprite;
    public Sprite MoveRightOnceSprite      => _moveRightOnceSprite;
    public Sprite StartUpSprite            => _startUpSprite;
    public Sprite StartDownSprite          => _startDownSprite;
    public Sprite StartRightSprite         => _startRightSprite;
    public Sprite StartLeftSprite          => _startLeftSprite;
    public Sprite ContinueVerticalSprite   => _continueVerticalSprite;
    public Sprite ContinueHorizontalSprite => _continueHorizontalSprite;
    public Sprite AngleUpRightSprite       => _angleUpRightSprite;
    public Sprite AngleUpLeftSprite        => _angleUpLeftSprite;
    public Sprite AngleDownRightSprite     => _angleDownRightSprite;
    public Sprite AngleDownLeftSprite      => _angleDownLeftSprite;
    public Sprite EndUpSprite              => _endUpSprite;
    public Sprite EndDownSprite            => _endDownSprite;
    public Sprite EndRightSprite           => _endRightSprite;
    public Sprite EndLeftSprite            => _endLeftSprite;
}
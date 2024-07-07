using System;
using Sonity;
using TbsFramework.Grid;
using TbsFramework.Players;
using UnityEngine;

public class GameOverSoundEvent : BaseSoundEvent
{
    public static event Action OnAnyPlayGameEndSFX;

    [SerializeField] private SoundEvent _alternateSoundEvent;

    private void OnEnable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.GameEnded += PlayGameOverSound;
    }

    private void OnDisable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.GameEnded -= PlayGameOverSound;
    }

    private void PlayGameOverSound(object sender, EventArgs eventArgs)
    {
        OnAnyPlayGameEndSFX?.Invoke();
        int playerNumber = (sender as CellGrid).CurrentPlayerNumber;

        for (int i = 0; i < CellGrid.Instance.Players.Count; i++)
        {
            if (CellGrid.Instance.Players[i].PlayerNumber == playerNumber &&
                CellGrid.Instance.Players[i] is HumanPlayer)
            {
                InvokeSoundEvent();
                return;
            }
        }

        InvokeAlternateSoundEvent();
    }

    private void InvokeAlternateSoundEvent() => _alternateSoundEvent.Play2D();
}
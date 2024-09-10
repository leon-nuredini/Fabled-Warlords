using System.Collections.Generic;
using NaughtyAttributes;
using TbsFramework.Players;
using UnityEngine;

namespace TbsFramework.Grid.GameResolvers
{
    public abstract class GameEndCondition : MonoBehaviour
    {
        public abstract GameResult CheckCondition(CellGrid cellGrid);
        
        public GameResult WinGame()
        {
            List<int> playersAlive = new List<int>();
            List<int> playersDead = new List<int>();

            foreach (var player in CellGrid.Instance.Players)
            {
                if (player is HumanPlayer)
                    playersAlive.Add(player.PlayerNumber);
                else if (player is AIPlayer)
                    playersDead.Add(player.PlayerNumber);
            }
            
            return new GameResult(true, playersAlive, playersDead);
        }
    }
}

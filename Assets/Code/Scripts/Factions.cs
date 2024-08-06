using Singleton;
using UnityEngine;

public class Factions : SceneSingleton<Factions>
{
    [SerializeField] private PlayerFaction _playerFaction;
    
    public PlayerFaction PlayerFaction => _playerFaction;

    private void Awake()
    {
        //dirty fix. Without the awake method the _playerFaction is null
    }
}

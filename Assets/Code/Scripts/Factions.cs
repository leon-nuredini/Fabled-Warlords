using Singleton;

public class Factions : SceneSingleton<Factions>
{
    public PlayerFaction _playerFaction;
    
    public PlayerFaction PlayerFaction => _playerFaction;

    private void Awake()
    {
        //dirty fix. Without the awake method the _playerFaction is null
    }
}

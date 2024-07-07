using System;
using Singleton;
using UnityEngine;

public class GameSettings : SceneSingleton<GameSettings>
{
    [SerializeField] private Preferences _preferences;

    public Preferences Preferences => _preferences;

    private void Awake()
    {
        LoadData();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void LoadData()
    {
        
    }

    private void ToggleMusic()
    {
        
    }

    private void ToggleSfx()
    {
        
    }
}

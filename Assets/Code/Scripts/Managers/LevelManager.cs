using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using NaughtyAttributes;
using System.Runtime.InteropServices;

public class LevelManager : Singleton<LevelManager>
{
    public static event Action OnAnyInitLevelLoading;
    public static event Action OnAnyNewLevelLoaded;

    public static bool openLevelSelection;

    [SerializeField] [Scene] private string _menuCanvasScene = "MenuCanvas";
    [SerializeField] [Scene] private string _managersScene = "Managers";

    private int  _levelToLoadIndex;
    private bool _isLoadingNewLevel;

    [DllImport("__Internal")]
   private static extern void StartLevelEvent(int level);

    protected override void Awake()
    {
        base.Awake();
        SceneManager.LoadScene(_managersScene,   LoadSceneMode.Additive);
        SceneManager.LoadScene(_menuCanvasScene, LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        FadePresenter.OnFadeIn                  += LoadLevel;
        UIReturnToMenu.OnAnyClickYesButton      += StartLoadingLevel;
        UICampaign.OnAnyClickedPlayButton       += StartLoadingLevel;
        UIGameOver.OnAnyClickReturnToMenuButton += GoToLevelSelection;
        SceneManager.sceneLoaded                += OnNewSceneLoaded;
    }

    private void OnDisable()
    {
        FadePresenter.OnFadeIn                  -= LoadLevel;
        UIReturnToMenu.OnAnyClickYesButton      -= StartLoadingLevel;
        UICampaign.OnAnyClickedPlayButton       -= StartLoadingLevel;
        UIGameOver.OnAnyClickReturnToMenuButton -= GoToLevelSelection;
        SceneManager.sceneLoaded                -= OnNewSceneLoaded;
    }

    private void GoToLevelSelection(int levelIndex)
    {
        openLevelSelection = true;
        StartLoadingLevel(levelIndex);
    }

    private void StartLoadingLevel(int levelIndex)
    {
        if (_isLoadingNewLevel) return;
        _isLoadingNewLevel = true;
        _levelToLoadIndex  = levelIndex;
        OnAnyInitLevelLoading?.Invoke();
    }

    private void LoadLevel() 
    {
        OnAnyNewLevelLoaded?.Invoke();
        #if UNITY_EDITOR
            SceneManager.LoadScene(_levelToLoadIndex);
            return;
        #endif

        if (_levelToLoadIndex > 0)
            StartLevelEvent(_levelToLoadIndex);
        CoolMathAds.instance.InitiateAds();
        SceneManager.LoadScene(_levelToLoadIndex);
    }

    private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode) => _isLoadingNewLevel = false;
}
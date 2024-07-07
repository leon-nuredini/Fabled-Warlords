using Singleton;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class FadePresenter : Singleton<FadePresenter>
{
    public static event Action OnFadeIn;

    private GraphicRaycaster _graphicRaycaster;

    private CanvasGroupFadeInTweener  _fadeInTweener;
    private CanvasGroupFadeOutTweener _fadeOutTweener;

    protected override void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _fadeInTweener    = GetComponentInChildren<CanvasGroupFadeInTweener>();
        _fadeOutTweener   = GetComponentInChildren<CanvasGroupFadeOutTweener>();
        base.Awake();
    }

    private void Start() => InitiateFadeOut();

    private void OnEnable()
    {
        LevelManager.OnAnyInitLevelLoading += InitiateFadeIn;
        _fadeInTweener.OnFadeInBegin       += OnFadeInBegin;
        _fadeInTweener.OnFadeInComplete    += OnFadeInComplete;
        _fadeOutTweener.OnFadeOutComplete  += OnFadeOutComplete;
        SceneManager.sceneLoaded           += OnSceneLoaded;
    }

    private void OnDisable()
    {
        LevelManager.OnAnyInitLevelLoading -= InitiateFadeIn;
        _fadeInTweener.OnFadeInBegin       -= OnFadeInBegin;
        _fadeInTweener.OnFadeInComplete    -= OnFadeInComplete;
        _fadeOutTweener.OnFadeOutComplete  -= OnFadeOutComplete;
        SceneManager.sceneLoaded           -= OnSceneLoaded;
    }

    private void InitiateFadeIn()    => _fadeInTweener.Execute();
    private void InitiateFadeOut()   => _fadeOutTweener.Execute();
    private void OnFadeInBegin()     => _graphicRaycaster.enabled = true;
    private void OnFadeInComplete()  => OnFadeIn?.Invoke();
    private void OnFadeOutComplete() => _graphicRaycaster.enabled = false;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => InitiateFadeOut();
}
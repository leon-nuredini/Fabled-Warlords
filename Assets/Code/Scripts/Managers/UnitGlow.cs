using UnityEngine;
using System;
using System.Collections;

public class UnitGlow : MonoBehaviour
{
    public static event Action OnAnyEnableGlow;
    public static event Action OnAnyDisableGlow;

    [SerializeField] private float _glowDuration = 0.5f;

    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_glowDuration);
    }

    private void Start()
    {
        if (GameSettings.Instance != null && GameSettings.Instance.Preferences != null)
            TryToEnableGlow(GameSettings.Instance.Preferences.EnableUnitGlow);
    }

    private void OnEnable()
    {
        if (GameSettings.Instance != null && GameSettings.Instance.Preferences != null)
            GameSettings.Instance.Preferences.OnUpdateUnitGlow += TryToEnableGlow;
    }

    private void OnDisable()
    {
        if (GameSettings.Instance != null && GameSettings.Instance.Preferences != null)
            GameSettings.Instance.Preferences.OnUpdateUnitGlow -= TryToEnableGlow;
    }

    private void TryToEnableGlow(bool enableGlow)
    {
        if (enableGlow)
        {
            TryToStopCoroutine();
            _coroutine = StartCoroutine(InitGlow());
        }
        else
        {
            DisableGlow();
        }
    }

    private void DisableGlow()
    {
        TryToStopCoroutine();
        OnAnyDisableGlow?.Invoke();
    }

    private IEnumerator InitGlow()
    {
        while (true)
        {
            yield return _wait;
            OnAnyEnableGlow?.Invoke();
            yield return _wait;
            OnAnyDisableGlow?.Invoke();
        }
    }

    private void TryToStopCoroutine()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
    }
}
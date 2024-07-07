using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Preferences", menuName = "Preferences/General", order = 0)]
public class Preferences : ScriptableObject
{
    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private bool _enableMusic = true;
    [SerializeField] private bool _enableSfx   = true;

    [Range(1f, 5f)] [SerializeField] private float _scrollSpeed = 1f;
    [Range(1f, 5f)] [SerializeField] private float _aiSpeed     = 1f;

    private Tween _tween;

    private const string MusicVolume = "MusicVolume";
    private const string SfxVolume   = "SFXVolume";

    #region Properties

    public bool EnableMusic
    {
        get => _enableMusic;
        set
        {
            _enableMusic = value;
            UpdateAudioMixer();
            PlayerPrefs.SetInt(SaveName.Music, _enableMusic ? 1 : 0);
            SaveData();
        }
    }

    public bool EnableSfx
    {
        get => _enableSfx;
        set
        {
            _enableSfx = value;
            UpdateAudioMixer();
            PlayerPrefs.SetInt(SaveName.SFX, _enableSfx ? 1 : 0);
            SaveData();
        }
    }

    public float ScrollSpeed
    {
        get => _scrollSpeed;
        set
        {
            _scrollSpeed = value;
            PlayerPrefs.SetFloat(SaveName.ScrollSpeed, _scrollSpeed);
            SaveDataDelayed();
        }
    }

    public float AISpeed
    {
        get => _aiSpeed;
        set
        {
            _aiSpeed = value;
            PlayerPrefs.SetFloat(SaveName.AISpeed, _aiSpeed);
            SaveDataDelayed();
        }
    }

    #endregion

    private void Awake() => LoadData();
    private void OnEnable() => UpdateAudioMixer();

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(SaveName.Music)) _enableMusic       = PlayerPrefs.GetInt(SaveName.Music) == 1;
        if (PlayerPrefs.HasKey(SaveName.SFX)) _enableSfx           = PlayerPrefs.GetInt(SaveName.SFX)   == 1;
        if (PlayerPrefs.HasKey(SaveName.ScrollSpeed)) _scrollSpeed = PlayerPrefs.GetFloat(SaveName.ScrollSpeed);
        if (PlayerPrefs.HasKey(SaveName.AISpeed)) _aiSpeed         = PlayerPrefs.GetFloat(SaveName.AISpeed);
    }

    private void SaveData() => PlayerPrefs.Save();

    private void SaveDataDelayed()
    {
        if (_tween != null) _tween.Kill();
        _tween = DOVirtual.DelayedCall(.25f, () => PlayerPrefs.Save());
    }

    private void UpdateAudioMixer()
    {
        _mixer.SetFloat(MusicVolume, _enableMusic ? Mathf.Log10(1f) * 20f : Mathf.Log10(0.0001f) * 20f);
        _mixer.SetFloat(SfxVolume,   _enableSfx ? Mathf.Log10(1f)   * 20f : Mathf.Log10(0.0001f) * 20f);
    }
}
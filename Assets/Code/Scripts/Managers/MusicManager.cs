using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager>
{
    private AudioSource _audioSource;

    [Scene] [SerializeField] private int       _mainMenuScene;
    [SerializeField]         private MusicData _musicData;

    private Tween _tween;

    private int _currMusicIndex;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start() => PlayNewMusic();

    private void OnEnable()
    {
        SceneManager.sceneLoaded               += PlayNewMusicOnSceneLoad;
        LevelManager.OnAnyInitLevelLoading     += FadeOutMusic;
        GameOverSoundEvent.OnAnyPlayGameEndSFX += FadeOutMusicFast;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded               -= PlayNewMusicOnSceneLoad;
        LevelManager.OnAnyInitLevelLoading     -= FadeOutMusic;
        GameOverSoundEvent.OnAnyPlayGameEndSFX -= FadeOutMusicFast;
    }

    private void PlayNewMusicOnSceneLoad(Scene scene, LoadSceneMode mode) => PlayNewMusic();
    private void FadeOutMusic()     => _musicData.FadeOutMusicVolume();
    private void FadeOutMusicFast() => _musicData.FadeOutMusicVolumeFast();

    private void PlayNewMusic()
    {
        KillTween();
        _currMusicIndex = 0;
        if (SceneManager.GetActiveScene().buildIndex == _mainMenuScene)
            PlayMainMenuThemeMusic(_musicData.MenuClip);
        else
            PlayNewGameMusic(_musicData.GameClipsList);
        _musicData.FadeInMusicVolume();
    }

    private void PlayMainMenuThemeMusic(AudioClip musicClip)
    {
        _audioSource.Stop();
        _audioSource.clip = musicClip;
        _audioSource.Play();
        _audioSource.loop = true;
    }

    private void PlayNewGameMusic(List<AudioClip> gameClipList)
    {
        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.clip = gameClipList[_currMusicIndex];
        _audioSource.Play();
        _currMusicIndex++;
        if (_currMusicIndex >= gameClipList.Count) _currMusicIndex = 0;

        float audioClipLength = _audioSource.clip.length;
        KillTween();
        _tween = DOVirtual.DelayedCall(audioClipLength, () => PlayNewGameMusic(gameClipList));
    }

    private void KillTween()
    {
        if (_tween != null)
            _tween.Kill();
    }
}
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "MusicData", menuName = "MusicData/MusicData", order = 0)]
public class MusicData : ScriptableObject
{
    [SerializeField] private AudioClip       _menuClip;
    [SerializeField] private List<AudioClip> _gameClipList;
    [SerializeField] private AudioMixer      _mixer;
    [SerializeField] private float           _musicFadeInDuration = 1f;
    [SerializeField] private float           _musicFadeOutDuration = 3f;
    [SerializeField] private float           _musicFadeOutShortDuration = 1f;

    private const string FadeVolume = "FadingMusicVolume";

    private Tween _tween;

    public AudioClip MenuClip => _menuClip;

    public List<AudioClip> GameClipsList => ShuffleMusicList(_gameClipList);

    public void FadeInMusicVolume()
    {
        KillTween();
        _tween = _mixer.DOSetFloat(FadeVolume, 0f, _musicFadeInDuration);
    }

    public void FadeOutMusicVolume()
    {
        KillTween();
        _tween = _mixer.DOSetFloat(FadeVolume, -80f, _musicFadeOutDuration);
    }
    
    public void FadeOutMusicVolumeFast()
    {
        KillTween();
        _tween = _mixer.DOSetFloat(FadeVolume, -80f, _musicFadeOutShortDuration);
    }

    private List<AudioClip> ShuffleMusicList(List<AudioClip> gameClipList)
    {
        System.Random random = new System.Random();
        gameClipList = gameClipList.OrderBy(x => random.Next()).ToList();
        return gameClipList;
    }

    private void KillTween()
    {
        if (_tween != null)
            _tween.Kill();
    }
}
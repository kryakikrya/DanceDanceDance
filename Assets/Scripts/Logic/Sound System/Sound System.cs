using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class SoundSystem : IDisposable
{
    private const float SoundVolumeRecoveryDuration = 0.1f;
    
    private readonly CMS _cms;
    private readonly List<AudioSource> _audioSources;

    private readonly AudioSource _musicA;
    private readonly AudioSource _musicB;

    private AudioSource _activeMusicSource;
    private AudioSource _inactiveMusicSource;

    private AudioClip _currentMusic;

    private CancellationTokenSource _musicCts;

    private const float FadeDuration = 2f;
    
    private float _soundVolumeRecoveryStartTime;
    private bool _isSoundVolumeRecovering;

    public SoundSystem(List<AudioSource> audioSources, CMS cms)
    {
        _cms = cms;
        _audioSources = audioSources;

        _musicA = _audioSources[0];
        _musicB = _audioSources[1];

        _musicA.loop = false;
        _musicB.loop = false;

        _musicA.volume = 1f;
        _musicB.volume = 0f;

        _activeMusicSource = _musicA;
        _inactiveMusicSource = _musicB;

        _musicCts = new CancellationTokenSource();

        for (int i = 2; i < _audioSources.Count; i++)
        {
            if (_audioSources[i] == null)
            {
                Debug.LogAssertion($"AudioSource[{i}] is null");
                continue;
            }

            _audioSources[i].playOnAwake = false;
        }
    }

    public void Dispose()
    {
        _musicCts.Cancel();
        _musicCts.Dispose();
        _musicCts = null;
    }

    public async UniTask PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Music clip == null");
            return;
        }

        _currentMusic = clip;

        CancellationToken token = _musicCts.Token;

        _inactiveMusicSource.clip = clip;
        _inactiveMusicSource.volume = 0f;
        _inactiveMusicSource.Play();

        float t = 0f;

        while (t < FadeDuration)
        {
            if (token.IsCancellationRequested)
            {
                _activeMusicSource.volume = 1f;
                _inactiveMusicSource.volume = 0f;
                return;
            }

            t += Time.deltaTime;

            float k = t / FadeDuration;

            if (k > 1f)
            {
                k = 1f;
            }

            _activeMusicSource.volume = 1f - k;
            _inactiveMusicSource.volume = k;

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        _activeMusicSource.Stop();

        AudioSource temp = _activeMusicSource;
        _activeMusicSource = _inactiveMusicSource;
        _inactiveMusicSource = temp;
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip == null");
            return;
        }

        float volume = GetSoundVolume();
        PlayOneShot(clip, volume);

        _isSoundVolumeRecovering = true;
        _soundVolumeRecoveryStartTime = Time.unscaledTime;
    }

    private float GetSoundVolume()
    {
        if (_isSoundVolumeRecovering == false)
        {
            return 1f;
        }

        float elapsedTime = Time.unscaledTime - _soundVolumeRecoveryStartTime;
        float volume = Mathf.Clamp01(elapsedTime / SoundVolumeRecoveryDuration);

        if (volume >= 1f)
        {
            _isSoundVolumeRecovering = false;
            return 1f;
        }

        return volume;
    }

    public void StopMusic()
    {
        _activeMusicSource.Pause();
        _inactiveMusicSource.Pause();
    }

    public void ResumeMusic()
    {
        _activeMusicSource.UnPause();
        _inactiveMusicSource.UnPause();
    }

    public void PlaySound(string clipKey)
    {
        AudioClip clip = _cms.GetEntity<AudioClip>(clipKey, isNullValid: true);
        
        if (clip is null)
        {
            return;
        }

        PlaySound(clip);
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            Debug.Log("StopAllSounds " + _audioSources[i].name);
            if (_audioSources[i] == null)
            {
                continue;
            }

            _audioSources[i].Stop();
        }
    }

    private void PlayOneShot(AudioClip clip, float volume)
    {
        AudioSource source = GetFreeSource();

        float pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        float targetVolume = volume * UnityEngine.Random.Range(0.95f, 1.05f);

        source.pitch = pitch;
        source.volume = 0f;
        source.clip = clip;
        source.Play();

        float duration = clip.length / pitch;
        float fadeDuration = duration * 0.05f;

        DOTween.To(
            () => source.volume,
            value => source.volume = value,
            targetVolume,
            fadeDuration
        );

        DOVirtual.DelayedCall(duration - fadeDuration, () =>
        {
            DOTween.To(
                () => source.volume,
                value => source.volume = value,
                0f,
                fadeDuration
            ).OnComplete(() =>
            {
                source.Stop();
                source.volume = 1f;
            });
        });

        source.pitch = 1f;
    }

    private AudioSource GetFreeSource()
    {
        for (int i = 2; i < _audioSources.Count; i++)
        {
            if (_audioSources[i] != null && _audioSources[i].isPlaying == false)
            {
                return _audioSources[i];
            }
        }

        return _audioSources[2];
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterEmotionController : BodyPart
{
    [SerializeField] private float _changeEmotionColldown = 2f;
    
    [SerializeField] private float _autoChangeFace = 3f;
    
    [SerializeField] private float _faceBack = 1f;
    
    [SerializeField] private float _hpPercentForFoolishFace = 50;
    
    [SerializeField] private Dictionary<DanceState, string> _emotionSounds = new Dictionary<DanceState, string>();

    private float _timer = 0;
    
    private SoundSystem _soundSystem;
    
    private HealthSystem _healthSystem;

    [Inject]
    private void Construct(SoundSystem soundSystem, HealthSystem healthSystem)
    {
        _soundSystem  = soundSystem;
        
        _healthSystem = healthSystem;
    }

    private void Start()
    {
        _timer = _changeEmotionColldown;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _autoChangeFace)
        {
            ChooseDefaultEmotion();
        }
    }

    public void PlayEmotion(CheckResultType result)
    {
        switch (result)
        {
            case CheckResultType.PerfectTime:
                PlayEmotion(DanceState.PerfectFace, true);
                break;
            case CheckResultType.NiceTime:
                PlayEmotion(DanceState.NiceFace, true);
                break;
            case CheckResultType.GoodTime:
                PlayEmotion(DanceState.GoodFace, true);
                break;
            case CheckResultType.BadTime:
                PlayEmotion(DanceState.BadFace, true);
                break;
            case CheckResultType.WrongCombination:
                PlayEmotion(DanceState.AngryFace, true);
                break;
            case CheckResultType.TooEarly:
                PlayEmotion(DanceState.AngryFace, true);
                break;
            case CheckResultType.TooLate:
                PlayEmotion(DanceState.AngryFace, true);
                break;
        }
    }

    private void ChooseDefaultEmotion(bool force = false)
    {
        Debug.Log("Choose Default Emotion");
        
        if (_healthSystem.CurrentHealth / _healthSystem.MaxHealth < _hpPercentForFoolishFace / 100)
        {
            if (DanceState.FoolishFace != _currentState)
            {
                PlayEmotion(DanceState.FoolishFace, force);
            }
        }
        else
        {
            if (DanceState.FoolishFace != _currentState)
            {
                PlayEmotion(DanceState.DefaultFace, force);
            }
        }
    }

    public void PlayEmotion(DanceState state, bool force = false)
    {
        if (_timer >= _changeEmotionColldown || force)
        {
            Debug.Log($"Пытаюсь поменять на {state}");
            ChangeState(state);
            _timer = 0;
            _soundSystem.PlaySound(_emotionSounds[state]);
            
            StartCoroutine(FaceBack(_faceBack, force));
        }
    }
    
    private System.Collections.IEnumerator FaceBack(float delay, bool force = false)
    {
        yield return new WaitForSeconds(delay);
        
        _timer = 0;

        ChooseDefaultEmotion(force);
    }
}
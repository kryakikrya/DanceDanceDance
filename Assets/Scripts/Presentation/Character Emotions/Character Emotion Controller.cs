using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterEmotionController : BodyPart
{
    [SerializeField] private float _autoChangeFace = 3f;
    
    [SerializeField] private float _faceBack = 1f;
    
    [SerializeField] private float _hpPercentForFoolishFace = 50;
    
    [SerializeField] private Dictionary<DanceState, string> _emotionSounds = new Dictionary<DanceState, string>();
    
    private SoundSystem _soundSystem;
    
    private HealthSystem _healthSystem;

    [Inject]
    private void Construct(SoundSystem soundSystem, HealthSystem healthSystem)
    {
        _soundSystem  = soundSystem;
        
        _healthSystem = healthSystem;
    }

    public void PlayEmotion(CheckResultType result)
    {
        switch (result)
        {
            case CheckResultType.PerfectTime:
                PlayEmotion(DanceState.PerfectFace);
                break;
            case CheckResultType.NiceTime:
                PlayEmotion(DanceState.NiceFace);
                break;
            case CheckResultType.GoodTime:
                PlayEmotion(DanceState.GoodFace);
                break;
            case CheckResultType.BadTime:
                PlayEmotion(DanceState.BadFace);
                break;
            case CheckResultType.WrongCombination:
                PlayEmotion(DanceState.AngryFace);
                break;
            case CheckResultType.TooEarly:
                PlayEmotion(DanceState.AngryFace);
                break;
            case CheckResultType.TooLate:
                PlayEmotion(DanceState.AngryFace);
                break;
        }
    }

    private void ChooseDefaultEmotion(bool force = false)
    {
        if (_healthSystem.CurrentHealth / _healthSystem.MaxHealth < _hpPercentForFoolishFace / 100)
        {
            if (DanceState.FoolishFace != _currentState)
            {
                PlayEmotion(DanceState.FoolishFace);
            }
        }
        else
        {
            if (DanceState.FoolishFace != _currentState)
            {
                PlayEmotion(DanceState.DefaultFace);
            }
        }
    }

    public void PlayEmotion(DanceState state)
    {
            ChangeState(state);
            _soundSystem.PlaySound(_emotionSounds[state]);
            
            StartCoroutine(FaceBack(_faceBack));
    }
    
    private System.Collections.IEnumerator FaceBack(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        ChooseDefaultEmotion();
    }
}
using System;
using System.Globalization;
using UnityEngine;
using Zenject;

public class Win : MonoBehaviour, IPrefLoader, IPrefSaver
{
    [SerializeField] private GameObject _winPanel;

    private ScoreSystem _scoreSystem;
    
    private HealthSystem _healthSystem;
    
    private SongClock _songClock;

    public string PrefKey => "TotalScore";
    
    private bool _won = false;

    [Inject]
    private void Construct(ScoreSystem scoreSystem, HealthSystem healthSystem, SongClock songClock)
    {
        _scoreSystem = scoreSystem;
        
        _healthSystem = healthSystem;
        
        _songClock = songClock;
    }

    private void OnEnable()
    {
        _winPanel.SetActive(false);
        
        _won = false;

        _songClock.OnSongFinished += WinGame;
    }

    private void OnDisable()
    {
        _winPanel.SetActive(false);
        
        _songClock.OnSongFinished -= WinGame;
    }

    public void WinGame()
    {
        if (!_won)
        {
            _won =  true;
            _healthSystem.ResetHealth();
        
            float totalScore = PlayerPrefs.GetFloat(PrefKey);
            totalScore += _scoreSystem.Score;
        
            Debug.Log($"Updating total score to {totalScore} pref = {PrefKey}");

            PlayerPrefs.SetFloat(PrefKey, totalScore);
            PlayerPrefs.Save();

            _winPanel.SetActive(true);
        }
    }

    public string GetSaveData()
    {
        return PlayerPrefs.GetFloat(PrefKey).ToString(CultureInfo.InvariantCulture);
    }

    public void LoadData(string data)
    {
        float totalScore = float.Parse(data, CultureInfo.InvariantCulture);

        PlayerPrefs.SetFloat(PrefKey, totalScore);
        PlayerPrefs.Save();
    }
}

using System.Globalization;
using UnityEngine;
using Zenject;

public class Win : MonoBehaviour, IPrefLoader, IPrefSaver
{
    [SerializeField] private GameObject _winPanel;

    private ScoreSystem _scoreSystem;

    public string PrefKey => "TotalScore";

    [Inject]
    private void Construct(ScoreSystem scoreSystem)
    {
        _scoreSystem = scoreSystem;
    }

    private void OnEnable()
    {
        _winPanel.SetActive(false);
    }

    public void WinGame()
    {
        float totalScore = PlayerPrefs.GetFloat(PrefKey);
        totalScore += _scoreSystem.Score;

        PlayerPrefs.SetFloat(PrefKey, totalScore);
        PlayerPrefs.Save();

        _winPanel.SetActive(true);
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

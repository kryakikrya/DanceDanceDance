using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongSO", menuName = "Scriptable Objects/SongSO")]
public class SongSO : ScriptableObject
{
    [SerializeField, Min(1)] private int _bpm;
    public int BPM => _bpm;

    [SerializeField] private float _duration;
    public float Duration => _duration;

    [SerializeField] private int[] _beats;
    public int[] Beats => _beats;
    
    [SerializeField, Min(1)] private int _autoIntervalMain = 4;
    [SerializeField, Min(1)] private int _autoIntervalSub = 2;

    public void GenerateBeats()
    {
        if (_bpm <= 0 || _autoIntervalMain <= 0 || _autoIntervalSub <= 0)
        {
            Debug.LogError("BPM and intervals must be greater than zero.", this);
            return;
        }

        int beatCount = Mathf.CeilToInt(_duration * _bpm / 60f);
        List<int> beats = new List<int>();

        int beat = Mathf.CeilToInt(6f * _bpm / 60f);

        while (beat < beatCount)
        {
            beats.Add(beat);

            int random = Random.Range(0, 101);
            beat += random < 10 ? _autoIntervalSub : _autoIntervalMain;
        }

        _beats = beats.ToArray();
    }
}
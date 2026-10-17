using UnityEngine;

[CreateAssetMenu(fileName = "SongSO", menuName = "Scriptable Objects/SongSO")]
public class SongSO : ScriptableObject
{
    [SerializeField] private int _bpm;
    public int BPM => _bpm;
    
    [SerializeField] private float _duration;
    public float Duration => _duration;

    [SerializeField] private int[] _beats;
    public int[] Beats => _beats;
}
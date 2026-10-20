using System;
using UnityEngine;

public class SongClock : MonoBehaviour
{
    private bool _isPlaying = false;
    
    public bool IsPlaying => _isPlaying;

    private float _songTimer = 0;
    
    public float SongTime => _songTimer;
    
    private SongSO _song;
    public SongSO Song => _song;
    
    public event Action OnSongFinished;

    private void Update()
    {
        if (_isPlaying)
        {
            _songTimer += Time.deltaTime;
            //Debug.Log(_songTimer);
            
            if (HasReached(_song.Duration))
            {
                OnSongFinished?.Invoke();
            }
        }
    }

    private void OnEnable()
    {
        _songTimer = 0;
    }

    public void SetIsPlaying(bool isPlaying, SongSO SO = null)
    {
        Debug.Log($"Is playing is {isPlaying}");

        if (SO != null)
        {
            _song = SO;   
        }
        
        _isPlaying = isPlaying;
    }
    
    public bool HasReached(float targetTime)
    {
        return SongTime >= targetTime;
    }
}

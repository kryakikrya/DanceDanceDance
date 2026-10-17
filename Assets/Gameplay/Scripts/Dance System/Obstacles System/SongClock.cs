using UnityEngine;

public class SongClock : MonoBehaviour
{
    private bool _isPlaying = false;
    
    public bool IsPlaying => _isPlaying;

    private float _songTimer = 0;
    
    public float SongTime => _songTimer;

    private void Update()
    {
        if (_isPlaying)
        {
            _songTimer += Time.deltaTime;
        }
    }

    public void SetIsPlaying(bool isPlaying)
    {
        Debug.Log($"Is playing is {isPlaying}");
        
        _isPlaying = isPlaying;
    }
    
    public bool HasReached(float targetTime)
    {
        return SongTime >= targetTime;
    }
}

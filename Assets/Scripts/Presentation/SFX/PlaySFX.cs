using UnityEngine;
using Zenject;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] private string _path;
    
    private SoundSystem _soundSystem;

    [Inject]
    private void Construct(SoundSystem soundSystem)
    {
        _soundSystem = soundSystem;
    }

    public void Play()
    {
        _soundSystem.PlaySound(_path);
    }
}

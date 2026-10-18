using UnityEngine;
using Zenject;

public class CDDisk : MonoBehaviour
{
    [SerializeField] private SongSO _song;
    [SerializeField] private float _rotationSpeed = 1f;

    [Inject] private SongClock _clock;

    private void Start()
    {
        float rotationSpeed = _rotationSpeed * _song.BPM;

        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        enabled = true;
    }

    private void Update()
    {
        if (_clock.IsPlaying)
        {
            transform.Rotate(Vector3.forward, _rotationSpeed * _song.BPM * Time.deltaTime);   
        }
    }
}
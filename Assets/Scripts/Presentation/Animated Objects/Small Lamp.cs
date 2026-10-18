using UnityEngine;
using Random = UnityEngine.Random;

public class SmallLamp : MonoBehaviour
{
    [SerializeField] private float _from;
    [SerializeField] private float _to;

    [SerializeField] private float _offTime;

    [SerializeField] private SpriteRenderer _renderer;

    private float _timer;
    private float _time;
    private bool _isOff;

    private void Start()
    {
        _time = Random.Range(_from, _to);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (!_isOff && _timer >= _time)
        {
            _timer = 0f;
            _isOff = true;
            _renderer.enabled = false;
            _time = _offTime;
        }
        else if (_isOff && _timer >= _time)
        {
            _timer = 0f;
            _isOff = false;
            _renderer.enabled = true;
            _time = Random.Range(_from, _to);
        }
    }
}
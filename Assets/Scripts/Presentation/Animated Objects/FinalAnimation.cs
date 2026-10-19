using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

public class FinalAnimation : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToDisable = new List<GameObject>();

    [SerializeField] private GameObject _darkCheriku;
    [SerializeField] private GameObject _hand;
    [SerializeField] private GameObject _finalComic;

    [SerializeField] private Light2D _light2d;

    [SerializeField] private GameObject _dark;
    [SerializeField] private GameObject _darkReal;
    [SerializeField] private GameObject _light;

    [SerializeField] private Sprite _scaryFace;
    [SerializeField] private SpriteRenderer _face;

    [SerializeField] private string _path1;
    [SerializeField] private string _path2;
    [SerializeField] private string _path3;
    [SerializeField] private string _path4;
    [SerializeField] private string _path5;

    [SerializeField] private float _delay1 = 2.5f;
    [SerializeField] private float _delay2 = 2.5f;
    [SerializeField] private float _delay3 = 2.5f;
    [SerializeField] private float _delay4 = 2f;
    [SerializeField] private float _delay5 = 4f;

    private SoundSystem _soundSystem;
    private CancellationTokenSource _cts;
    private Battery _battery;

    [Inject]
    private void Construct(SoundSystem soundSystem, Battery battery)
    {
        _soundSystem = soundSystem;
        _battery = battery;
    }

    private void Start()
    {
        _battery.OnBatteryDead += DoFinal;
    }

    private void DoFinal()
    {
        Final().Forget();
    }

    private void OnDestroy()
    {
        if (_battery != null)
        {
            _battery.OnBatteryDead -= DoFinal;
        }
    }

    public async UniTask Final()
    {
        _soundSystem.StopMusic();
        _soundSystem.StopAllSounds();
        
        _cts?.Cancel();
        _cts?.Dispose();

        _cts = new CancellationTokenSource();
        CancellationTokenSource cts = _cts;
        CancellationToken token = cts.Token;

        try
        {
            foreach (GameObject obj in _objectsToDisable)
            {
                obj.SetActive(false);
            }
            
            _light.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_delay1), cancellationToken: token);

            _light.SetActive(false);
            _dark.SetActive(true);
            _soundSystem.PlaySound(_path1);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_delay1), cancellationToken: token);
            
            _darkCheriku.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(_delay1), cancellationToken: token);

            _soundSystem.PlaySound(_path2);
            _dark.SetActive(false);
            _light.SetActive(true);
            _face.sprite = _scaryFace;

            await UniTask.Delay(TimeSpan.FromSeconds(_delay2), cancellationToken: token);

            _darkCheriku.SetActive(false);
            _hand.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(_delay3), cancellationToken: token);

            _dark.SetActive(true);
            _light.SetActive(false);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_delay4), cancellationToken: token);

            _soundSystem.PlaySound(_path3);
            _light.SetActive(false);
            _darkReal.SetActive(true);
            _hand.SetActive(false);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_delay5), cancellationToken: token);
            
            _soundSystem.PlaySound(_path4);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_delay5), cancellationToken: token);
            
            _soundSystem.PlaySound(_path5);
            _finalComic.SetActive(true);

            _light2d.intensity = 1;
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
        }
        finally
        {
            if (_cts == cts)
            {
                _cts.Dispose();
                _cts = null;
            }
        }
    }

    private void OnDisable()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }
}
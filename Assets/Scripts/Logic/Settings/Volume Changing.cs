using System.Globalization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class VolumeChanging : MonoBehaviour, IPrefLoader, IPrefSaver, IPointerUpHandler
{
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioMixerGroup _mixerGroup;
    [SerializeField] private string _volumeParameter;
    [SerializeField] private AudioClip _sound;
    [SerializeField] private bool _isMusic;

    private SoundSystem _soundSystem;

    public string PrefKey => $"{_volumeParameter}";
    public float Value => _slider.value;

    [Inject]
    private void Construct(SoundSystem soundSystem)
    {
        _soundSystem = soundSystem;
    }

    private void Sync()
    {
        _mixerGroup.audioMixer.GetFloat(_volumeParameter, out float volumeDb);
        float defaultValue = Mathf.Pow(10f, volumeDb / 20f);

        float volume = PlayerPrefs.GetFloat(PrefKey, defaultValue);

        _slider.SetValueWithoutNotify(volume);
        ChangeVolume(volume);

        _slider.onValueChanged.AddListener(ChangeVolume);

        EventTrigger trigger = _slider.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };

        entry.callback.AddListener(_ => PlayPreviewSound());
        trigger.triggers.Add(entry);
    }

    private void ChangeVolume(float value)
    {
        float volume = value > 0f ? Mathf.Log10(value) * 20f : -80f;
        _mixerGroup.audioMixer.SetFloat(_volumeParameter, volume);

        PlayerPrefs.SetFloat(PrefKey, value);
        PlayerPrefs.Save();
    }

    private void PlayPreviewSound()
    {
        if (_isMusic)
        {
            _soundSystem.PlayMusic(_sound);
        }
        else
        {
            _soundSystem.PlaySound(_sound);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isMusic)
        {
            _soundSystem.PlayMusic(_sound);
        }
        else
        {
            _soundSystem.PlaySound(_sound);
        }
    }

    public string GetSaveData()
    {
        Sync();
        
        return PlayerPrefs.GetFloat(PrefKey).ToString(CultureInfo.InvariantCulture);
    }

    public void LoadData(string data)
    {
        float volume = float.Parse(data, CultureInfo.InvariantCulture);

        PlayerPrefs.SetFloat(PrefKey, volume);
        _slider.value = volume;
        ChangeVolume(volume);
        PlayerPrefs.Save();

        Debug.Log($"[VolumeChanging] Loaded binding: {data}", this);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(ChangeVolume);
    }
}
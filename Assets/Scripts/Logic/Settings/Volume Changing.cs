using System.Globalization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeChanging : MonoBehaviour, IPrefLoader, IPrefSaver
{
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioMixerGroup _mixerGroup;
    [SerializeField] private string _volumeParameter;

    public string PrefKey => $"Volume_{_volumeParameter}";
    public float Value => _slider.value;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float value)
    {
        float volume = value > 0f ? Mathf.Log10(value) * 20f : -80f;
        _mixerGroup.audioMixer.SetFloat(_volumeParameter, volume);
    }

    public string GetSaveData()
    {
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
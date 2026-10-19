using UnityEngine;

public class SettingsApp : App
{
    [SerializeField] private VolumeChanging[] _volumeChangings;

    public override void Close()
    {
        foreach (VolumeChanging volumeChanging in _volumeChangings)
        {
            PlayerPrefs.SetFloat(volumeChanging.PrefKey, volumeChanging.Value);
        }

        PlayerPrefs.Save();

        base.Close();
    }
}
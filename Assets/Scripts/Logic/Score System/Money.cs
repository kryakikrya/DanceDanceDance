using System;
using System.Globalization;
using UnityEngine;

public class Money : IPrefLoader, IPrefSaver
{
    public string PrefKey => "TotalScore";

    public event Action<int> OnMoneyChanged;

    public int GetMoney()
    {
        return PlayerPrefs.GetInt(PrefKey);
    }

    public void ChangeMoney(int amount)
    {
        PlayerPrefs.SetInt(PrefKey, GetMoney() + amount);
        PlayerPrefs.Save();

        OnMoneyChanged?.Invoke(GetMoney());
    }

    public string GetSaveData()
    {
        return GetMoney().ToString(CultureInfo.InvariantCulture);
    }

    public void LoadData(string data)
    {
        PlayerPrefs.SetInt(PrefKey, int.Parse(data, CultureInfo.InvariantCulture));
        PlayerPrefs.Save();
    }
}
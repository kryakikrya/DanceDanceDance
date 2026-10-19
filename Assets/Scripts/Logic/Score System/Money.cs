using System;
using System.Globalization;
using UnityEngine;

public class Money : IPrefLoader, IPrefSaver
{
    public string PrefKey => "TotalScore";

    public event Action<int> OnMoneyChanged;

    public int GetMoney()
    {
        return Mathf.RoundToInt(PlayerPrefs.GetFloat(PrefKey, 0));
    }

    public void ChangeMoney(int amount)
    {
        int current = GetMoney() + amount;
        LoadData(current.ToString(CultureInfo.InvariantCulture));

        OnMoneyChanged?.Invoke(GetMoney());
    }

    public string GetSaveData()
    {
        return GetMoney().ToString(CultureInfo.InvariantCulture);
    }

    public void LoadData(string data)
    {
        PlayerPrefs.SetFloat(PrefKey, float.Parse(data, CultureInfo.InvariantCulture));
        PlayerPrefs.Save();
    }
}
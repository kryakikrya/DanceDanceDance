using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyButton : MonoBehaviour, IPrefLoader, IPrefSaver
{
    [SerializeField] private string _prefKey;
    [SerializeField] private int _price;
    
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _unlockedButton;

    private Money _money;

    public string PrefKey => _prefKey;

    [Inject]
    private void Construct(Money money)
    {
        _money = money;
    }

    private void OnEnable()
    {
        _buyButton.GetComponent<Button>().onClick.AddListener(Buy);
    }

    private void OnDisable()
    {
        _buyButton.GetComponent<Button>().onClick.RemoveListener(Buy);
    }

    public void Buy()
    {
        if (_money.GetMoney() < _price)
        {
            return;
        }

        _money.ChangeMoney(-_price);
        UnlockItem();
        PlayerPrefs.SetInt(PrefKey, 1);
        PlayerPrefs.Save();
    }

    public void UnlockItem()
    {
        _buyButton.SetActive(false);
        _unlockedButton.SetActive(true);
    }

    public string GetSaveData()
    {
        return PlayerPrefs.GetInt(PrefKey, 0) == 1 ? "true" : "false";
    }

    public void LoadData(string data)
    {
        if (bool.Parse(data))
        {
            UnlockItem();
        }
    }
}
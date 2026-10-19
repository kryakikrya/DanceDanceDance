using TMPro;
using UnityEngine;
using Zenject;

public class MoneyBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;

    private Money _money;

    [Inject]
    private void Construct(Money money)
    {
        _money = money;
    }

    private void OnEnable()
    {
        _money.OnMoneyChanged += UpdateMoney;
        UpdateMoney(_money.GetMoney());
    }

    private void OnDisable()
    {
        _money.OnMoneyChanged -= UpdateMoney;
    }

    private void UpdateMoney(int amount)
    {
        _moneyText.text = amount.ToString();
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CoinsHUDPresenter : MonoBehaviour
{
    [SerializeField] private CoinsAmountData _coinsAmount;

    private void OnEnable()
    {
        UpdateHUDAmount();

        EndLevelEvents.Instance.OnCoinAdded += AddCoin;
    }

    private void OnDisable()
    {
        if (EndLevelEvents.Instance != null)
        {
            EndLevelEvents.Instance.OnCoinAdded -= AddCoin;
        }
    }

    public void AddCoin()
    {
        _coinsAmount.CoinsAmount++;

        UpdateHUDAmount();
    }

    private void UpdateHUDAmount()
    {
        GetComponent<Text>().text = "" + _coinsAmount.CoinsAmount;
    }
}
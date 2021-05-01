using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelEvents : MonoBehaviour
{
    private static EndLevelEvents _instance;
    public static EndLevelEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EndLevelEvents>();
            }
            
            return _instance = FindObjectOfType<EndLevelEvents>();
        }
    }
    private void Awake()
    {
        _instance = this;
    }

    public event Action OnAllEnemiesKilled;

    private List<CoinMovement> _spawnedCoins = new List<CoinMovement>();

    public event Action OnCoinAdded;

    [SerializeField] private Transform _player;

    public void AddCoin(CoinMovement newCoin)
    {
        _spawnedCoins.Add(newCoin);
    }

    public void Remove(CoinMovement coin)
    {
        _spawnedCoins.Remove(coin);
    }

    public void AllEnemyKilled()
    {
        OnAllEnemiesKilled.Invoke();

        foreach (CoinMovement item in _spawnedCoins)
        {
            item.StartMoveToPlayer(_player, OnCoinAdded);
        }
    }
}

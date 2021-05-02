using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private EnemiesToSpawnAmount EnemiesToSpawnData;
    [SerializeField] private PlayerStats PlayerStats;
    [SerializeField] private PlayerHPDataBetweenScenes PlayerHPContainer;

    [SerializeField] private int EnemiesToSpawnOnStart;

    public void InitStartValues()
    {
        EnemiesToSpawnData.EnemiesToSpawnCount = EnemiesToSpawnData.StartEnemiesToSpawnCountValue;
        PlayerHPContainer.PlayerMaxHP = PlayerStats.GetMaxHealth();
        PlayerHPContainer.PlayerHP = PlayerStats.GetMaxHealth();
    }
}

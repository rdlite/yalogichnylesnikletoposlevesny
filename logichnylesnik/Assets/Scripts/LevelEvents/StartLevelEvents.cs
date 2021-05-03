using System;
using System.Collections;
using UnityEngine;

public class StartLevelEvents : MonoBehaviour
{
    public event Action OnGameStarted;

    private static StartLevelEvents _instance;
    public static StartLevelEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StartLevelEvents>();
            }

            return _instance = FindObjectOfType<StartLevelEvents>();
        }
    }
    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] private InGameUI _inGameUI;

    [SerializeField] private EnemiesToSpawnAmount _enemiesCountData;
    [SerializeField] private PlayerAttackTypeBetweenScenes _playerWeaponBetweenSceneData;

    private void Start()
    {
        if (_enemiesCountData.EnemiesToSpawnCount == _enemiesCountData.StartEnemiesToSpawnCountValue)
        {
            SetPlayerWeponType(0);

            StartCoroutine(StartGameCounter(3));
        }
        else
        {
            StartCoroutine(StartGameCounter(0));
        }
    }

    private IEnumerator StartGameCounter(int timeWaiting)
    {
        _inGameUI.SetActiveInGameHUD(false);

        _inGameUI.StartCounter(timeWaiting);

        yield return new WaitForSeconds(timeWaiting);

        _inGameUI.SetActiveInGameHUD(true);

        CallOnStartGameEvents();
    }

    private void CallOnStartGameEvents()
    {
        OnGameStarted?.Invoke();
    }

    private void SetPlayerWeponType(int id)
    {
        _playerWeaponBetweenSceneData.AttackType = id;
    }
}
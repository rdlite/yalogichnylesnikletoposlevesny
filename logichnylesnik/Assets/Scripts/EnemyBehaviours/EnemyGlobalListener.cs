using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlobalListener : MonoBehaviour
{
    [Range(.1f, 1f)]
    [SerializeField] private float _nearestEnemyFindRefreshingTime = .5f; 

    private List<Enemy> _enemiesOnScene = new List<Enemy>();
    private Transform _player;

    private static EnemyGlobalListener _instance;
    public static EnemyGlobalListener Instance { get => _instance; }
    private void Awake() => _instance = this;

    public GameObject NearestEnemy { get; private set; }

    public void AddEnemy(Enemy enemyToAdd) => _enemiesOnScene.Add(enemyToAdd);

    public void InitPlayer(Transform playerObject) => _player = playerObject;

    private void Start()
    {
        StartLevelEvents.Instance.OnGameStarted += StartCheckingNearestEnemy;
    }

    private void StartCheckingNearestEnemy()
    {
        StartCoroutine(CheckNearestEnemy());
    }

    private IEnumerator CheckNearestEnemy()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(_nearestEnemyFindRefreshingTime);

            if (_enemiesOnScene.Count == 0)
            {
                yield break;
            }

            float minDistance = (_player.position - _enemiesOnScene[0].transform.position).magnitude;
            NearestEnemy = _enemiesOnScene[0].gameObject;

            foreach (Enemy item in _enemiesOnScene)
            {
                float distToEnemy = (_player.position - item.transform.position).magnitude;

                if (distToEnemy < minDistance)
                {
                    minDistance = distToEnemy;
                    NearestEnemy = item.gameObject;
                }

                item.SetNearestIndicatorActive(false);
            }

            NearestEnemy.GetComponent<Enemy>().SetNearestIndicatorActive(true);
        }
    }

    public void OnEnemyDead(Enemy enemy)
    {
        _enemiesOnScene.Remove(enemy);

        if (_enemiesOnScene.Count == 0)
        {
            EndLevelEvents.Instance.AllEnemyKilled();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _FXPrefab;
    [SerializeField] private float _timeToDestroy;

    [SerializeField] private Transform _spawnPoint;

    public void SpawnParticleAndDestroyAfterTime()
    {
        Destroy(Instantiate(_FXPrefab, _spawnPoint.position, Quaternion.identity), _timeToDestroy);
    }
}

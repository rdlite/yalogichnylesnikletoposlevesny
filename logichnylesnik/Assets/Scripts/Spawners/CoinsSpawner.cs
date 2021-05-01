using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CoinPrefab;

    public void SpawnCoins(Vector3 startPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newCoin = Instantiate(CoinPrefab);

            newCoin.transform.position = startPosition;
            newCoin.transform.rotation = Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));

            newCoin.GetComponent<CoinMovement>().AddPower(Vector3.up * 4f + Random.insideUnitSphere * 5f);

            EndLevelEvents.Instance.AddCoin(newCoin.GetComponent<CoinMovement>());
        }
    }
}

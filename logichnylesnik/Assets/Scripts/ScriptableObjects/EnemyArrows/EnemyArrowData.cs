using UnityEngine;

[CreateAssetMenu(fileName = "EnemyArrowData")]
public class EnemyArrowData : ScriptableObject
{
    [SerializeField] private GameObject ArrowPrefab;

    public GameObject InstantiatePrefab()
    {
        return Instantiate(ArrowPrefab);
    }
}

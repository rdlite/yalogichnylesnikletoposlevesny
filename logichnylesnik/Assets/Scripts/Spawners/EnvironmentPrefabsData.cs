using UnityEngine;

public class EnvironmentPrefabsData : MonoBehaviour
{
    [SerializeField] private GameObject BaseCubePrefab;

    [SerializeField] private GameObject BrightGrassPrefab;
    [SerializeField] private GameObject DarkGrassPrefab;
    [SerializeField] private GameObject StoneObstalcePrefab;
    [SerializeField] private GameObject WaterObstalcePrefab;
    [SerializeField] private GameObject DoorObstaclePrefab;

    public GameObject GetBrightGrassPrefab()
    {
        return BrightGrassPrefab;
    }

    public GameObject GetDarkGrassPrefab()
    {
        return DarkGrassPrefab;
    }

    public GameObject GetStoneObstalcePrefab()
    {
        return StoneObstalcePrefab;
    }

    public GameObject GetWaterObstalcePrefab()
    {
        return WaterObstalcePrefab;
    }

    public GameObject GetDoorObstaclePrefab()
    {
        return DoorObstaclePrefab;
    }

    public float GetCubesScale()
    {
        return BaseCubePrefab.transform.localScale.x;
    }
}

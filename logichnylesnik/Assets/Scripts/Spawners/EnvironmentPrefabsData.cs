using UnityEngine;

public class EnvironmentPrefabsData : MonoBehaviour
{
    [SerializeField] private GameObject BaseCubePrefab;

    [SerializeField] private GameObject BrightGrassPrefab;
    [SerializeField] private GameObject DarkGrassPrefab;
    [SerializeField] private GameObject StoneObstalcePrefab;
    [SerializeField] private GameObject WaterObstalcePrefab;

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

    public float GetCubesScale()
    {
        return BaseCubePrefab.transform.localScale.x;
    }
}

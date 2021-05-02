using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCanvas : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;

    public void InitSliderValues(float value, float maxValue)
    {
        _healthSlider.maxValue = maxValue;
        _healthSlider.value = value;
    }

    public void SetSliderValues(float newValue)
    {
        _healthSlider.value = newValue;
    }

    public void FollowBy(Transform targetToFollow)
    {
        StartCoroutine(FollowingByTarget(targetToFollow));
    }

    private IEnumerator FollowingByTarget(Transform target)
    {
        while (gameObject.activeSelf)
        {
            transform.position = target.position;

            yield return null;
        }
    }
}

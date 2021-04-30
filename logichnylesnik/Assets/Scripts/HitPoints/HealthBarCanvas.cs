using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCanvas : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;

    public void InitSliderValues(float currentHealth)
    {
        _healthSlider.maxValue = currentHealth;
        _healthSlider.value = currentHealth;
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

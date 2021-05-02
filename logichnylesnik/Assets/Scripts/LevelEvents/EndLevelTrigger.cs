using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    [SerializeField] private InGameUI _inGameUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerComponentsManager>() != null)
        {
            _inGameUI.StartShowBlackScreen(ReloadLevel);
        }
    }

    private void ReloadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
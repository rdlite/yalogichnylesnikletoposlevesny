using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private CoinsAmountData _coinsContainer;

    [SerializeField] private Image _foreground;

    [SerializeField] private MainMenu _menu;

    private void Start()
    {
        StartCoroutine(HideBlackScreen(null));
    }

    public void StartGame()
    {
        StartCoroutine(ShowBlackScreen(LoadLevel));

        _coinsContainer.CoinsAmount = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator HideBlackScreen(Action funcToLoadAfter)
    {
        _foreground.gameObject.SetActive(true);

        float alpha = 1f;

        while (alpha >= 0f)
        {
            alpha -= Time.deltaTime;
            _foreground.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        _foreground.gameObject.SetActive(false);

        if (funcToLoadAfter != null)
        {
            funcToLoadAfter.Invoke();
        }
    }

    private IEnumerator ShowBlackScreen(Action funcToLoadAfter)
    {
        _foreground.gameObject.SetActive(true);

        float alpha = 0f;

        while (alpha <= 1f)
        {
            alpha += Time.deltaTime;
            _foreground.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        if (funcToLoadAfter != null)
        {
            funcToLoadAfter.Invoke();
        }
    }

    private void LoadLevel()
    {
        _menu.InitStartValues();

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
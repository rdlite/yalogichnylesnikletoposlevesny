using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIButtons : MonoBehaviour
{
    [SerializeField] private GameObject _inGameHUD;
    [SerializeField] private GameObject _pauseMenu;

    [SerializeField] private Image _foreground;

    public void PauseGame()
    {
        Time.timeScale = 0;

        _inGameHUD.SetActive(false);
        _pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        _inGameHUD.SetActive(true);
        _pauseMenu.SetActive(false);
    }

    public void GoToMenu()
    {     
        StartShowBlackScreen(LoadMenu);
    }

    private void Start()
    {
        StartHideBlackScreen(null);
    }

    public void StartShowBlackScreen(Action func)
    {
        _inGameHUD.SetActive(false);
        _pauseMenu.SetActive(false);

        StartCoroutine(ShowBlackScreen(func));
    }

    private void StartHideBlackScreen(Action func)
    {
        StartCoroutine(HideBlackScreen(func));
    }

    private IEnumerator HideBlackScreen(Action funcToLoadAfter)
    {
        _foreground.gameObject.SetActive(true);

        float alpha = 1f;

        while (alpha >= 0f)
        {
            alpha -= Time.unscaledDeltaTime;
            _foreground.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        _foreground.gameObject.SetActive(false);

        if (funcToLoadAfter != null)
        {
            funcToLoadAfter.Invoke();
        }

        _inGameHUD.SetActive(true);
    }

    private IEnumerator ShowBlackScreen(Action funcToLoadAfter)
    {
        _foreground.gameObject.SetActive(true);

        float alpha = 0f;

        while (alpha <= 1f)
        {
            alpha += Time.unscaledDeltaTime;
            _foreground.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        if (funcToLoadAfter != null)
        {
            funcToLoadAfter.Invoke();
        }
    }

    private void LoadMenu()
    {
        Time.timeScale = 1;

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

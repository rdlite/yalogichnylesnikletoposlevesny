using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _inGameHUD;
    [SerializeField] private GameObject _pauseMenu;

    [SerializeField] private Image _foreground;

    [SerializeField] private GameObject _gameCounterPanel, _gameCounterText;
    [SerializeField] private float _startCounterTextScale, _endCounterTextScale;

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

    public void SetActiveInGameHUD(bool value)
    {
        _inGameHUD.SetActive(value);
    }

    public void StartCounter(int startCounterValue)
    {
        StartCoroutine(StartGameCounter(startCounterValue));
    }

    private IEnumerator StartGameCounter(int startCounterValue)
    {
        _gameCounterPanel.SetActive(true);
        _gameCounterText.SetActive(true);

        for (int i = startCounterValue; i > 0; i--)
        {
            float timer = 1f;

            _gameCounterText.GetComponent<Text>().text = i.ToString();

            while (timer > 0f)
            {
                timer -= Time.deltaTime;

                _gameCounterText.transform.localScale = Vector3.one * Mathf.Lerp(_startCounterTextScale, _endCounterTextScale, 1f - timer);

                yield return null;
            }
        }

        _gameCounterPanel.SetActive(false);
        _gameCounterText.SetActive(false);
    }
}

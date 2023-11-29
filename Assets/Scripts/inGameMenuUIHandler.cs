using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class inGameMenuUIHandler : MonoBehaviour
{
    Canvas canvas;
    public GameObject resumeButton;
    public GameObject resetButton;
    public Text heading;

    private void Awake()
    {
        heading.text = "Race over!";
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        GameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            heading.text = "Race paused!";
            Time.timeScale = 0;
            resetButton.SetActive(false);
            resumeButton.SetActive(true);
            canvas.enabled = true;
            Debug.Log("Return key was pressed.");
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        resetButton.SetActive(true);
        resumeButton.SetActive(false);
        heading.text = "Race over!";
        canvas.enabled = false;
    }

    public void OnRaceAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExitToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    IEnumerator ShowMenuCO()
    {
        yield return new WaitForSeconds(1);
        canvas.enabled = true;
    }

    void OnGameStateChanged(GameManager gameManager)
    {
        if (GameManager.instance.GetGameState() == GameStates.raceOver)
        {
            StartCoroutine(ShowMenuCO());
        }
    }

    void OnDestroy()
    {
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
}

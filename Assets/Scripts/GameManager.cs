using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running, raceOver};

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    GameStates gameState = GameStates.countDown;

    float raceStartedTime = 0;
    float raceCompletedTime = 0;

    public event Action<GameManager> OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LevelStart()
    {
        //gameState = GameStates.countDown;
    }

    public GameStates GetGameState()
    {
        return gameState;
    }

    void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;
            OnGameStateChanged?.Invoke(this);
        }
    }

    public float GetRaceTime()
    {
        if (gameState == GameStates.raceOver)
            return raceCompletedTime - raceStartedTime;
        else return Time.time - raceStartedTime;
    }

    public void OnRaceStart()
    {
        raceStartedTime = Time.time;
        gameState = GameStates.running;
    }

    public void OnRaceCompleted()
    {
        raceCompletedTime = Time.time;
        ChangeGameState(GameStates.raceOver);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }
}

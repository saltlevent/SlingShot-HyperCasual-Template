using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region enums

    public enum GamePhase { None, Preparing, Game, Paused, Finished }

    #endregion

    public static GameController instance { private set; get; }

    private GamePhase _GamePhase = GamePhase.Preparing;

    public bool isGamePaused { get; private set; }

    public int currentStageScore { get; private set; }

    public GamePhase gamePhase
    {
        get
        {
            return _GamePhase;
        }
        private set
        {
            _GamePhase = value;
            OnGamePhaseChanged?.Invoke(value);
            Debug.Log(_GamePhase);
        }
    }

    #region events

    public event Action<GamePhase> OnGamePhaseChanged;

    public event Action OnGameRestart;
    public event Action<Stage> OnNewStageOpen;
    public event Action<bool> OnGameFinished;
    public event Action<bool> OnGamePauseStateChange;

    #endregion


    private void Awake()
    {
        instance = this;
    }

    #region Public Methods

    public void StartStage(Stage stage)
    {
        gamePhase = GamePhase.Preparing;
        OnNewStageOpen.Invoke(stage);
    }

    public void setGamePhase(GamePhase phase)
    {
        gamePhase = phase;
    }
    public void SetGamePause(bool pauseEnable)
    {
        if (pauseEnable)
        {
            isGamePaused = true;
            Time.timeScale = 0;
        }
        else
        {
            isGamePaused = false;
            Time.timeScale = 1;
        }
        OnGamePauseStateChange.Invoke(pauseEnable);
    }

    public void RestartGame()
    {
        if (isGamePaused)
            SetGamePause(false);

        gamePhase = GamePhase.Game;

        OnGameRestart.Invoke();
    }

    public void FinishGame(bool success)
    {
        OnGameFinished.Invoke(success);
    }

    public void SetScore(int score)
    {
        currentStageScore = score;
    }

    public void SetStage(int stageNo)
    {
        var stage = Resources.Load<GameObject>($"stage{stageNo}");

        Instantiate(stage);
    }

    #endregion

}

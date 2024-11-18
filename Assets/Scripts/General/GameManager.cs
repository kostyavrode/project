using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    OFF=0,
    PLAYING=1,
    PAUSED=2,
    FINISHED=3
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Action onBallScored;
    public static Action<GameState> onGameStateChanged;
    
    public GameState State { get { return this.state; } }
    public GameState PastState { get { return this.pastState; } }

    //[SerializeField] private UIManager uiManager;
    //[SerializeField] private LevelManager levelManager;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform tempStartPosition;

    private List<IGameListener> listeners = new();
    private GameState state;
    private GameState pastState;
    private GameObject player;
    private int kicksCount;
    private int ballScored;
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        onBallScored += BallScored;
    }
    private void OnDisable()
    {
        onBallScored -= BallScored;
    }
    public void AddListener(IGameListener listener)
    {
        listeners.Add(listener);
    }    
    public void ChangeGameState(GameState state)
    {
        pastState = this.state;
        switch (state)
        {
            case GameState.OFF:
                break;
            case GameState.PLAYING:
                break;
            case GameState.PAUSED:
                break;
            case GameState.FINISHED:
                break;
        }
        onGameStateChanged?.Invoke(state);
    }
        public void StartGame()
    {
        //levelManager.SpawnLevel();
        SpawnPlayer();
        //CameraFollow.instance.SetTarget(player);
        SetPlayerPosition();
        foreach(var listner in listeners)
        {
            if (listner is IGameStartListener startListener)
            {
                startListener.OnGameStarted();
            }
        }
        ChangeGameState(GameState.PLAYING);
    }
    private void EndGame(bool isWin=true)
    {
        player.SetActive(false);
        foreach (var listner in listeners)
        {
            if (listner is IGameFinishedListener finishListener)
            {
                finishListener.OnGameFinished();
            }
        }
    }
    private void PauseGame()
    {
        foreach (var listner in listeners)
        {
            if (listner is IGamePauseListener pauseListener)
            {
                pauseListener.OnGamePaused();
            }
        }
    }
    private void ResumeGame()
    {
        foreach (var listner in listeners)
        {
            if (listner is IGameResumeListener resumeListener)
            {
                resumeListener.OnGameResumed();
            }
        }
    }
    private void SpawnPlayer()
    {
        GameObject newPlayer=Instantiate(playerPrefab);
        player = newPlayer;
    }
    private void SetPlayerPosition()
    {
        player.transform.position = tempStartPosition.position;
    }
    private void BallScored()
    {
        Debug.Log("Ball Scored");
        IncreaseBallScored();
        Debug.Log(ballScored);
        //if (ballScored>=levelManager.GetHolesCount())
        //{
          //  EndGame(true);
        //}
    }
    public void RestartLevel()
    {
        //levelManager.DespawnLevel();
        ballScored = 0;
        kicksCount = 0;
        //levelManager.SpawnLevel();
        BallControl.instance.ResetVelocity();
        SetPlayerPosition();
    }
    public void IncreaseKicksCount()
    {
        //kicksCount+=1;
        //uiManager.ShowKicksCount(kicksCount.ToString());
    }
    public void IncreaseBallScored()
    {
        //ballScored+=1;
        //uiManager.ShowBallsScored(ballScored.ToString());
        //kicksCount = 0;
    }
    public int GetKicksCount()
    {
        return kicksCount;
    }    
    public int GetBallScored()
    {
        return ballScored;
    }
    }
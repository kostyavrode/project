using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameListener 
{ }
public interface IGameStartListener : IGameListener
{
    void OnGameStarted();
}
public interface IGamePauseListener : IGameListener
{
    void OnGamePaused();
}
public interface IGameResumeListener : IGameListener
{
    void OnGameResumed();
}
public interface IGameFinishedListener : IGameListener
{
    void OnGameFinished();
}

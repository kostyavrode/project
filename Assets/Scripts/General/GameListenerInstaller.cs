using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent(typeof(GameManager))]
public class GameListenerInstaller : MonoBehaviour
{
    private void Awake()
    {
        GameManager gameManager = GetComponent<GameManager>();
        IGameListener[] listeners = GetComponentsInChildren<IGameListener>();

        foreach (var listener in listeners)
        {
            gameManager.AddListener(listener);
        }
    }
}

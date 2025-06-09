using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager instance { get; private set; } = null;

    public List<Player> players = new List<Player>();


    // Actions
    public Action<Vector2> analogStickMoved;
    public Action confirmButtonPressed;
    public Action revertButtonPressed;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }
}

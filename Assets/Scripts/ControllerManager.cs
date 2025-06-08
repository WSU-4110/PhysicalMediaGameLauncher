using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager instance { get; private set; } = null;

    public List<Player> players = new List<Player>();


    // Actions
    public UnityEvent<Vector2> analogStickMoved;
    public UnityEvent confirmButtonPressed;
    public UnityEvent revertButtonPressed;

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

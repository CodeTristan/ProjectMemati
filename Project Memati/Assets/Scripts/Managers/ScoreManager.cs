using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Dictionary<Player, int> scores = new Dictionary<Player, int>();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        List<Player> players = PlayerManager.instance.players;
        foreach(Player p in players)
        {
            scores.Add(p, 0);
        }
    }

    public void AddScore(Player player, int amount)
    {
        scores[player] += amount;
    }
}

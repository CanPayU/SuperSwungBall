using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameScene.Solo;
using GameScene.Multi;

using GameScene;

[System.Serializable]
public class Game
{
    private static Game game_instance = new Game();

    private Dictionary<int, Team> teams;
    private bool finished;
    private int max_point = 1;

    public Game()
    {
        finished = false;
        teams = new Dictionary<int, Team>();

        teams[0] = Settings.Instance.Selected_Team;
        teams[1] = Settings.Instance.Random_Team;
    }
    public Game(Team ennemy_t)
    {
        finished = false;
        teams = new Dictionary<int, Team>();

        if (PhotonNetwork.isMasterClient)
        {
            teams[0] = Settings.Instance.Selected_Team;
            teams[1] = ennemy_t;
        }
        else
        {
            teams[0] = ennemy_t;
            teams[1] = Settings.Instance.Selected_Team;
        }

    }

    public void goal(int team_id)
    {
        Team team = teams[team_id];
        team.Points = 1;
        if (team.Points >= max_point)
        {
            finished = true;
            GameKit.GameBehavior.Call.OnEndGame(End.TIME);
        }
    }

    public static Game Instance
    {
        get { return game_instance; }
        set { game_instance = value; }
    }
    public Dictionary<int, Team> Teams
    {
        get { return teams; }
    }
    public bool isFinish
    {
        get { return finished; }
        set { finished = value; }
    }
    public Team MyTeam
    {
        get
        {
            if (!PhotonNetwork.inRoom)
                return teams[0];
            int myID = (PhotonNetwork.isMasterClient) ? 0 : 1;
            return teams[myID];
        }
    }
    public Team EnnemyTeam
    {
        get
        {
            if (!PhotonNetwork.inRoom)
                return teams[1];
            int EnnemyID = (PhotonNetwork.isMasterClient) ? 1 : 0;
            return teams[EnnemyID];
        }
    }
}

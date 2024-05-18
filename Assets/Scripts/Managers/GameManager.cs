using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // to access the methods

    public GameState gameState; // the state of the game

    public void setGameState(GameState state) // change the state of the game
    {
        gameState = state;    
        switch (state)
        {
            case GameState.Start:
                BattleManager.Instance.battleStart();
                break;
            case GameState.Battle:
                BattleManager.Instance.advanceTurn(0);     
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Busy:
                break;
            case GameState.Won:
                break;
            case GameState.Lost:
                break;
            default:
                break;
        }
    }

    private void Awake() // initialize the instance
    {
        Instance = this;
    }

    void Start() 
    {
        setGameState(GameState.Start); // battle is the first state for some reason
    }

}
public enum GameState // gamestates
{
    Start,
    Battle,
    PlayerTurn,
    EnemyTurn,
    Busy,
    Won,
    Lost,
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public TextMeshProUGUI scoreText;
    private int _currentScore = 0;
    private int _currentTiles = 100;
    public TextMeshProUGUI tilesText;

    public State state = State.Game;

    public enum State
    {
        Game,
        End
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        IncreaseScore(0);
        DecreaseTiles(0);
    }

    public void IncreaseScore(int addValue)
    {
        _currentScore += addValue;
        scoreText.text = "Score: " + _currentScore;
    }
    
    public void DecreaseTiles(int decreaseValue)
    {
        _currentTiles -= decreaseValue;
        if (_currentTiles <= 0)
        {
            _currentTiles = 0;
            state = State.End;
        }
        tilesText.text = _currentTiles.ToString();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

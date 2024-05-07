using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public float gameSpeed = 0.8f;
    private int _score = 0;
    public delegate void OnSocreChangeDelegate(int score);
    public event OnSocreChangeDelegate OnScoreChangeEvent;
    public delegate void IsGameOverDelegate(bool isGameOver);
    public event IsGameOverDelegate OnGameOverEvent;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChangeEvent?.Invoke(_score);
        }
    }

    public void SetGameOver(bool isGameOver)
    {
        OnGameOverEvent?.Invoke(isGameOver);
    }

    public void ResetGame()
    {
        Score = 0;
        foreach (var block in GameObject.FindGameObjectsWithTag("Block"))
        {
            DestroyImmediate(block);
        }
        GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>().Reset();
        OnGameOverEvent?.Invoke(false);
    }
 
    public static GameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            GameObject go = new GameObject("GameManager");
            go.AddComponent<GameManager>();

            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }
    
}
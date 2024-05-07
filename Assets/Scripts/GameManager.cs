using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public float gameSpeed = 0.8f;
    private int score = 0;
    public delegate void onSocreChangeDelegate(int score);
    public event onSocreChangeDelegate OnScoreChangeEvent;
    public delegate void isGameOverDelegate(bool isGameOver);
    public event isGameOverDelegate OnGameOverEvent;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            OnScoreChangeEvent?.Invoke(score);
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
            if(_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }
 
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }
    
}
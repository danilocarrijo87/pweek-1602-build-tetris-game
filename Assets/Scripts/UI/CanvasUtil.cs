using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUtil : MonoBehaviour
{
    public Transform GameOverUI;
    public Transform PlayAgainButton;

    private void Start()
    {
        GameManager.Instance.OnGameOverEvent += OnGameOverEvent;
    }

    public void OnGameOverEvent(bool isGameOver)
    {
        GameOverUI.gameObject.SetActive(isGameOver);
        PlayAgainButton.gameObject.SetActive(isGameOver);
    }
    
    // Start is called before the first frame update
    public void PlayAgain()
    {
        GameOverUI.gameObject.SetActive(false);
        PlayAgainButton.gameObject.SetActive(false);
        GameManager.Instance.ResetGame();
    }
}

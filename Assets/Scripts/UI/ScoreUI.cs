using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        _textMeshProUGUI = GetComponent<TMPro.TextMeshProUGUI>();
        GameManager.Instance.OnScoreChangeEvent += OnScoreChangeEvent;
        GameManager.Instance.OnGameOverEvent += OnGameOverEvent;
    }

    private void OnGameOverEvent(bool isGameOver)
    {
        if (!isGameOver)
        {
            _textMeshProUGUI.text = "0";
        }
    }

    private void OnScoreChangeEvent(int i)
    {
        _textMeshProUGUI.text = i.ToString();
    }
}

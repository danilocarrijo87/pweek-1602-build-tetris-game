using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    public Transform textFlag;
    public GameObject fireWorks;

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
        int intScore = Int32.Parse(_textMeshProUGUI.text);
        if (intScore % 1000 == 0 && intScore!= 0)
        { 
            StartCoroutine(NextLevelMessage());
        }
        
    }
    private IEnumerator NextLevelMessage()
    {
        textFlag.gameObject.SetActive(true);
        fireWorks.SetActive(true);
        yield return new WaitForSeconds(3f);
        textFlag.gameObject.SetActive(false);
        fireWorks.SetActive(false);
    }

}

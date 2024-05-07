using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private int _totalTime = 0;
    private bool _isStopped = false;

    private void Awake (){ 
        _text = GetComponent<TextMeshProUGUI>(); 
    }

    private void Start ()
    {
        StartCoroutine(Wait());
        GameManager.Instance.OnGameOverEvent += OnGameOverEvent;
    }

    private void OnGameOverEvent(bool isGameOver)
    {
        if (isGameOver)
        {
            _isStopped = true;
        }
        else
        {
            _totalTime = 0;
            _text.text = "00:00:00";
            _isStopped = false;
            StartCoroutine(Wait());
        }
    }

    private string LeadingZero (int n){
        return n.ToString().PadLeft(2, '0');
    }
    
    IEnumerator Wait()
    {
        while (!_isStopped)
        {
            yield return new WaitForSeconds(1);
            if (_isStopped) yield break;
            _totalTime++;
            var time = TimeSpan.FromSeconds( _totalTime );
            var hour = LeadingZero( time.Hours );
            var minute = LeadingZero( time.Minutes );
            var second = LeadingZero( time.Seconds );
            _text.text = hour + ":" + minute + ":" + second; 
        }
    } 
}

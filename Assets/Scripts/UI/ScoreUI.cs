using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnScoreChangeEvent += OnScoreChangeEvent;
    }

    private void OnScoreChangeEvent(int i)
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = i.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

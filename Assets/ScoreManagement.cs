using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManagement : MonoBehaviour
{
    public TextMeshProUGUI scoreUI;
    
    private int score;
    private Dictionary<int, int> LinePoints;
    
    // Start is called before the first frame update
    void Start()
    {
        LinePoints = new();
        
        score = 0;
        LinePoints[1] = 40;
        LinePoints[2] = 100;
        LinePoints[3] = 300;
        LinePoints[4] = 1200;
    }

    [ContextMenu("Increase Score")]
    public void AddScore(int lines)
    {
        score += GetPointsForLines(lines);
        scoreUI.text = score.ToString();
    }

    private int GetPointsForLines(int lines)
    {
        return LinePoints.ContainsKey(lines) ? LinePoints[lines] : 0;
    }
}
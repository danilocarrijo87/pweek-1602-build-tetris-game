using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAgainButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayAgain()
    {
        GameManager.Instance.ResetGame();
    }
}

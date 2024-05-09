using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SpawnPieceScript : MonoBehaviour
{
    public GameObject[] Pieces;

    private bool isGameAlive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        NewPiece();
    }

    public void NewPiece()
    {
        if (!isGameAlive) return;
        
        var spawnPosition = new Vector3(5f, 24, 5f);
        Instantiate(Pieces[Random.Range(0, Pieces.Length)], transform.position, Quaternion.identity);
    }

    public void GameOver()
    {
        this.enabled = false;
        isGameAlive = false;
    }
}

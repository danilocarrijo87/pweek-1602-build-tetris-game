using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SpawnPieceScript : MonoBehaviour
{
    public GameObject[] Pieces;
    public GameObject Grid;
    public GameObject NextUI;

    private bool isGameAlive = true;
    private int nextPiece = -1;
    private GameObject nextBlock;
    
    // Start is called before the first frame update
    void Start()
    {
        NewPiece();
    }

    public void NewPiece()
    {
        if (!isGameAlive) return;

        if (nextPiece == -1) nextPiece = Random.Range(0, Pieces.Length);
        
        // For playable piece
        Destroy(nextBlock);
        var playableSpawn = GetPlayableSpawn();
        Instantiate(Pieces[nextPiece], playableSpawn, Quaternion.identity);
        
        // For next piece
        nextPiece = Random.Range(0, Pieces.Length);
        nextBlock = Instantiate(Pieces[nextPiece], NextUI.transform.position, Quaternion.identity);
    }

    private Vector3 GetPlayableSpawn()
    {
        var x = Mathf.RoundToInt(Grid.transform.position.x);
        var z = Mathf.RoundToInt(Grid.transform.position.z);
        var y = (Mathf.RoundToInt(Grid.transform.position.y) * 2) + (Vector3.up.y * 5);

        return new Vector3(x, y, z);
    }

    public void GameOver()
    {
        this.enabled = false;
        isGameAlive = false;
    }
}

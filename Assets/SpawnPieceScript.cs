using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPieceScript : MonoBehaviour
{
    public GameObject[] Pieces;
    
    // Start is called before the first frame update
    void Start()
    {
        NewPiece();
    }

    public void NewPiece()
    {
        //var spawnPosition = new Vector3(5, 18, 5);
        var spawnPosition = new Vector3(1, 18, 1);
        Instantiate(Pieces[0], spawnPosition, Quaternion.identity);
        //Instantiate(Pieces[Random.Range(0, Pieces.Length-1)], spawnPosition, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public GameObject[] blocks;
    public GameObject spawnPoint;
    private BlockPiece[,] _blockLocation;
    
    // Start is called before the first frame update
    void Start()
    {
        _blockLocation = new BlockPiece[(int) transform.localScale.x, (int) transform.localScale.y];
        NewBlock();
    }
    
    public void UpdateBlocksPos(Transform blockObj, string id)
    {
        foreach (Transform block in blockObj)
        {
            var position = block.transform.position;
            var posX = Mathf.RoundToInt(position.x);
            var posY = Mathf.RoundToInt(position.y);
            _blockLocation[posX, posY] = new BlockPiece(id, block);
        }
    }
    public void ClearBlocksPos(Transform blockObj)
    {
        foreach (Transform block in blockObj)
        {
            var position = block.transform.position;
            var posX = Mathf.RoundToInt(position.x);
            var posY = Mathf.RoundToInt(position.y);
            _blockLocation[posX, posY] = null;
        }
    }

    public bool CanMove(int posX, int posY, string id)
    {
        return _blockLocation[posX, posY] == null || _blockLocation[posX, posY].id == id;
    }

    private bool IsLineCompleted(int posY)
    {
        for (var j = 0; j < transform.localScale.x; j++)
        {
            if (_blockLocation[j, posY] == null)
            {
                return false;
            }
        }

        return true;
    }

    public void CheckLines(int heigth = 0)
    {
        if (heigth < transform.localScale.y)
        {
            if (!IsLineCompleted(heigth)) CheckLines(heigth + 1);
            else
            {
                for (var a = 0; a < transform.localScale.x; a++)
                { 
                    if (_blockLocation[a, heigth] != null)
                    {
                        GameManager.Instance.Score += 10;
                        DestroyImmediate(_blockLocation[a, heigth].piece.gameObject);
                        _blockLocation[a, heigth] = null;
                    }
                    for (var k = heigth; k < transform.localScale.y; k++)
                    {
                        if (k + 1 < transform.localScale.y)
                        {
                            if (_blockLocation[a, k + 1] == null) continue;
                            _blockLocation[a, k] = _blockLocation[a, k + 1];
                            _blockLocation[a, k + 1] = null;
                            _blockLocation[a, k].piece.position  += new Vector3(0, -1, 0);
                        }
                    }
                }
                CheckLines(heigth);
            }
        }
    }

    public bool IsGameOver(int posY)
    {
        if (posY == spawnPoint.transform.position.y)
        {
            GameManager.Instance.SetGameOver(true);
            return true;
        }

        return false;
    }

    public void Reset()
    {
        for (var j = 0; j < transform.localScale.x; j++)
        {
            for (var i = 0; i < transform.localScale.y; i++)
            {
                _blockLocation[j, i] = null;
            }
        }

        NewBlock();
    }

    public void NewBlock()
    {
        Instantiate(blocks[Random.Range(0, blocks.Length)], spawnPoint.transform.position, quaternion.identity);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagement : MonoBehaviour
{
    private Transform[,,] gridSquares;
    
    // Start is called before the first frame update
    void Start()
    {
        gridSquares = new Transform[(int) transform.localScale.x+1, (int) transform.localScale.y+1, (int) transform.localScale.z+1];
    }

    public void AddPieceToSpace(Transform piece)
    {
        foreach (Transform pieceBlock in piece)
        {
            var roundedX = Mathf.RoundToInt(pieceBlock.transform.position.x);
            var roundedY = Mathf.RoundToInt(pieceBlock.transform.position.y);        
            var roundedZ = Mathf.RoundToInt(pieceBlock.transform.position.z);
            
            AddPieceBlockToSpace(roundedX,roundedY,roundedZ, pieceBlock);
        }
    }

    public bool ValidMove(Transform piece)
    {
        foreach (Transform pieceBlock in piece)
        {
            var roundedX = Mathf.RoundToInt(pieceBlock.transform.position.x);
            var roundedY = Mathf.RoundToInt(pieceBlock.transform.position.y);
            var roundedZ = Mathf.RoundToInt(pieceBlock.transform.position.z);

            if (!IsXValid(roundedX) || !IsYValid(roundedY) || !IsZValid(roundedZ))
            {
                return false;
            }

            if (!IsSpaceFree(roundedX, roundedY, roundedZ))
            {
                return false;
            }
        }

        return true;
    }

    public void CheckCompletedPlanes(Transform piece)
    {
        var intervalY = GetMinAndMaxY(piece);

        CheckPlane();
    }

    private void CheckPlane(int coordYToCheck = 0)
    {
        if (coordYToCheck > transform.localScale.y) return;
        
        if (!IsPlaneComplete(coordYToCheck))
        {
            Debug.Log($"{coordYToCheck}");
            CheckPlane(coordYToCheck + 1);
        }
        else
        {
            Debug.Log($"LIMPOU: -> {coordYToCheck}");
            DestroyPlane(coordYToCheck);
            LowerAbovePieces(coordYToCheck);
            CheckPlane(coordYToCheck);
        }
    }

    private void LowerAbovePieces(int posY)
    {
        for (var y = posY; y < transform.localScale.y-1; y++)
        {
            for (var x = 0; x < transform.localScale.x; x++)
            {
                for (var z = 0; z < transform.localScale.z; z++)
                {
                    gridSquares[x, y, z] = gridSquares[x, y + 1, z];
                    gridSquares[x, y + 1, z] = null;
                    
                    Debug.Log($"{x}, {y}, {z}");
                    
                    if (gridSquares[x, y, z] != null)
                    {
                        gridSquares[x, y, z].position += Vector3.down;
                    }
                }
            }
        }
    }
    
    private void DestroyPlane(int posY)
    {
        for (var x = 0; x < transform.localScale.x; x++)
        {
            for (var z = 0; z < transform.localScale.z; z++)
            {
                if (gridSquares[x, posY, z] != null)
                {
                    DestroyImmediate(gridSquares[x,posY,z].gameObject);
                    RemovePieceBlockFromSpace(x, posY, z);
                }
            }
        }
    }

    private bool IsPlaneComplete(int posY)
    {
        for (var z = 0; z < transform.localScale.z; z++)
        {
            if (!CheckLineX(posY, z))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckLineX(int posY, int posZ)
    {
        for (var x = 0; x < transform.localScale.x; x++)
        {
            if (gridSquares[x, posY, posZ] == null)
            {
                return false;
            }
        }

        return true;
    }

    private (int, int) GetMinAndMaxY(Transform piece)
    {
        var minY = Mathf.RoundToInt(transform.localScale.y);
        var maxY = 0;
        
        foreach (Transform pieceBlock in piece)
        {
            if (pieceBlock.transform.position.y < minY)
            {
                minY = Mathf.RoundToInt(pieceBlock.transform.position.y);
            }
            
            if (pieceBlock.transform.position.y > maxY)
            {
                maxY = Mathf.RoundToInt(pieceBlock.transform.position.y);
            }
        }

        return (minY, maxY);
    }
    

    public bool IsGameOver(float posY)
    {
        var roundedY = Mathf.RoundToInt(posY);

        return roundedY >= transform.localScale.y;
    }

    public void ResetGame()
    {
        for (var x = 0; x < transform.localScale.x; x++)
        {
            for (var y = 0; y < transform.localScale.y; y++)
            {
                for (var z = 0; z < transform.localScale.z; z++)
                {
                    RemovePieceBlockFromSpace(x,y,z);
                }
            }
        }
    }
    
    private void AddPieceBlockToSpace(
        int posX,
        int posY,
        int posZ,
        Transform piece)
    {
        gridSquares[posX, posY, posZ] = piece;
    }

    private void RemovePieceBlockFromSpace(
        int posX,
        int posY,
        int posZ)
    {
        gridSquares[posX, posY, posZ] = null;
    }
    
    private bool IsSpaceFree(
        int posX,
        int posY,
        int posZ)
    {
        return gridSquares[posX, posY, posZ] == null;
    }

    private bool IsXValid(int x)
    {
        return x >= 0 && x < transform.localScale.x;
    }
    
    private bool IsYValid(int y)
    {
        return y >= 0;
    }

    private bool IsZValid(int z)
    {
        return z >= 0 && z < transform.localScale.z;
    }
}
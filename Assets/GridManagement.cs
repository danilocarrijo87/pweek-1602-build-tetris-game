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

            AddPieceBlockToSpace(roundedX,roundedY,roundedZ, piece);
        }
    }

    public void RemovePieceFromSpace(Transform piece)
    {
        foreach (Transform pieceBlock in piece)
        {
            var roundedX = Mathf.RoundToInt(pieceBlock.transform.position.x);
            var roundedY = Mathf.RoundToInt(pieceBlock.transform.position.y);        
            var roundedZ = Mathf.RoundToInt(pieceBlock.transform.position.z);

            RemovePieceBlockFromSpace(roundedX,roundedY,roundedZ);
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

    // Need to implement rotate first so I can test it
    public bool CheckCompletedPlanes(Transform piece)
    {
        var completedPlane = false;
        var intervalY = GetMinAndMaxY(piece);

        for (var y = intervalY.Item1; y <= intervalY.Item2; y++)
        {
            if (!IsLineComplete(y))
            {
                continue;
            }

            DestroyPlane(y);
            completedPlane = true;
        }

        /*if (completedPlane)
        {
            LowerAbovePieces(intervalY.Item1);
        }*/

        return completedPlane;
    }

    private void LowerAbovePieces(int posY)
    {
        for (var y = posY; y < transform.localScale.y; y++)
        {
            for (var x = 0; x <= transform.localScale.x; x++)
            {
                for (var z = 0; z <= transform.localScale.z; z++)
                {
                    gridSquares[x, y, z] = gridSquares[x, y + 1, z];
                    gridSquares[x, y + 1, z] = null;
                    
                    if (gridSquares[x, y, z] != null)
                    {
                        gridSquares[x, y, z].transform.position += new Vector3(0, -1, 0);
                    }
                }
            }
        }
    }
    
    private void DestroyPlane(int posY)
    {
        for (int x = 0; x < transform.localScale.x; x++)
        {
            for (int z = 0; z <= transform.localScale.z; z++)
            {
                DestroyImmediate(gridSquares[x,posY,z].gameObject);
                RemovePieceBlockFromSpace(x, posY, z);
            }
        }
    }

    private bool IsLineComplete(int posY)
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
        return z >= 0 && z <= transform.localScale.z;
    }
}

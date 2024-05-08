using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.TerrainTools;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MovementScript : MonoBehaviour
{
    private static GridManagement gridManagement;
    private SpawnPieceScript spawnPieceScript;
    
    private float previousTime;
    private float defaultFallTime = 1f;
    private bool isGameAlive = true;

    private Dictionary<int, Vector3> movements = new();
    
    // Start is called before the first frame update
    void Start()
    {
        movements[(int)KeyCode.LeftArrow] = Vector3.left;
        movements[(int)KeyCode.RightArrow] = Vector3.right;
        movements[(int)KeyCode.DownArrow] = Vector3.back;
        movements[(int)KeyCode.UpArrow] = Vector3.forward;
        movements[(int)KeyCode.Space] = Vector3.down;
        movements[(int)KeyCode.LeftShift] = new Vector3(0, 0, -90);   // Rotate on Z axis
        movements[(int)KeyCode.LeftControl] = new Vector3(-90, 0, 0); // Rotate on X axis
        
        gridManagement = GameObject.FindWithTag("BoardGrid").GetComponent<GridManagement>();
        spawnPieceScript = GameObject.FindWithTag("PieceSpawner").GetComponent<SpawnPieceScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameAlive) return;
        
        Vector3 move = default;
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move = movements[(int)KeyCode.LeftArrow];
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move = movements[(int)KeyCode.RightArrow];
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move = movements[(int)KeyCode.DownArrow];
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move = movements[(int)KeyCode.UpArrow];
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            RotatePiece(movements[(int)KeyCode.LeftShift]);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            RotatePiece(movements[(int)KeyCode.LeftControl]);
        }

        if (move != default)
        {
            MovePiece(move);
            
            if (!gridManagement.ValidMove(transform))
            {
                ResetMove(move);
            }
        }

        var fallTime = Input.GetKey(KeyCode.Space) ? defaultFallTime / 10 : defaultFallTime;

        LowerPiece(fallTime);
    }

    private void MovePiece(Vector3 move)
    {
        transform.position += move;
    }

    private void ResetMove(Vector3 move)
    {
        transform.position -= move;
    }

    private void RotatePiece(Vector3 move)
    {
        transform.localEulerAngles += move;
        if (!gridManagement.ValidMove(transform))
        {
            transform.localEulerAngles -= move;
        }
    }

    private void LowerPiece(float fallTime)
    {
        if (Time.time - previousTime <= fallTime)
        {
            return;
        }
        
        var move = movements[(int)KeyCode.Space];
        MovePiece(move);

        if (gridManagement.ValidMove(transform, false))
        {
            if (!gridManagement.IsGridSpaceAvailable(transform))
            {
                ResetMove(move);
                this.enabled = false;
            
                var isGameOver = gridManagement.IsGameOver(transform.position.y);
                
                if (!isGameOver)
                {
                    InvalidMove(transform);
                }
                else
                {
                    isGameAlive = false;
                    spawnPieceScript.GameOver();
                    //gridManagement.ResetGame();
                }
            }
        }
        else
        {
            ResetMove(move);
            this.enabled = false;

            InvalidMove(transform);
        }
        
        previousTime = Time.time;
    }

    private void InvalidMove(Transform piece)
    {
        gridManagement.AddPieceToSpace(transform);
        gridManagement.CheckCompletedPlanes(transform);
        spawnPieceScript.NewPiece();
    }

    public void IncrementLowerSpeed(float value)
    {
        defaultFallTime += value;
    }
    
    public void SetLowerSpeed(float value)
    {
        defaultFallTime = value;
    }
}

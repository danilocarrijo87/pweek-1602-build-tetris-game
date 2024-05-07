using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.TerrainTools;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MovementScript : MonoBehaviour
{
    private static GridManagement gridManagement;
    private float previousTime;
    private float defaultFallTime = 1f;

    private Dictionary<int, Vector3> movements = new();
    
    // Start is called before the first frame update
    void Start()
    {
        movements[(int)KeyCode.LeftArrow] = new Vector3(-1, 0, 0);
        movements[(int)KeyCode.RightArrow] = new Vector3(1, 0, 0);
        movements[(int)KeyCode.DownArrow] = new Vector3(0, 0, -1);
        movements[(int)KeyCode.UpArrow] = new Vector3(0, 0, 1);
        movements[(int)KeyCode.Space] = new Vector3(0, -1, 0);
        movements[(int)KeyCode.LeftShift] = new Vector3(0, 0, -90);
        movements[(int)KeyCode.LeftControl] = new Vector3(-90, 0, 0);
        
        gridManagement = GameObject.FindWithTag("BoardGrid").GetComponent<GridManagement>();
    }

    // Update is called once per frame
    void Update()
    {
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
        
        // Can be improved by checking only the Y coord.
        if (!gridManagement.ValidMove(transform))
        {
            ResetMove(move);
            this.enabled = false;
            
            var isGameOver = gridManagement.IsGameOver(transform.position.y);

            if (!isGameOver)
            {
                gridManagement.AddPieceToSpace(transform);
                FindObjectOfType<SpawnPieceScript>().NewPiece();
                gridManagement.CheckCompletedPlanes(transform);
            }
            else
            {
                gridManagement.ResetGame();
            }
        }
        
        previousTime = Time.time;
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

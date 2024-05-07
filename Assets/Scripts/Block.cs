using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector3 pivot;
    private float loopTime;

    private static Grid Grid;
    [HideInInspector]
    public string id;
    bool isBlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        Grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        var localScale = Grid.transform.localScale;
    }

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (!isBlocked)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(-1))
            {
                MoveX(-1);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(1))
            {
                MoveX(1);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && CanMove(0))
            {
                Grid.ClearBlocksPos(this.transform);
                transform.RotateAround( transform.TransformPoint(pivot), new Vector3(0,0,1), 90);
                Grid.UpdateBlocksPos(this.transform, id);
            }

            if (CanMove(0) && Time.time - loopTime > (Input.GetKey(KeyCode.DownArrow) ? GameManager.Instance.gameSpeed /10 : GameManager.Instance.gameSpeed))
            {
                Grid.ClearBlocksPos(this.transform);
                transform.position  += new Vector3(0, -1, 0);
                loopTime = Time.time;
                Grid.UpdateBlocksPos(this.transform, id);
            }
        }
    }

    private void MoveX(int x)
    {
        Grid.ClearBlocksPos(this.transform);
        transform.position += new Vector3(x, 0, 0);
        Grid.UpdateBlocksPos(this.transform, id);
    }

    private bool CanMove(int xModifier)
    {
        foreach (Transform block in transform)
        {
            var position = block.transform.position;
            var posX = Mathf.RoundToInt(position.x + xModifier);
            var posY = Mathf.RoundToInt(position.y - 1);
            
            
            if (posX < 0 || posX >= Grid.transform.localScale.x)
            {
                return false;
            }

            if (posY < 0)
            {
                isBlocked = true;
                this.enabled = false;
                if (!Grid.IsGameOver(Mathf.RoundToInt(position.y )))
                {
                    Grid.UpdateBlocksPos(this.transform, id);
                    Grid.CheckLines();
                    Grid.NewBlock();
                    return false;
                }
            }
            
            var canMove = Grid.CanMove(posX, posY, id);
            if (!canMove)
            {
                isBlocked = true;
                this.enabled = false;
                if (!Grid.IsGameOver(Mathf.RoundToInt(position.y )))
                {
                    Grid.UpdateBlocksPos(this.transform, id);
                    Grid.CheckLines();
                    Grid.NewBlock();
                    return false;
                }
            }

        }
        return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector3 pivot;
    private float _loopTime;

    private Grid _grid;
    [HideInInspector]
    public string id;

    private bool _isBlocked = false;

    private static Color lightGreen = new Color(171f, 211f, 179f);
    private static Color green = new Color(115f, 182f, 128f);
    private static Color darkGreen = new Color(58f, 91f, 64f);
    private static Color lightBlue = new Color(127f, 175f, 208f);
    private static Color blue = new Color(42f, 122f, 176f);
    private static Color darkBlue = new Color(21f, 61f, 88f);

    // Start is called before the first frame update
    void Start()
    {
        _grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        var localScale = _grid.transform.localScale;
    }

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        List<Color> blockColors = new List<Color>() { lightGreen, green, darkGreen, lightBlue, blue, darkBlue };
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (var spriteRenderer in sprites)
        {
            var color = blockColors[0];
            Debug.Log(color);
            Debug.Log(Color.yellow);
            // spriteRenderer.color = new Color(21f, 61f, 88f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!_isBlocked)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(-1))
            {
                MoveX(-1);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(1))
            {
                MoveX(1);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && CanRotate())
            {
                _grid.ClearBlocksPos(this.transform);
                transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0,0,1), 90);
                _grid.UpdateBlocksPos(this.transform, id);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HardDrop();
            } 
            else if (CanMove(0) && Time.time - _loopTime > (Input.GetKey(KeyCode.DownArrow) ? GameManager.Instance.gameSpeed / 10 : GameManager.Instance.gameSpeed))
            {
                _grid.ClearBlocksPos(this.transform);
                transform.position += new Vector3(0, -1, 0);
                _loopTime = Time.time;
                _grid.UpdateBlocksPos(this.transform, id);
            }
        }
    }

    private void HardDrop()
    {
        while (CanMove(0) && this.enabled)
        {
            _grid.ClearBlocksPos(this.transform);
            transform.position += new Vector3(0, -1, 0);
            _loopTime = Time.time;
            _grid.UpdateBlocksPos(this.transform, id);
        }
    }

    private void MoveX(int x)
    {
        _grid.ClearBlocksPos(this.transform);
        transform.position += new Vector3(x, 0, 0);
        _grid.UpdateBlocksPos(transform, id);
    }

    private bool CanRotate()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        Vector3 pivotPoint = transform.TransformPoint(pivot);

        foreach (Transform block in transform)
        {
            Vector3 dir = block.position - pivotPoint;
            dir = rotation * dir;
            Vector3 newPos = pivotPoint + dir;

            int posX = Mathf.RoundToInt(newPos.x);
            int posY = Mathf.RoundToInt(newPos.y);

            if (posX < 0 || posX >= _grid.transform.localScale.x)
            {
                return false;
            }

            if (!_grid.CanMove(posX, posY, id))
            {
                return false;
            }
        }
        return true;
    }
    
    private bool CanMove(int xModifier)
    {
        foreach (Transform block in transform)
        {
            var position = block.transform.position;
            var posX = Mathf.RoundToInt(position.x + xModifier);
            var posY = Mathf.RoundToInt(position.y - 1);
            
            if (posX < 0 || posX >= _grid.transform.localScale.x)
            {
                return false;
            }

            if (posY < 0)
            {
                DisableBlockAndAddAnotherIfNotGameOver(position.y);
                return false;
            }
            
            var canMove = _grid.CanMove(posX, posY, id);
            if (!canMove)
            {
                DisableBlockAndAddAnotherIfNotGameOver(position.y);
                return false;
            }

        }
        return true;
    }

    public void DisableBlockAndAddAnotherIfNotGameOver(float posY)
    {
        _isBlocked = true;
        this.enabled = false;
        if (!_grid.IsGameOver(Mathf.RoundToInt(posY)))
        {
            _grid.UpdateBlocksPos(this.transform, id);
            _grid.CheckLines();
            _grid.NewBlock();
        }
    }
}

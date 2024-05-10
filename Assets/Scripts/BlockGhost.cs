using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGhost : MonoBehaviour
{
    public Vector3 pivot;
    private Grid _grid;
    public bool isEnabled = true;
    [HideInInspector]
    public string id;
    // Start is called before the first frame update
    void Start()
    {
        _grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
    }

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateSelf()
    {
            _grid.ClearBlocksPos(transform, true);
            transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), 90);
            var minY = 0;
            foreach (Transform t in transform)
            {
                if (Mathf.RoundToInt(t.position.y) < minY) {
                    minY = Mathf.RoundToInt(t.position.y);
                }
            }
        transform.position += new Vector3(0, -minY);
        _grid.UpdateBlocksPos(transform, id, true);
    }

    public bool CanRotate()
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

            if (posY < 0)
            {

            }

            if (!_grid.CanMove(posX, posY, id, true))
            {
                return false;
            }
        }
        return true;
    }

    public void MoveX(int x)
    {
        _grid.ClearBlocksPos(transform, true);
        transform.position = new Vector3(transform.position.x + x, 0, 0);
        transform.position = _grid.SetLowestYCoordinateForGhost(transform);
        _grid.UpdateBlocksPos(transform, id, true);
    }
}

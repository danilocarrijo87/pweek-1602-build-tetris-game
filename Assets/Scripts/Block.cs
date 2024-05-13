using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector3 pivot;
    private float _loopTime;

    private Grid _grid;
    [HideInInspector]
    public GameObject _ghost;
    [HideInInspector]
    public string id;
    [HideInInspector]
    public BlockGhost _ghostBlock;
    

    private bool _isBlocked = false;
    private AudioManager _audioManager;
    private Color color;

    private Color lightGreen = new Color(171f/255f, 211f/255f, 179f/255f);
    private Color green = new Color(115f/255f, 182f/255f, 128f/255f);
    private Color darkGreen = new Color(58f/255f, 91f/255f, 64f/255f);
    private Color lightBlue = new Color(127f/255f, 175f/255f, 208f/255f);
    private Color blue = new Color(42f/255f, 122f/255f, 176f/255f);
    private Color darkBlue = new Color(21f/255f, 61f/255f, 88f/255f);
    

    // Start is called before the first frame update
    void Start()
    {
        _grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        var localScale = _grid.transform.localScale;
    }

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        List<Color> blockColors = new List<Color>() { lightGreen, green, darkGreen, lightBlue, blue, darkBlue };
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        color = blockColors[UnityEngine.Random.Range(0, blockColors.Count - 1)];
        foreach (var spriteRenderer in sprites)
        {
            spriteRenderer.color = color;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!_isBlocked)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(-1))
            {
                _audioManager.PlaySFX(_audioManager.MovePiece);
                MoveX(-1);
                _ghostBlock.MoveX(-1);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(1))
            {
                _audioManager.PlaySFX(_audioManager.MovePiece);
                MoveX(1);
                _ghostBlock.MoveX(1);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && CanRotate())
            {
                RotateSelf();
                 _ghostBlock.RotateSelf();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _audioManager.PlaySFX(_audioManager.DropPiece);
                HardDrop();
                _grid.ClearBlocksPos(_ghostBlock.transform, true);
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

    private void RotateSelf()
    {
        _audioManager.PlaySFX(_audioManager.SpinPiece);
        _grid.ClearBlocksPos(this.transform);
        transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), 90);
        _grid.UpdateBlocksPos(this.transform, id);
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
                _audioManager.PlaySFX(_audioManager.CantMove);
                return false;
            }

            if (!_grid.CanMove(posX, posY, id))
            {
                _audioManager.PlaySFX(_audioManager.CantMove);
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
                if(xModifier != 0) _audioManager.PlaySFX(_audioManager.CantMove);
                return false;
            }

            if (posY < 0)
            {
                if(xModifier != 0) _audioManager.PlaySFX(_audioManager.CantMove);
                DisableBlockAndAddAnotherIfNotGameOver(position.y);
                return false;
            }
            
            var canMove = _grid.CanMove(posX, posY, id);
            if (!canMove)
            {
                if(xModifier != 0) _audioManager.PlaySFX(_audioManager.CantMove);
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
        _ghostBlock.isEnabled = false;
        _grid.ClearBlocksPos(_ghostBlock.transform, true);
        Destroy(_ghost);
        if (!_grid.IsGameOver(Mathf.RoundToInt(posY)))
        {
            _grid.UpdateBlocksPos(this.transform, id);
            _grid.CheckLines();
            _grid.NewBlock();
        }
    }
}

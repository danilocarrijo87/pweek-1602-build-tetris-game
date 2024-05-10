using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public GameObject[] blocks;
    public GameObject[] ghostBlocks;
    public GameObject spawnPoint;
    public GameObject newxtBlockPoint;
    public GameObject fireWorks;
    private int _nextBlock;
    private BlockPiece[,] _blockLocation;
    private BlockPiece[,] _ghostLocation;
    private GameObject _nextBlockObj;
    private static int _previousScore = 0;
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _blockLocation = new BlockPiece[(int) transform.localScale.x, (int) transform.localScale.y];
        _ghostLocation = new BlockPiece[(int) transform.localScale.x, (int) transform.localScale.y];
        NewBlock();
    }
    
    public void UpdateBlocksPos(Transform blockObj, string id, bool isGhost = false)
    {
        foreach (Transform block in blockObj)
        {
            var position = block.transform.position;
            var posX = Mathf.RoundToInt(position.x);
            var posY = Mathf.RoundToInt(position.y);
            if (isGhost)
            {
                _ghostLocation[posX, posY] = new BlockPiece(id, block);
            } else
            {
                _blockLocation[posX, posY] = new BlockPiece(id, block);
            }
        }
    }
    public void ClearBlocksPos(Transform blockObj, bool isGhost = false)
    {
        foreach (Transform block in blockObj)
        {
            var position = block.transform.position;
            var posX = Mathf.RoundToInt(position.x);
            var posY = Mathf.RoundToInt(position.y);
            if (isGhost)
            {
                _ghostLocation[posX, posY] = null;
            } else
            {
                _blockLocation[posX, posY] = null;
            }
        }
    }

    public bool CanMove(int posX, int posY, string id, bool isGhost = false)
    {
        if (isGhost)
        {
            return _ghostLocation[posX, posY] == null || _ghostLocation[posX, posY].id == id;
        }
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

        _audioManager.PlaySFX(_audioManager.PlayerScore);
        return true;
    }

    public void CheckLines(int heigth = 0, int lineCount = 0)
    {
        if (heigth < transform.localScale.y)
        {
            if (!IsLineCompleted(heigth)) CheckLines(heigth + 1, lineCount);
            else
            {
                for (var a = 0; a < transform.localScale.x; a++)
                { 
                    if (_blockLocation[a, heigth] != null)
                    {
                        GameManager.Instance.Score += 10;
                        if (_blockLocation[a, heigth].piece.GetComponent<BlockBlink>() != null)
                        {
                            _blockLocation[a, heigth].piece.GetComponent<BlockBlink>().willBeDestroyed = true;
                        }
                        Destroy(_blockLocation[a, heigth].piece.gameObject, GameManager.Instance.destroyBlockTime);
                        _blockLocation[a, heigth] = null;
                    }
                    for (var k = heigth; k < transform.localScale.y; k++)
                    {
                        if (k + 1 < transform.localScale.y)
                        {
                            if (_blockLocation[a, k + 1] == null) continue;
                            _blockLocation[a, k] = _blockLocation[a, k + 1];
                            _blockLocation[a, k + 1] = null;
                            StartCoroutine(PullDown(_blockLocation[a, k].piece));
                        }
                    }
                }
                if (GameManager.Instance.Score - _previousScore >= 1000)
                {
                    GameManager.Instance.gameSpeed -= 0.1f;
                    _previousScore = GameManager.Instance.Score;
                }
                CheckLines(heigth, lineCount += 1);
            }
        }
        
        if (lineCount >= GameManager.Instance.lineNeededToActivateEffect)
        {
            StartCoroutine(CongratsEffect());
        }
    }

    private IEnumerator CongratsEffect()
    {
        fireWorks.SetActive(true);
        yield return new WaitForSeconds(3f);
        fireWorks.SetActive(false);
    }

    private IEnumerator PullDown(Transform piece)
    {
        yield return new WaitForSeconds(GameManager.Instance.destroyBlockTime);
        if (piece != null)
        {
            piece.position  += new Vector3(0, -1, 0);
        }
    }

    public bool IsGameOver(int posY)
    {
        if (posY == spawnPoint.transform.position.y)
        {
            GameManager.Instance.SetGameOver(true);
            _audioManager.PlayMusicSource(_audioManager.GameOver);
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

        _audioManager.PlayMusicSource(_audioManager.Theme);
        NewBlock();
    }

    private bool YCollisionDetected(Block block, Transform ghostTransform)
    {
        foreach (Transform t in ghostTransform)
        {
            var position = t.position;
            var posX = Mathf.RoundToInt(position.x);
            var posY = Mathf.RoundToInt(position.y);
            if (posY < 0)
            {
                return true;
            }
            if (_blockLocation[posX, posY] != null && _blockLocation[posX, posY].id != block.id) 
            {
                return true;
            }
        }
        return false;
    }

    public Vector3 SetLowestYCoordinateForGhost(Transform transform)
    {
        var currentBlock = GameObject.FindGameObjectsWithTag("Block").Where(obj => obj.GetComponent<Block>().enabled).First().GetComponent<Block>();
        while (YCollisionDetected(currentBlock, transform))
        {
            transform.position += new Vector3(0, 1);
        }
        return transform.position;
    }

    private bool CollisionDetected(BlockGhost ghost)
    {
        foreach (Transform block in ghost.transform)
        {
            var position = block.position;
            var posX = Mathf.RoundToInt(position.x);
            var posY = Mathf.RoundToInt(position.y);
            if (posY < 0 && ghost.transform.position.y == 0)
            {
                return true;
            }
            if (posX >= 0 && posY >= 0 && _blockLocation[posX, posY] != null)
            {
                return true;
            }
        }
        return false;
    }

    private GameObject CreateGhostAtValidPosition()
    {
        Vector3 ghostPosition = new Vector3(spawnPoint.transform.position.x, 0);
        var ghost = Instantiate(ghostBlocks[_nextBlock], ghostPosition, quaternion.identity);
        var ghostComponent = ghost.GetComponent<BlockGhost>();
        while (CollisionDetected(ghostComponent))
        {
            ghostComponent.transform.position += new Vector3(0, 1);
        }
        return ghost;
    }

    public void NewBlock()
    {
        var currentBlock = Instantiate(blocks[_nextBlock], spawnPoint.transform.position, quaternion.identity);
        var block = currentBlock.GetComponent<Block>();
        block._ghost = CreateGhostAtValidPosition();
        block._ghostBlock = block._ghost.GetComponent<BlockGhost>();
        _nextBlock = Random.Range(0, blocks.Length);
        Destroy(_nextBlockObj);
        _nextBlockObj = Instantiate(blocks[_nextBlock], newxtBlockPoint.transform.position, quaternion.identity);
    }
}
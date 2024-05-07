using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPiece
{
    public string id;
    public Transform piece;

    public BlockPiece(string id, Transform piece)
    {
        this.id = id;
        this.piece = piece;
    }
}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovePoint
{
    public Vector3 position;
    public Vector2 direction;
    public float delay;

    public MovePoint(Vector3 position, Vector2 direction, float delay)
    {
        this.position = position;
        this.direction = direction;
        this.delay = delay;
    }
}

public class Piece : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    public MatchValue matchValue;

    private int moveX;
    private int moveY;
    private BoardManager _board;

  //  [HideInInspector]
    public List<MovePoint> futurePositions;
    [HideInInspector]
    public float speedTime = 0f;
    [HideInInspector]
    public bool marked = false;
    [HideInInspector]
    public bool toMove = false;

    public void Init(BoardManager board)
    {
        futurePositions = new List<MovePoint>();
        _board = board;
    }

    public void SetCoords(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }
}

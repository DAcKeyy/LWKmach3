using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static Action<List<Piece>> OnMatched = delegate { };
    public static Action<Tile> OnSelected = delegate { };

    public int xIndex;
    public int yIndex;

    public Sprite lightTile;
    public Sprite darkTile;

    public TileType tileType;
    public TileType becomeTileType;
    public List<Sprite> obstacleTiles;

    private BoardManager _board;
    private SpriteRenderer srenderer;
    public int health;
    public List<MovePoint> futurePositions;
    public int tileColor;

    public void Init(int x, int y, BoardManager board, int width, int height)
    {
        srenderer = transform.GetComponent<SpriteRenderer>();

        xIndex = x;
        yIndex = y;
        _board = board;

        if (obstacleTiles.Count > 0)
        {
            srenderer.sprite = obstacleTiles[obstacleTiles.Count - 1];
            health = obstacleTiles.Count;
        }
    }

    public void SetCoords(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void BreakObstacleTile(ref Tile[,] tiles)
    {
        if (tileType == TileType.Obstacle)
        {
            health--;

            if (health > 0)
                srenderer.sprite = obstacleTiles[health - 1];

            if (health == 0)
            {
                tileType = becomeTileType;
                srenderer.sprite = GetProperSprite(xIndex, yIndex);
            }
        }
    }

    private Sprite GetProperSprite(int i, int j)
    {
        Sprite tile = null;

        if (_board.GetTiles()[i, j + 1] != null)
        {
            if (_board.GetTiles()[i, j + 1].tileColor == 0)
            {
                tile = darkTile;
                tileColor = 1;
            }
            else
            {
                tileColor = 0;
                tile = lightTile;
            }
        }

        return tile;
    }

    private void OnMouseDown()
    {
        if (_board != null)
        {
            _board.ClickTile(this);
        }
    }

    private void OnMouseEnter()
    {
        if (_board != null)
        {
            _board.DragToTile(this);
        }
    }

    private void OnMouseUp()
    {
        if (_board != null)
        {
            _board.RealiseTile();
        }
    }
}
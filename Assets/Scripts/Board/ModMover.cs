using System;
using System.Collections.Generic;
using UnityEngine;

public class ModMover
{
    public static Action OnTileBecameEmpty = delegate { };
    public static Action PiecesMoved = delegate { };

    private int _width;
    private int _height;
    private int _boardUpHeight;

    private BoardManager _boardManager;

    private bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0 && y < _height);
    }

    public ModMover(int width, int height, BoardManager boardManager, int boardUpHeight)
    {
        _width = width;
        _height = height;
        _boardManager = boardManager;
        _boardUpHeight = boardUpHeight;
    }

    public Tile[,] MoveTilesUp()
    {
        Tile[,] tiles = _boardManager.GetTiles();

        for (int j = _height - _boardUpHeight - 1; j >= 0; j--)
        {
            for (int i = 0; i < _width; i++)
            {
                if (tiles[i, j] != null)
                {
                    MoveTileTo(i, i, j, j + _boardUpHeight);
                }
            }
        }

        return tiles;
    }

    public Piece[,] MovePiecesUp()
    {
        Piece[,] pieces = _boardManager.GetPieces();

        for (int j = _height - _boardUpHeight - 1; j >= 0; j--)
        {
            for (int i = 0; i < _width; i++)
            {
                if (pieces[i, j] != null)
                {
                    MovePieceTo(i, i, j, j + _boardUpHeight);
                }
            }
        }

        return pieces;
    }

    public Piece[,] MovePiecesDown()
    {
        Piece[,] pieces = _boardManager.GetPieces();
        int iterations = 0;

        while (iterations < 25)
        {
            for (int j = 0; j < _height; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    if (_boardManager.GetPieces()[i, j] != null)
                    {
                        if (IsWithinBounds(i, j - 1) && CanPieceFallThere(i, j - 1))
                        {
                            MovePieceTo(i, i, j, j - 1);
                        }
                    }
                }
            }

            _boardManager.SetDelay();
            _boardManager.OnTileBecameEmptyRow(SetTimeDelay());

            iterations++;
        }

        return pieces;
    }

    private List<float> SetTimeDelay()
    {
        List<float> delays = new List<float>();

        for (int i = 0; i < _width; i++)
        {
            if ((_boardManager.GetPieces()[i, _height - 1] == null && _boardManager.GetTiles()[i, _height - 1].tileType != TileType.Obstacle))
            {
                for (int u = _height - 2; u >= 0; u--)
                {
                    if (_boardManager.GetPieces()[i, u] == null && _boardManager.GetTiles()[i, u].tileType != TileType.Obstacle)
                    {
                        if (u != 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (u == 0 && _boardManager.GetPieces()[i, u] == null && _boardManager.GetTiles()[i, u].tileType != TileType.Obstacle)
                            {
                                delays.Add(0.15f);
                            }
                        }
                    }

                    if (_boardManager.GetPieces()[i, u] != null && _boardManager.GetTiles()[i, u].tileType != TileType.Obstacle)
                    {
                        delays.Add(_boardManager.GetPieces()[i, u].speedTime + 0.15f);
                        break;
                    }


                    if (_boardManager.GetTiles()[i, u].tileType == TileType.Obstacle)
                    {
                        delays.Add(0.15f);
                        break;
                    }
                }
            }
        }
        return delays;
    }

    private bool CanPieceFallThere(int i, int j)
    {
        bool canFall = false;

        if (_boardManager.GetPieces()[i, j] == null && _boardManager.GetTiles()[i, j].tileType != TileType.Obstacle)
        {
            canFall = true;
        }

        return canFall;
    }

    private void MovePieceTo(int fromI, int toI, int fromJ, int toJ)
    {
        _boardManager.GetPieces()[toI, toJ] = _boardManager.GetPieces()[fromI, fromJ];
        _boardManager.GetPieces()[toI, toJ].SetCoords(toI, toJ);
        _boardManager.GetPieces()[toI, toJ].futurePositions.Add(new MovePoint(new Vector3(toI, toJ, 0f), new Vector2(0f, 0f), 0f));
        _boardManager.GetPieces()[fromI, fromJ] = null;
    }

    private void MoveTileTo(int fromI, int toI, int fromJ, int toJ)
    {
        _boardManager.GetTiles()[toI, toJ] = _boardManager.GetTiles()[fromI, fromJ];
        _boardManager.GetTiles()[toI, toJ].SetCoords(toI, toJ);
        _boardManager.GetTiles()[toI, toJ].futurePositions.Add(new MovePoint(new Vector3(toI, toJ, 0f), new Vector2(0f, 0f), 0f));
        _boardManager.GetTiles()[fromI, fromJ] = null;
    }
}






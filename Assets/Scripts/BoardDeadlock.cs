using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardDeadlock
{
    private int _width;
    private int _height;

    public BoardDeadlock(int width, int height)
    {
        _width = width;
        _height = height;
    }

    private List<Piece> GetRowOrColumnList(Piece[,] allPieces, int x, int y, int listLength = 3, bool checkRow = true)
    {
        List<Piece> piecesList = new List<Piece>();

        for (int i = 0; i < listLength; i++)
        {
            if (checkRow)
            {
                if (x + i < _width && y < _height)
                {
                    if (allPieces[x + i, y] != null)
                    {
                        piecesList.Add(allPieces[x + i, y]);
                    }
                }
            }
            else
            {
                if (x < _width && y + i < _height)
                {
                    if (allPieces[x, y + i] != null)
                    {
                        piecesList.Add(allPieces[x, y + i]);
                    }
                }
            }
        }
        return piecesList;
    }

    private List<Piece> GetMinimumMatches(List<Piece> gamePieces, int minForMatch = 2)
    {
        List<Piece> matches = new List<Piece>();

        var groups = gamePieces.GroupBy(n => n.matchValue);

        foreach (var grp in groups)
        {
            if (grp.Count() >= minForMatch && grp.Key != MatchValue.None)
            {
                matches = grp.ToList();
            }
        }
        return matches;
    }

    private List<Piece> GetNeighbors(Piece[,] allPieces, int x, int y)
    {
        List<Piece> neighbors = new List<Piece>();

        Vector2[] searchDirections = new Vector2[4]
        {
            new Vector2(-1f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 1f),
            new Vector2(0f, -1f)
        };

        foreach (Vector2 dir in searchDirections)
        {
            if (x + (int)dir.x >= 0 && x + (int)dir.x < _width && y + (int)dir.y >= 0 && y + (int)dir.y < _height)
            {
                if (allPieces[x + (int)dir.x, y + (int)dir.y] != null)
                {
                    if (!neighbors.Contains(allPieces[x + (int)dir.x, y + (int)dir.y]))
                    {
                        neighbors.Add(allPieces[x + (int)dir.x, y + (int)dir.y]);
                    }
                }
            }
        }
        return neighbors;
    }

    private bool HasMoveAt(Piece[,] allPieces, int x, int y, int listLength = 3, bool checkRow = true)
    {
        List<Piece> pieces = GetRowOrColumnList(allPieces, x, y, listLength, checkRow);
        List<Piece> matches = GetMinimumMatches(pieces, listLength - 1);
        Piece unmatchedPiece = null;

        if (pieces != null && matches != null)
        {
            if (pieces.Count == listLength && matches.Count == listLength - 1)
            {
                unmatchedPiece = pieces.Except(matches).FirstOrDefault();
            }

            if (unmatchedPiece != null)
            {
                List<Piece> neighbors = GetNeighbors(allPieces, unmatchedPiece.xIndex, unmatchedPiece.yIndex);
                neighbors = neighbors.Except(matches).ToList();
                neighbors = neighbors.FindAll(n => n.matchValue == matches[0].matchValue);
                matches = matches.Union(neighbors).ToList();
            }

            if (matches.Count >= listLength)
            {
               // string rowColStr = (checkRow) ? " row " : " column ";
               // Debug.Log("======= AVAILABLE MOVE ================================");
               // Debug.Log("Move " + matches[0].matchValue + " piece to " + unmatchedPiece.xIndex + "," +
               //     unmatchedPiece.yIndex + " to form matching " + rowColStr);
                return true;
            }
        }
        return false;
    }

    public bool IsDeadlocked(Piece[,] allPieces, int listLength = 3)
    {
        bool isDeadlocked = true;

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (HasMoveAt(allPieces, i, j, listLength, true) || HasMoveAt(allPieces, i, j, listLength, false))
                {
                    isDeadlocked = false;

                }
            }
        }

        //if (isDeadlocked)
        //{
        //    Debug.Log(" ===========  BOARD DEADLOCKED ================= ");
        //}
        return isDeadlocked;
    }
}

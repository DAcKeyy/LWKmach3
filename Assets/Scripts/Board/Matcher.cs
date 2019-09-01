using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Matcher
{
    public int _width;
    public int _height;

    public Matcher(int width, int height)
    {
        _width = width;
        _height = height;
    }

    private bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0 && y < _height);
    }

    private List<Piece> FindMatches(Piece[,] pieces, Tile[,] tiles, int startX, int startY, Vector2 searchDirection, int minLength = 3)
    {
        List<Piece> matches = new List<Piece>();
        Piece startPiece = null;

        if (IsWithinBounds(startX, startY))
        {
            startPiece = pieces[startX, startY];
        }

        if (startPiece != null)
        {
            matches.Add(startPiece);
        }
        else
        {
            return null;
        }

        int nextX;
        int nextY;

        int maxValue = (_width > _height) ? _width : _height;

        for (int i = 1; i < maxValue - 1; i++)
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!IsWithinBounds(nextX, nextY))
            {
                break;
            }

            Piece nextPiece = pieces[nextX, nextY];

            if (nextPiece == null)
            {
                break;
            }
            else
            {
                if (nextPiece.matchValue == startPiece.matchValue &&
                    !matches.Contains(nextPiece))
                {
                    matches.Add(nextPiece);
                }
                else
                {
                    break;
                }
            }
        }

        if (matches.Count >= minLength)
        {
            return matches;
        }

        return null;
    }

    private List<Piece> FindVerticalMatches(Piece[,] pieces, Tile[,] tiles, int startX, int startY, int minLength = 3)
    {
        List<Piece> upwardsMatches = FindMatches(pieces, tiles, startX, startY, new Vector2(0, 1), 2);
        List<Piece> downwardMatches = FindMatches(pieces, tiles, startX, startY, new Vector2(0, -1), 2);

        if (upwardsMatches == null)
        {
            upwardsMatches = new List<Piece>();
        }

        if (downwardMatches == null)
        {
            downwardMatches = new List<Piece>();
        }

        var combinedMatches = upwardsMatches.Union(downwardMatches).ToList();
        return (combinedMatches.Count >= minLength) ? combinedMatches : null;
    }

    private List<Piece> FindHorizontalMatches(Piece[,] pieces, Tile[,] tiles, int startX, int startY, int minLength = 3)
    {
        List<Piece> rightMatches = FindMatches(pieces, tiles, startX, startY, new Vector2(1, 0), 2);
        List<Piece> leftMatches = FindMatches(pieces, tiles, startX, startY, new Vector2(-1, 0), 2);

        if (rightMatches == null)
        {
            rightMatches = new List<Piece>();
        }

        if (leftMatches == null)
        {
            leftMatches = new List<Piece>();
        }

        var combinedMatches = rightMatches.Union(leftMatches).ToList();
        return (combinedMatches.Count >= minLength) ? combinedMatches : null;
    }

    public List<Piece> FindMatchesAt(Piece[,] pieces, Tile[,] tiles, int x, int y, int minLengh = 3)
    {
        List<Piece> horizMatches = FindHorizontalMatches(pieces, tiles, x, y, 3);
        List<Piece> vertMatches = FindVerticalMatches(pieces, tiles, x, y, 3);

        if (horizMatches == null)
        {
            horizMatches = new List<Piece>();
        }

        if (vertMatches == null)
        {
            vertMatches = new List<Piece>();
        }

        var combinedMatches = horizMatches.Union(vertMatches).ToList();
        return combinedMatches;
    }

    public List<Piece> FindAllMatches(Piece[,] pieces, Tile[,] tiles)
    {
        List<Piece> combinedMatches = new List<Piece>();

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                List<Piece> matches = FindMatchesAt(pieces, tiles, i, j);
                combinedMatches = combinedMatches.Union(matches).ToList();
            }
        }
        return combinedMatches;
    }

    public List<List<Piece>> FindAllMatchesNotCompined(Piece[,] pieces, Tile[,] tiles)
    {
        List<List<Piece>> nonCombinedMatches = new List<List<Piece>>();

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                List<Piece> matches = FindMatchesAt(pieces, tiles, i, j);
                nonCombinedMatches.Add(matches);
            }
        }
        return nonCombinedMatches;
    }
}

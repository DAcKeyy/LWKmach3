using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardShuffler
{
    private int _width;
    private int _height;
    private BoardManager _boardManager;

    public BoardShuffler(int width, int height, BoardManager boardManager)
    {
        _width = width;
        _height = height;
        _boardManager = boardManager;
    }

    public List<Piece> RemoveNormalPieces()
    {
        List<Piece> normalPieces = new List<Piece>();

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_boardManager.GetPieces()[i, j] != null)
                {
                    // Bomb bomb = allPieces[i, j].GetComponent<Bomb>();
                    // Collectible collectible = allPieces[i, j].GetComponent<Collectible>();

                    //if (bomb == null && collectible == null)
                    //{
                    normalPieces.Add(_boardManager.GetPieces()[i, j]);

                    _boardManager.GetPieces()[i, j] = null;
                    //}
                }
            }
        }

        return normalPieces;
    }

    private List<Piece> ShuffleList(List<Piece> piecesToShuffle)
    {
        List<Piece> shuffle = new List<Piece>(piecesToShuffle);

        int maxCount = shuffle.Count;

        for (int i = 0; i < maxCount - 1; i++)
        {
            int r = UnityEngine.Random.Range(i, maxCount);

            if (r == i)
            {
                continue;
            }

            Piece temp = shuffle[r];
            shuffle[r] = shuffle[i];
            shuffle[i] = temp;
        }
        return shuffle;
    }

    public void Shuffle()
    {
        List<Piece> pieces = RemoveNormalPieces();
        List<Piece> p = ShuffleList(pieces);
        _boardManager.FillBoardFromList(p);

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_boardManager.GetPieces()[i, j] != null)
                {
                    _boardManager.GetPieces()[i, j].futurePositions.Add(new MovePoint(new Vector3(i, j, 0), new Vector2(0f, 0f), 0f));
                }
            }
        }
    }
}

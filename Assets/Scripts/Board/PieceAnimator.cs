using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using NaughtyAttributes;

public class PieceAnimator : MonoBehaviour
{
    public static Action<Tile, Tile, bool> PiecesSwitched = delegate { };
    public static Action<List<Tile>> PiecesDeleted = delegate { };
    public static Action BoardMovedUp = delegate { };
    public static Action PiecesMoved = delegate { };
    public static Action OnTileBecameEmpty = delegate { };

    [SerializeField] GameObject ExplotionPoint;
    [BoxGroup("Animation settings")]
    public float fallTime = 0.2f;
    [BoxGroup("Animation settings")]
    public float upTime = 1f;
    [BoxGroup("Animation settings")]
    public float swipeTime = 0.175f;
    [BoxGroup("Animation settings")]
    public float deathTime = 0.25f;
    [BoxGroup("Animation settings")]
    public float shuffleTime = 0.6f;
    [BoxGroup("Animation settings")]
    [Space(5)]
    public bool moreElasticFall = false;

    private int _piecesToMove;
    private int _movedPieces;


    private int _piecesToDelete;
    private int _deletedPieces;
    private int _movedTiles;

    private int _width;
    private int _height;
    private int _boardUpHeight;

    private ModMover _modMover;
    private BoardManager _boardManager;

    public void Init(int width, int height, BoardManager boardManager, int boardUpHeight)
    {
        _width = width;
        _height = height;
        _boardManager = boardManager;
        _boardUpHeight = boardUpHeight;
        DOTween.Init(true, true).SetCapacity(1250, 500);
    }

    public void SetModMover(ModMover mover)
    {
        _modMover = mover;
    }

    public void DeleteAnimation(Tile[,] tiles, List<Piece> piecesToDelete)
    {
        if (piecesToDelete.Count > 0)
        {
            List<Tile> emptiedTiles = new List<Tile>();
            _piecesToDelete = piecesToDelete.Count;
            _deletedPieces = 0;

            foreach (Piece piece in piecesToDelete)
            {
                emptiedTiles.Add(tiles[piece.xIndex, piece.yIndex]);
                piece.transform.DOScale(new Vector3(0f, 0f, 0f), deathTime).SetEase(Ease.Linear).OnComplete(() => Deleted(piece, emptiedTiles));
            }
        }
        else
        {
            Debug.Log("No pieces for delete animation");
        }
    }

    public void ExplotionAnimation(Tile[,] tiles, List<Piece> piecesToDelete)
    {
        foreach (Piece piece in piecesToDelete)
        {
            var explotionPointe = Instantiate(ExplotionPoint, new Vector2(piece.xIndex, piece.yIndex), piece.transform.rotation, this.transform);
            explotionPointe.gameObject.GetComponent<Animation>().Play();
        }

        DeleteAnimation(tiles, piecesToDelete);
    }


    private void Deleted(Piece piece, List<Tile> deletedTiles)
    {
        _deletedPieces++;
        GameObject.Destroy(piece.gameObject); //POOL
        if (_deletedPieces == _piecesToDelete)
        {
            _deletedPieces = 0;
            _piecesToDelete = 0;
            PiecesDeleted(deletedTiles);
        }
    }

    public void MovePieces(Piece[,] pieces, Tile clickedTile, Tile targetTile, bool back)
    {
        Piece clickedPiece = pieces[clickedTile.xIndex, clickedTile.yIndex];
        Piece targetPiece = pieces[targetTile.xIndex, targetTile.yIndex];

        clickedPiece.transform.DOMove(new Vector3(clickedTile.xIndex, clickedTile.yIndex, 0f), swipeTime).SetEase(Ease.Linear);
        targetPiece.transform.DOMove(new Vector3(targetTile.xIndex, targetTile.yIndex, 0f), swipeTime).SetEase(Ease.Linear).OnComplete(() => PiecesSwitched(clickedTile, targetTile, back));
    }

    private void Ended(Piece piece, bool shuffled)
    {
        _movedPieces++;

        if (!shuffled)
        {
            int i = UnityEngine.Random.Range(0, 2);

            if (!moreElasticFall)
            {
                piece.transform.DOPunchPosition(new Vector3(0f, -0.05f, 0f), 0.45f, 8, 25f);
            }
            else
            {
                piece.transform.DOPunchPosition(new Vector3(0.0f, -0.1f, 0.0f), 0.4f, 8, 12f, false);
            }

            if (i == 1)
            {
                piece.transform.DOPunchRotation(new Vector3(0f, 0, 4f), 0.65f, 8, 25f);
            }
            else
            {
                piece.transform.DOPunchRotation(new Vector3(0f, 0, -4f), 0.65f, 8, 25f);
            }
        }

        piece.futurePositions.Clear();
        piece.marked = false;
        piece.speedTime = 0f;

        if (_movedPieces == _piecesToMove)
        {
            PiecesMoved();
        }
    }

    public void MoveGameObjectPieces(List<GameObject> pieces)
    {
        foreach (GameObject piece in pieces)
        {
            piece.transform.DOMove(new Vector3(piece.transform.position.x, piece.transform.position.y + _boardUpHeight, 0f), upTime).OnComplete(() => MovedUpPieces(pieces));
        }
    }

    public void MoveGameObjectTiles(List<GameObject> tiles)
    {
        foreach (GameObject tile in tiles)
        {
            tile.transform.DOMove(new Vector3(tile.transform.position.x, tile.transform.position.y + _boardUpHeight, 0f), upTime).OnComplete(() => MovedUpTiles(tiles));
        }
    }

    private void MovedUpPieces(List<GameObject> pieces)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            Destroy(pieces[i]);
        }
    }

    private void MovedUpTiles(List<GameObject> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            Destroy(tiles[i]);
        }
    }

    public void MoveAllTilesUp()
    {
        for (int j = 0; j < _height; j++)
        {
            for (int i = 0; i < _width; i++)
            {
                if (_boardManager.GetTiles()[i, j] != null)
                {
                    _boardManager.GetTiles()[i, j].transform.DOMove(_boardManager.GetTiles()[i, j].futurePositions[0].position, upTime).OnComplete(() => MovedAllTilesUp());
                }
            }
        }
    }

    private void MovedAllTilesUp()
    {
        _movedTiles++;

        for (int j = 0; j < _height; j++)
        {
            for (int i = 0; i < _width; i++)
            {
                _boardManager.GetTiles()[i, j].futurePositions.Clear();
            }
        }

        if (_movedTiles == _width * _height)
        {
            _movedTiles = 0;
            BoardMovedUp();
        }
    }

    private void MovedAllPiecesUp()
    {
        for (int j = 0; j < _height; j++)
        {
            for (int i = 0; i < _width; i++)
            {
                if (_boardManager.GetPieces()[i, j] != null)
                {
                    _boardManager.GetPieces()[i, j].futurePositions.Clear();
                }
            }
        }
    }

    public void MoveAllPiecesUp()
    {
        for (int j = _height - 1; j >= 0; j--)
        {
            for (int i = 0; i < _width; i++)
            {
                if (_boardManager.GetPieces()[i, j] != null)
                {
                    _boardManager.GetPieces()[i, j].transform.DOMove(_boardManager.GetPieces()[i, j].futurePositions[0].position, upTime).OnComplete(() => MovedAllPiecesUp());
                }
            }
        }
    }

    public void MoveAll(Piece[,] pieces, bool shuffle)
    {
        _piecesToMove = 0;
        _movedPieces = 0;

        for (int j = 0; j < _height; j++)
        {
            for (int i = 0; i < _width; i++)
            {
                if (_boardManager.GetPieces()[i, j] != null)
                {
                    if (_boardManager.GetPieces()[i, j].futurePositions.Count > 0)
                    {
                        if (!shuffle)
                        {
                            StartCoroutine(MoveRoutine(_boardManager.GetPieces()[i, j], _boardManager.GetPieces()[i, j].speedTime));
                        }
                        else
                        {
                            MoveSuffle(_boardManager.GetPieces()[i, j]);
                        }
                    }
                }
            }
        }
    }

    private void MoveSuffle(Piece piece)
    {
        _piecesToMove++;

        Sequence s = DOTween.Sequence();
        s.SetEase(Ease.InSine);
        s.OnComplete(() => Ended(piece, true));

        for (int p = 0; p < piece.futurePositions.Count; p++)
        {
            s.Insert(p * 0.3f, piece.transform.DOMove(piece.futurePositions[p].position, shuffleTime));
        }
    }

    private IEnumerator MoveRoutine(Piece piece, float delay)
    {
        _piecesToMove++;

        yield return new WaitForSeconds(delay);

        Sequence s = DOTween.Sequence();
        s.SetEase(Ease.InSine);
        s.OnComplete(() => Ended(piece, false));

        for (int p = 0; p < piece.futurePositions.Count; p++)
        {
            //was fallTime / 2f
            s.Insert(p * fallTime / 4f, piece.transform.DOMove(piece.futurePositions[p].position, fallTime));
        }
    }
}

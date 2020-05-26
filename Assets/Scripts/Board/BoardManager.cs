using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager
{
    public static Action OnDeselected = delegate { };
    public static Action<int> OnScoreCounted = delegate { };


    private Piece[,] _pieces;
    private Tile[,] _tiles;
    private PieceAnimator _pieceAnimator;
    private Matcher _matcher;
    private List<GameObject> _piecePrefabs;

    private readonly Transform _piecesParent;
    private readonly Transform _tilesParent;
    private readonly Transform _maskParent;

    private GameObject _tileObstacle;
    private GameObject _tileLight;
    private GameObject _tileDark;
    private GameObject _tileHorizon;
    private GameObject _tileMaskPrefab;

    private int _width;
    private int _height;
    private int _obstaclesHeight;
    private int _horizonHeight;
    private int _boardUpHeight;
    private int _colorCount;

    private Tile _clickedTile;
    private Tile _targetTile;
    private Piece _clickedPiece;
    private bool bombActive = false;

    public bool _canTouch = true;

    public BoardManager(int width, int height, PieceAnimator pieceAnimator, GameObject tileObstacle, List<GameObject> piecePrefabs, GameObject tileLight, GameObject tileDark, GameObject tileMaskPrefab, GameObject tileHorizon, int obstaclesHeight, int horizonHeight, int boardUpHeight, int colorCount)
    {
        _piecePrefabs = new List<GameObject>();
        _pieceAnimator = pieceAnimator;
        _matcher = new Matcher(width, height);

        _width = width;
        _height = height;

        _tileObstacle = tileObstacle;
        _tileLight = tileLight;
        _tileDark = tileDark;
        _tileMaskPrefab = tileMaskPrefab;
        _piecePrefabs = piecePrefabs;
        _tileHorizon = tileHorizon;
        _boardUpHeight = boardUpHeight;
        _colorCount = colorCount;

        _piecesParent = new GameObject("Pieces").transform;
        _tilesParent = new GameObject("Tiles").transform;
        _maskParent = new GameObject("Mask").transform;

        _piecesParent.transform.SetParent(pieceAnimator.transform);
        _tilesParent.transform.SetParent(pieceAnimator.transform);
        _maskParent.transform.SetParent(pieceAnimator.transform);

        _obstaclesHeight = obstaclesHeight;
        _horizonHeight = horizonHeight;

        SetupTiles();
        FillBoard();
        SpawnMaskTiles();
        //MakeHorizon();
    }


    //Getters

    public Piece[,] GetPieces()
    {
        return _pieces;
    }

    public Tile[,] GetTiles()
    {
        return _tiles;
    }

    public List<GameObject> GetUpperPieces()
    {
        List<GameObject> pieces = new List<GameObject>();
        for (int j = _boardUpHeight; j > 0; j--)
        {
            for (int i = 0; i < _width; i++)
            {
                pieces.Add(_pieces[i, _height - j].gameObject);
            }
        }

        return pieces;
    }

    public List<GameObject> GetUpperTiles()
    {
        List<GameObject> tiles = new List<GameObject>();
        for (int j = _boardUpHeight; j > 0; j--)
        {
            for (int i = 0; i < _width; i++)
            {
                tiles.Add(_tiles[i, _height - j].gameObject);
            }
        }

        return tiles;
    }


    //Checkers

    public bool HorizonFree()
    {
        int counter = 0;

        for (int i = 0; i < _width; i++)
        {
            if (_tiles[i, _horizonHeight].tileType == TileType.Normal)
            {
                counter++;
            }
        }

        if (counter == _width)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0 && y < _height);
    }

    private bool IsNextTo(Tile start, Tile end)
    {
        if (Mathf.Abs(start.xIndex - end.xIndex) == 1 && start.yIndex == end.yIndex)
        {
            return true;
        }

        if (Mathf.Abs(start.yIndex - end.yIndex) == 1 && start.xIndex == end.xIndex)
        {
            return true;
        }

        return false;
    }


    //System

    public void SpawnDownTiles()
    {
        List<GameObject> tiles = new List<GameObject>();
        for (int j = -_boardUpHeight; j < 0; j++)
        {
            for (int i = 0; i < _width; i++)
            {
                GameObject tile = GameObject.Instantiate(_tileObstacle, new Vector3(i, j), Quaternion.identity, _tilesParent);
                tile.name = "Tile (" + i + "," + _boardUpHeight + j + ")";
                _tiles[i, _boardUpHeight + j] = tile.GetComponent<Tile>();
                _tiles[i, _boardUpHeight + j].Init(i, _boardUpHeight + j, this, _width, _height);
                _tiles[i, _boardUpHeight + j].futurePositions.Clear();
                _tiles[i, _boardUpHeight + j].futurePositions.Add(new MovePoint(new Vector3(i, _boardUpHeight + j, 0f), new Vector2(0f, 0f), 0f));
            }
        }
    }

    public void SetDelay()
    {
        for (int i = 0; i < _width; i++)
        {
            float delay = 0f;
            for (int j = 0; j < _height; j++)
            {
                if (_pieces[i, j] != null)
                {
                    if (_pieces[i, j].futurePositions.Count > 0)
                    {
                        if (!_pieces[i, j].marked)
                        {
                            if (_pieces[i, j].speedTime == 0f)
                            {
                                _pieces[i, j].speedTime = delay;
                                delay += 0.15f;
                            }
                        }
                    }
                }
            }
        }
    }

    public List<Piece> ClearBoard(List<Piece> pieces)
    {
        BreakObstacleTiles(pieces);

        List<Piece> piecesToDelete = new List<Piece>();

        foreach (Piece piece in pieces)
        {
            piecesToDelete.Add(piece);
        }

        foreach (Piece piece in piecesToDelete)
        {
            _pieces[piece.xIndex, piece.yIndex] = null;
            _canTouch = false;
        }

        OnScoreCounted(piecesToDelete.Count);

        return piecesToDelete;
    }

    public void OnTileBecameEmptyRow(List<float> times)
    {
        List<Tile> empty = new List<Tile>();

        for (int i = 0; i < _width; i++)
        {
            if ((_pieces[i, _height - 1] == null && _tiles[i, _height - 1].tileType != TileType.Obstacle))
            {
                empty.Add(_tiles[i, _height - 1]);
            }
        }

        if (times.Count == empty.Count)
        {
            for (int i = 0; i < empty.Count; i++)
            {
                int heigth = 0;
                heigth = _height - 1;

                Piece piece = FillRandomAt(_piecePrefabs[UnityEngine.Random.Range(0, _colorCount)], empty[i].xIndex, heigth, 1);
                piece.futurePositions.Add(new MovePoint(new Vector3(empty[i].xIndex, heigth, 0f), new Vector2(0f, 0f), 0f));
                piece.speedTime = times[i];
                _pieces[empty[i].xIndex, heigth] = piece;
                _pieces[empty[i].xIndex, heigth].marked = true;
            }
        }
        else
        {
            Debug.Log("Не совпало!");
        }
    }

    private void BreakObstacleTiles(List<Piece> pieces)
    {
        foreach (Piece piece in pieces)
        {
            if (IsWithinBounds(piece.xIndex - 1, piece.yIndex))
            {
                _tiles[piece.xIndex - 1, piece.yIndex].BreakObstacleTile(ref _tiles);
            }

            if (IsWithinBounds(piece.xIndex + 1, piece.yIndex))
            {
                _tiles[piece.xIndex + 1, piece.yIndex].BreakObstacleTile(ref _tiles);
            }

            if (IsWithinBounds(piece.xIndex, piece.yIndex - 1))
            {
                _tiles[piece.xIndex, piece.yIndex - 1].BreakObstacleTile(ref _tiles);
            }

            if (IsWithinBounds(piece.xIndex, piece.yIndex + 1))
            {
                _tiles[piece.xIndex, piece.yIndex + 1].BreakObstacleTile(ref _tiles);
            }
        }
    }

    public void AddColor()
    {
        _colorCount++;
    }


    //Creating board

    public void FillBoard()
    {
        if (_pieces == null)
        {
            _pieces = new Piece[_width, _height];
        }

        GameObject[] previousLeft = new GameObject[_height];
        GameObject previousBelow = null;

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_pieces[i, j] == null)
                {
                    if (_tiles[i, j].tileType != TileType.Obstacle)
                    {
                        List<GameObject> possiblePieces = new List<GameObject>();

                        for (int c = 0; c < _colorCount; c++)
                        {
                            possiblePieces.Add(_piecePrefabs[c]);
                        }

                        possiblePieces.Remove(previousLeft[j]);
                        possiblePieces.Remove(previousBelow);

                        GameObject piecePrefab = possiblePieces[UnityEngine.Random.Range(0, possiblePieces.Count)];
                        previousLeft[j] = piecePrefab;
                        previousBelow = piecePrefab;

                        Piece piece = FillRandomAt(piecePrefab, i, j);
                        _pieces[i, j] = piece;
                    }
                }
            }
        }
    }

    public void FillBoardFromList(List<Piece> pieces)
    {
        Queue<Piece> shuffled = new Queue<Piece>(pieces);

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_pieces[i, j] == null && _tiles[i, j].tileType != TileType.Obstacle)
                {
                    _pieces[i, j] = shuffled.Dequeue();
                    _pieces[i, j].SetCoords(i, j);
                }
            }
        }
    }

    private void PlacePiece(Tile clickedTile, Tile targetTile, bool back)
    {
        Piece clickedPiece = _pieces[clickedTile.xIndex, clickedTile.yIndex];
        Piece targetPiece = _pieces[targetTile.xIndex, targetTile.yIndex];

        if (clickedPiece != null && targetPiece != null)
        {
            _canTouch = false;
            _pieces[clickedTile.xIndex, clickedTile.yIndex] = targetPiece;
            _pieces[targetPiece.xIndex, targetPiece.yIndex] = clickedPiece;

            clickedPiece.SetCoords(targetTile.xIndex, targetTile.yIndex);
            targetPiece.SetCoords(clickedTile.xIndex, clickedTile.yIndex);

            _clickedPiece = _pieces[targetTile.xIndex, targetTile.yIndex];

            _pieceAnimator.MovePieces(_pieces, clickedTile, targetTile, back);
        }
    }

    private void MakeHorizon()
    {
        for (int i = 0; i < _width; i++)
        {
            GameObject.Instantiate(_tileHorizon, new Vector3(i, _horizonHeight - 0.5f), Quaternion.identity, _tilesParent);
        }
    }

    private void MakeTile(GameObject prefab, int x, int y)
    {
        if (prefab != null)
        {
            GameObject tile = GameObject.Instantiate(prefab, new Vector3(x, y), Quaternion.identity, _tilesParent);
            tile.name = "Tile (" + x + "," + y + ")";

            _tiles[x, y] = tile.GetComponent<Tile>();

            if (prefab == _tileDark)
            {
                _tiles[x, y].tileColor = 1;
            }
            else
            {
                _tiles[x, y].tileColor = 0;
            }
            _tiles[x, y].Init(x, y, this, _width, _height);
        }
        else
        {
            Debug.Log("No prefab setted");
        }
    }

    private void SetupTiles()
    {
        _tiles = new Tile[_width, _height];

        //Использовать для заполнения нижнего ряда обстаклами
        //if (_obstaclesHeight > 0)
        //{
        //    for (int i = 0; i < _width; i++)
        //    {
        //        for (int j = 0; j < _obstaclesHeight; j++)
        //        {
        //            MakeTile(_tileObstacle, i, j);
        //        }
        //    }
        //}

        for (int i = 0; i < _width; i++)
        {
            bool evenRow = false;
            for (int j = 0; j < _height; j++)
            {
                if (j % 2 == 0)
                {
                    evenRow = true;
                }
                else
                {
                    evenRow = false;
                }

                if (_tiles[i, j] == null)
                {
                    if (evenRow)
                    {
                        if (i % 2 == 0)
                        {
                            MakeTile(_tileDark, i, j);
                        }
                        else
                        {
                            MakeTile(_tileLight, i, j);
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            MakeTile(_tileLight, i, j);
                        }
                        else
                        {
                            MakeTile(_tileDark, i, j);
                        }
                    }
                }
            }
        }
    }

    private void SpawnMaskTiles()
    {
        for (int j = 1; j <= _boardUpHeight; j++)
        {
            for (int i = 0; i < _width; i++)
            {
                GameObject.Instantiate(_tileMaskPrefab, new Vector3(i, _height - 1 + j, 0f), Quaternion.identity, _maskParent);
                GameObject.Instantiate(_tileMaskPrefab, new Vector3(i, -j, 0f), Quaternion.identity, _maskParent);
            }
        }
    }

    public void PlacePiece(Piece piece, int x, int y, int offset = 0)
    {
        if (piece == null)
        {
            Debug.Log("Board: Invalid Piece");
        }

        if (offset > 0)
        {
            piece.transform.position = new Vector3(x, y + offset, 0f);
        }
        else
        {
            piece.transform.position = new Vector3(x, y, 0f);
        }

        piece.transform.rotation = Quaternion.identity;
        if (IsWithinBounds(x, y))
        {
            _pieces[x, y] = piece;
        }
        piece.SetCoords(x, y);
    }

    private Piece FillRandomAt(GameObject prefab, int x, int y, int offset = 0)
    {
        GameObject randomPiece = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, _piecesParent.transform);
        if (randomPiece != null)
        {
            randomPiece.GetComponent<Piece>().Init(this);
            PlacePiece(randomPiece.GetComponent<Piece>(), x, y, offset);
            return randomPiece.GetComponent<Piece>();
        }
        return null;
    }

    //BOMB

    public void ActiveteBomb(bool enabled)
    {
        bombActive = enabled;
    }

    public bool BombIsActive()
    {
        return bombActive;
    }

    private List<Piece> RoundBomb(Tile tile)
    {
        int x = tile.xIndex;
        int y = tile.yIndex;

        List<Piece> piecesToDelete = new List<Piece>();

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (IsWithinBounds(i, j))
                {
                    if (_pieces[i, j] != null)
                    {
                        piecesToDelete.Add(_pieces[i, j]);
                    }
                }
            }
        }
        return piecesToDelete;
    }

    private List<Piece> CrossBomb(Tile tile)
    {
        int x = tile.xIndex;
        int y = tile.yIndex;

        List<Piece> piecesToDelete = new List<Piece>();

        for (int i = 0; i < _width; i++)
        {
            if (_pieces[i, y] != null)
            {
                piecesToDelete.Add(_pieces[i, y]);
            }
        }

        for (int j = 0; j < _height; j++)
        {
            if (_pieces[x, j] != null)
            {
                piecesToDelete.Add(_pieces[x, j]);
            }
        }

        return piecesToDelete;
    }

    public List<Piece> BombExplosion(Tile tile)
    {
        List<Piece> piecesToDelete = new List<Piece>();

        if (_canTouch)
        {
            _canTouch = false;

            int rnd = UnityEngine.Random.Range(0, 2);

            if (rnd == 0)
            {
                piecesToDelete.AddRange(CrossBomb(tile));
            }
            else
            {
                piecesToDelete.AddRange(RoundBomb(tile));
            }

            return piecesToDelete;
        }

        return null;
    }

    //Touch callbacks

    public void ClickTile(Tile tile)
    {
        if (_canTouch)
        {
            if (_clickedTile == null)
            {
                if (bombActive)
                {
                    BombExplosion(_clickedTile);
                }
                else
                {
                    _clickedTile = tile;
                }
            }
        }
    }


    public void DragToTile(Tile tile)
    {
        if (_canTouch)
        {
            if (_clickedTile != null && IsNextTo(tile, _clickedTile))
            {
                _targetTile = tile;
            }
        }
    }

    public void RealiseTile()
    {
        if (_canTouch)
        {
            if (_clickedTile != null && _targetTile != null)
            {
                SwitchTiles(_clickedTile, _targetTile);
            }

            _clickedTile = null;
            _targetTile = null;
        }
    }

    public List<Piece> EndSwitchPieces(Tile clickedTile, Tile targetTile, bool back)
    {
        if (!back)
        {
            List<Piece> clickedPieceMatches = _matcher.FindMatchesAt(_pieces, _tiles, clickedTile.xIndex, clickedTile.yIndex);
            List<Piece> targetPieceMatches = _matcher.FindMatchesAt(_pieces, _tiles, targetTile.xIndex, targetTile.yIndex);

            if (clickedPieceMatches.Count > 0 || targetPieceMatches.Count > 0)
            {
                var piecesToDelete = clickedPieceMatches.Union(targetPieceMatches).ToList();
                return piecesToDelete;
            }
            else
            {
                PlacePiece(clickedTile, targetTile, true);
            }
        }
        else
        {
            _canTouch = true;
        }
        return new List<Piece>();
    }

    private void SwitchTiles(Tile clickedTile, Tile targetTile)
    {
        if (_canTouch)
        {
            OnDeselected();
            PlacePiece(clickedTile, targetTile, false);
        }
    }
}

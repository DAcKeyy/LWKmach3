using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using System;
using TMPro;

public class Board : MonoBehaviour
{
    public static Action BoardUp = delegate { };

    [BoxGroup("TaskManager")]
    public TaskManager taskManager;

    [Space(10)]
    [BoxGroup("Board settings")]
    public int width = 7;
    [BoxGroup("Board settings")]
    public int height = 9;
    [BoxGroup("Board settings")]
    public float borderSize = 1;
    [BoxGroup("Board settings")]
    public int obstaclesHeight = 3;
    [BoxGroup("Board settings")]
    public int horizonHeight = 2;
    [BoxGroup("Board settings")]
    public int boardUpHeight = 2;
    [BoxGroup("Board settings")]
    public int colorCount = 4;
    [BoxGroup("Board settings")]
    public float cameraOffset = 1f;
    [BoxGroup("Board settings")]
    [ReadOnly]
    public int _diggerDeep = 0;


    [BoxGroup("Tile prefabs")]
    [ShowAssetPreview(32, 32)]
    public GameObject tileLight;
    [BoxGroup("Tile prefabs")]
    [ShowAssetPreview(32, 32)]
    public GameObject tileDark;
    [BoxGroup("Tile prefabs")]
    [ShowAssetPreview(32, 32)]
    public GameObject tileObstacle;
    [BoxGroup("Tile prefabs")]
    [ShowAssetPreview(32, 32)]
    public GameObject tileHorizon;
    [BoxGroup("Tile prefabs")]
    [ShowAssetPreview(32, 32)]
    public GameObject tileMaskPrefab;

    [ReorderableList]
    public List<GameObject> piecePrefabs;

    private PieceAnimator _pieceAnimator;
    private ModMover _modMover;
    private Matcher _matcher;
    private BoardManager _boardManager;
    private BoardDeadlock _boardDeadlock;
    private BoardShuffler _boardShuffler;
    private UpTimer _upTimer;

    private void OnEnable()
    {
        PieceAnimator.PiecesSwitched += PiecesSwitched;
        PieceAnimator.PiecesMoved += PiecesMoved;
        PieceAnimator.PiecesDeleted += PiecesDeleted;
        Tile.OnSelected += BombExplosion;
        //PieceAnimator.BoardMovedUp += BoardMovedUp;

        //Board.BoardUp += AddLvl;
    }

    private void OnDisable()
    {
        PieceAnimator.PiecesSwitched -= PiecesSwitched;
        PieceAnimator.PiecesMoved -= PiecesMoved;
        PieceAnimator.PiecesDeleted -= PiecesDeleted;
        Tile.OnSelected -= BombExplosion;

        //PieceAnimator.BoardMovedUp -= BoardMovedUp;

        //Board.BoardUp += AddLvl;
    }

    public void Restart()
    {
        if (_boardManager._canTouch)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (GameObject.Find("Tiles"))
        {
            Destroy(GameObject.Find("Tiles").gameObject);
            Destroy(GameObject.Find("Pieces").gameObject);
            Destroy(GameObject.Find("Mask").gameObject);
        }

        SetupCamera();
        taskManager.Init();


        _matcher = new Matcher(width, height);
        _upTimer = GetComponent<UpTimer>();
        _pieceAnimator = GetComponent<PieceAnimator>();
        _boardManager = new BoardManager(
                                        width, height, _pieceAnimator,
                                        tileObstacle, piecePrefabs, tileLight,
                                        tileDark, tileMaskPrefab, tileHorizon,
                                        obstaclesHeight, horizonHeight, boardUpHeight,
                                        colorCount
                                        );

        _modMover = new ModMover(width, height, _boardManager, boardUpHeight);
        _pieceAnimator.Init(width, height, _boardManager, boardUpHeight);
        _pieceAnimator.SetModMover(_modMover);
        _boardDeadlock = new BoardDeadlock(width, height);
        _boardShuffler = new BoardShuffler(width, height, _boardManager);

        transform.position = new Vector3(-width - borderSize, 0f, 0f);
        StartCoroutine(MoveBoardRoutine());
    }

    private IEnumerator MoveBoardRoutine()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        transform.DOMove(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutCubic).OnComplete(() => DeadLockOnStart());
    }

    private void DeadLockOnStart()
    {
        if (_boardDeadlock.IsDeadlocked(_boardManager.GetPieces(), 3))
        {
            Debug.Log("DeadLocked");
            Shuffle();
            _boardManager._canTouch = false;
        }
    }

    private void Start()
    {
        StartGame();
        //AddLvl();
    }

    private void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((width - 1) / 2f, (height - 1) / 2f, -10f);
        float aspectRatio = Screen.width / (float)Screen.height;
        float verticalSize = height / 2f + borderSize;
        float horizontalSize = (width / 2f + borderSize) / aspectRatio;
        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + cameraOffset, Camera.main.transform.position.z);
    }


    private void PiecesSwitched(Tile clickedTile, Tile targetTile, bool back)
    {
        List<Piece> piecesToDelete = _boardManager.EndSwitchPieces(clickedTile, targetTile, back);

        if (piecesToDelete.Count > 0)
        {
            taskManager.CheckTask(piecesToDelete);
            List<Piece> pieces = _boardManager.ClearBoard(piecesToDelete);
            _pieceAnimator.DeleteAnimation(_boardManager.GetTiles(), pieces);
        }
    }

    private void PiecesDeleted(List<Tile> deletedTiles)
    {
        Piece[,] pieces = _modMover.MovePiecesDown();
        _pieceAnimator.MoveAll(pieces, false);
    }

    public void MoveBoardUp()
    {
        List<GameObject> upperPieces = _boardManager.GetUpperPieces();
        List<GameObject> upperTiles = _boardManager.GetUpperTiles();


        Piece[,] pieces = _modMover.MovePiecesUp();
        Tile[,] tiles = _modMover.MoveTilesUp();

        _boardManager.SpawnDownTiles();

        _pieceAnimator.MoveGameObjectPieces(upperPieces);
        _pieceAnimator.MoveGameObjectTiles(upperTiles);
        _pieceAnimator.MoveAllPiecesUp();
        _pieceAnimator.MoveAllTilesUp();
    }

    public void Shuffle()
    {
        if (_boardManager != null)
        {
            if (_boardShuffler != null && _boardManager._canTouch)
            {
                _boardManager._canTouch = false;
                StartCoroutine(ShuffleRoutine());
            }
        }
    }

    ////////////////////////BOMB///////////////////////////

    public void ActivateBomb()
    {
        _boardManager.ActiveteBomb(true);
    }

    private void BombExplosion(Tile tile)
    {
        if (_boardManager._canTouch)
        {
            if (_boardManager.BombIsActive())
            {
                List<Piece> piecestodel = _boardManager.BombExplosion(tile);
                List<Piece> pieces = _boardManager.ClearBoard(piecestodel);
                _pieceAnimator.DeleteAnimation(_boardManager.GetTiles(), pieces);
                _boardManager.ActiveteBomb(false);
            }
        }
    }


    ////////////////////////BOMB///////////////////////////

    private IEnumerator ShuffleRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        _boardShuffler.Shuffle();
        _pieceAnimator.MoveAll(_boardManager.GetPieces(), true);
    }

    private void BoardMovedUp()
    {
        if (_boardManager.HorizonFree())
        {
            _boardManager._canTouch = false;
            MoveBoardUp();
        }
        else
        {
            _diggerDeep++;
            _upTimer.AddTime();
            BoardUp();
            _boardManager._canTouch = true;
        }
    }

    private void PiecesMoved()
    {
        List<Piece> matches = _matcher.FindAllMatches(_boardManager.GetPieces(), _boardManager.GetTiles());
        _boardManager._canTouch = true;
        if (matches.Count > 0)
        {
            _boardManager._canTouch = false;
            List<Piece> piecestodel = _boardManager.ClearBoard(matches);
            _pieceAnimator.DeleteAnimation(_boardManager.GetTiles(), piecestodel);
        }
        else
        {
            if (_boardDeadlock.IsDeadlocked(_boardManager.GetPieces(), 3))
            {
                Debug.Log("DeadLocked");
                Shuffle();
                _boardManager._canTouch = false;
            }
            else
            {
                //Использовать для подъема боарда
                //if (_boardManager.HorizonFree())
                //{
                //    _boardManager._canTouch = false;
                //    MoveBoardUp();
                //}
                //else
                //{
                _boardManager._canTouch = true;
                //}
            }
        }
    }

    //private void ScoreCount(int picesDeletedCount)
    //{
    //    var scoreText = Convert.ToInt32(ScoreText.text);

    //    scoreText += picesDeletedCount * 100; //Шаманить тут 

    //    ScoreText.text = Convert.ToString(scoreText);
    //}

    //private void AddLvl()
    //{
    //    var lvlText = Convert.ToInt32(LevelText.text);

    //    lvlText += 1;

    //    LevelText.text = Convert.ToString(lvlText);
    //}
}

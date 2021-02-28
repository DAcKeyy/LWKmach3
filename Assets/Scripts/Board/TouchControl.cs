using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControl : MonoBehaviour
{
    public static Action<Tile> OnSelected = delegate { };

    public BoardManager _board;
    private Collider2D col;
    private bool _canTouch = true;

    private void HandleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector3.forward, 50);
                col = hit.collider;

                if (col)
                {
                    Tile currentTile = hit.collider.GetComponent<Tile>();
                    OnSelected(currentTile);
                    _board.ClickTile(currentTile);
                }
                break;

            case TouchPhase.Moved:
                if (_canTouch)
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector3.forward, 50);

                    if (hit2.collider != col)
                    {
                        _board.DragToTile(hit2.collider.GetComponent<Tile>());
                        _board.RealiseTile();
                        _canTouch = false;
                    }
                }
                break;

            case TouchPhase.Ended:
                _canTouch = true;
                break;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            HandleTouch(Input.GetTouch(0));
        }
    }
}

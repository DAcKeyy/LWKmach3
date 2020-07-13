using DG.Tweening;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System;

public enum Vector
{
    None,
    Up,
    Down,
    Right,
    Left
}

public class UISlidePanels : MonoBehaviour
{
    public static Action<RectTransform> UISlidePanelEnded;

    [SerializeField] Vector MovingVector = 0;
    [SerializeField] Ease Ease = Ease.Linear;
    [SerializeField] float Speed = 1;
    [SerializeField] [Slider(0f, 1f)] float SlideValue = 1;

    private RectTransform PanelTransform;
    private Vector2 TargetPosition;
    private Vector2 StartPosition;
    private bool isMooving = false;
    private bool isClosed = false;

    private void Start()
    {
        PanelTransform = gameObject.GetComponent<RectTransform>();
        StartPosition = PanelTransform.position;

        TargetPosition = GetTargetPosition(MovingVector, PanelTransform);

    }

    private Vector2 GetTargetPosition(Vector movingVector, RectTransform objectTransform )
    {
        Vector2 targetVector = new Vector2(objectTransform.position.x, objectTransform.position.y);

        switch (movingVector)
        {
            case Vector.Up:
                targetVector.y += (objectTransform.sizeDelta.y - objectTransform.sizeDelta.y * SlideValue);
                break;

            case Vector.Down:
                targetVector.y -= (objectTransform.sizeDelta.y - objectTransform.sizeDelta.y * SlideValue);
                break;

            case Vector.Left:
                targetVector.x -= (objectTransform.sizeDelta.x - objectTransform.sizeDelta.x * SlideValue);
                break;

            case Vector.Right:
                targetVector.x += (objectTransform.sizeDelta.x - objectTransform.sizeDelta.x * SlideValue);
                break;

            case Vector.None:
                targetVector.x = objectTransform.rect.width;
                targetVector.y = objectTransform.rect.height;
                return targetVector;
        }
        return targetVector;
    }

    public void Move()
    {
        isMooving = true;
        isClosed = !isClosed;

        StartCoroutine(Tweening());
    }

    private IEnumerator Tweening()
    {
        if(isClosed) PanelTransform.DOAnchorPos(TargetPosition, Speed).SetEase(Ease).OnComplete(() => UISlidePanelEnded?.Invoke(PanelTransform));
        else PanelTransform.DOAnchorPos(StartPosition, Speed).SetEase(Ease).OnComplete(() => UISlidePanelEnded?.Invoke(PanelTransform));

        yield return null;
    }
}
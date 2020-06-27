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
    [SerializeField] Ease Ease;
    [SerializeField] float Speed;
    [Slider(0f, 1f)] float SlideValue;

    private RectTransform PanelTransform;
    private Vector2 TargetPosition;
    private bool isMooving = false;

    private void Start()
    {
        PanelTransform = gameObject.GetComponent<RectTransform>();

        Debug.Log(PanelTransform.rect.width);
        Debug.Log(PanelTransform.rect.height);

        TargetPosition = GetTargetPosition(MovingVector, PanelTransform);
    }

    private Vector2 GetTargetPosition(Vector movingVector, RectTransform objectTransform )
    {
        Vector2 targetVector = new Vector2();

        switch (movingVector)
        {
            case Vector.Up:
                targetVector.y += (PanelTransform.rect.height - PanelTransform.rect.height * SlideValue);
                break;

            case Vector.Down:
                targetVector.y -= (PanelTransform.rect.height - PanelTransform.rect.height * SlideValue);
                break;

            case Vector.Left:
                targetVector.x -= (PanelTransform.rect.width - PanelTransform.rect.width * SlideValue);
                break;

            case Vector.Right:
                targetVector.x += (PanelTransform.rect.width - PanelTransform.rect.width * SlideValue);
                break;

            case Vector.None:
                targetVector.x = PanelTransform.rect.width;
                targetVector.y = PanelTransform.rect.height;
                return targetVector;
        }
        Debug.Log(targetVector.x+ "///"+ targetVector.y);
        return targetVector;
    }

    public void Move()
    {
        isMooving = true;

        StartCoroutine(Tweening());
    }

    private void StopMoving()
    {

    }

    private IEnumerator Tweening()
    {
        //PanelTransform.DOMove(TargetPosition, Speed).SetEase(Ease).OnComplete(() => UISlidePanelEnded(PanelTransform));

        yield return null;
    }

}

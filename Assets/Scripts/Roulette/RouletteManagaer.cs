using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RouletteManagaer : MonoBehaviour
{
    [SerializeField] GameObject WinGiftPanel = null;
    [SerializeField] GameObject WeelPanel = null;

    private Vector2 StartPosition;

    private void Start()
    {
        StartPosition = WeelPanel.transform.position;
        Appear();
    }

    private void OnEnable()
    {
        RouletteMover.EndRotate += Disappear;
    }
    private void OnDisable()
    {
        RouletteMover.EndRotate -= Disappear;
    }

    #region Appear
    public void Appear()
    {
        WeelPanel.transform.DOMove(Vector2.zero, 0.25F);
    }

    #endregion
    #region Disappear
    private void Disappear()
    {
        WeelPanel.transform.DOMove(StartPosition, 0.25F);
    }

    #endregion

    private void ActivateWinGift()
    {

    }

    public void SetActive()
    {

    }
}

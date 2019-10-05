using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RouletteManagaer : MonoBehaviour
{
    [SerializeField] GameObject WinGiftPanel = null;
    [SerializeField] GameObject WeelPanel = null;


    private int WinSector = 0;
    private Vector2 StartPosition;

    private void Start()
    {
        StartPosition = WeelPanel.transform.position;
        Appear();
    }

    private void OnEnable()
    {
        RouletteMover.EndRotate += Disappear;
        RouletteMover.StartRotate += Moving;
    }
    private void OnDisable()
    {
        RouletteMover.EndRotate -= Disappear;
        RouletteMover.StartRotate -= Moving;
    }

    #region Appear
    public void Appear()
    {
        WeelPanel.transform.Find("Panel").GetComponent<Image>().raycastTarget = false;
        WeelPanel.transform.DOMove(Vector2.zero, 0.25F);
    }

    #endregion
    private void Moving()
    {
        WeelPanel.transform.Find("Panel").GetComponent<Image>().raycastTarget = true;
    }
    #region Disappear
    private void Disappear(int Sector)
    {
        WinSector = Sector;
        WeelPanel.transform.DOMove(StartPosition, 0.25F);
        ActivateWinGift();
    }

    #endregion

    private void ActivateWinGift()
    {
        var Text = WinGiftPanel.transform.Find("Panel/Text").gameObject;
        Text.GetComponent<TMP_Text>().text = "Your prise is\n" + WinSector;

        WinGiftPanel.transform.DOMove(Vector2.zero, 0.25F);
    }

    public void EndWinGift()
    {
        WinGiftPanel.transform.DOMove(StartPosition, 0.25F);
    }
}

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
    }

    private void OnEnable()
    {
        RouletteMover.StartRotate += Moving;
        RouletteMover.EndRotate += Disappear;
    }
    private void OnDisable()
    {
        RouletteMover.StartRotate -= Moving;
        RouletteMover.EndRotate -= Disappear;
    }

    #region Whole Visual Process

    public void Appear()
    {
        WeelPanel.transform.Find("Panel").GetComponent<Image>().raycastTarget = false;
        WeelPanel.transform.DOMove(Vector2.zero, 0.25F);
    }


    private void Moving()
    {
        WeelPanel.transform.Find("Panel").GetComponent<Image>().raycastTarget = true;
    }

    private void Disappear(int Sector)
    {
        WinSector = Sector;
        WeelPanel.transform.DOMove(StartPosition, 0.25F);
        ActivateWinGift();
    }



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

#endregion
}

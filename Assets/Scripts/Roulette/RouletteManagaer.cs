using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using DG.Tweening;

public class RouletteManagaer : MonoBehaviour
{
    [SerializeField] GameObject WinGiftPanel = null;
    [SerializeField] GameObject WeelPanel = null;

    private int WinSector = 0;
    private Vector2 StartPosition;

    WebSender Sender = new WebSender();

    //public static Action ResetWeelSlider;

    private void Start() 
    {
        StartPosition = WeelPanel.transform.position;
    }

    private void OnEnable()
    {
        //WeelSlider.SendRequestForSpin += SendingRquest;

        RouletteMover.StartRotate += Moving;
        RouletteMover.EndRotate += Disappear;
        RouletteMover.EndRotate += ActivateWinGift;
    }
    private void OnDisable()
    {
        //WeelSlider.SendRequestForSpin -= SendingRquest;

        RouletteMover.StartRotate -= Moving;
        RouletteMover.EndRotate -= Disappear;
        RouletteMover.EndRotate -= ActivateWinGift;
    }

    public void SendingRquest()
    {
        var webRequest = UnityWebRequest.Get(URLStruct.SpinWeel);

        StartCoroutine(Sender.SendWebRequest(webRequest, Response, Errors));

    }

    void Response(string value)
    {

        Debug.Log(value);

        //ResetWeelSlider();
    }

    void Errors(string value)
    {
        Debug.Log(value);


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

    public void Disappear(int Sector)
    {
        WinSector = Sector;
        WeelPanel.transform.DOMove(StartPosition, 0.25F);
    }

    private void ActivateWinGift(int Sector)
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

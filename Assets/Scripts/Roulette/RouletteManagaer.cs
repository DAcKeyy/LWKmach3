using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;

public class RouletteManagaer : MonoBehaviour
{
    [SerializeField] GameObject WinGiftPanel = null;
    [SerializeField] GameObject WeelPanel = null;
    [SerializeField] GameObject PanelWithText = null;
    [SerializeField] GameObject CoinsPanel = null;
    [SerializeField] GameObject Weel = null;
    [SerializeField] GameObject CouponsManger_EventSystem = null;
    

    private string Win;
    private Vector2 StartPosition;

    WebSender Sender = new WebSender();
    Сoupon сoupon = new Сoupon();
    //public static Action ResetWeelSlider;

    private void Start() 
    {
        StartPosition = WeelPanel.transform.position;
    }

    private void OnEnable()
    {
        //WeelSlider.SendRequestForSpin += SendingRquest;
        RouletteMover.EndRotate += Disappear;
        RouletteMover.EndRotate += ActivateWinGift;
    }
    private void OnDisable()
    {
        //WeelSlider.SendRequestForSpin -= SendingRquest;
        RouletteMover.EndRotate -= Disappear;
        RouletteMover.EndRotate -= ActivateWinGift;
    }

    public void SendingRquest()
    {
        WeelPanel.transform.Find("Panel").GetComponent<Image>().raycastTarget = true;

        var webRequest = UnityWebRequest.Get(URLStruct.SpinWeel);
        webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
        webRequest.SetRequestHeader("Authorization", "Bearer "+ GlobalDataBase.Token);
        StartCoroutine(Sender.SendWebRequest(webRequest, Response, Errors));
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="response"></param>
    void Response(string response)
    {
        Debug.Log(response);

        Regex Money = new Regex(@"meta");
        Regex Coupon = new Regex(@"coupon");

        if (!String.IsNullOrEmpty(response) && Money.IsMatch(response))
        {
            Debug.Log("Dat's money");

            var Obj = JsonUtility.FromJson<RoulleteResponse>(response);

            char[] mas = Obj.meta.title.ToCharArray();
            string NumberCoins = null;
            foreach (char number in mas)
            {
                if (char.IsDigit(number) == true)
                {
                    NumberCoins += number;
                }
            }

            Win = NumberCoins + " ";

            WinGiftText(Win);


            Moving("Coins " + NumberCoins);
        }

        if (String.IsNullOrEmpty(response) == false && Coupon.IsMatch(response))
        {
            Debug.Log("Dat's coupon");

            var Obj = JsonUtility.FromJson<CouponResponse>(response);

            сoupon.company_name = Obj.data.attributes.company;
            сoupon.contact = Obj.data.attributes.contact;
            сoupon.description = Obj.data.attributes.description;
            сoupon.promo = Obj.data.attributes.coupon;
            сoupon.expiration_date = Obj.data.attributes.expiration_date;

            CouponsManger_EventSystem.GetComponent<CouponsManger>().SaveCoupon(сoupon);

            Win = "Coupon\n" + сoupon.company_name + "\n" + сoupon.description;
            WinGiftText(Win);

            if(Convert.ToInt32(Obj.data.attributes.discount) < 5)
                Moving("Coupon usual");
            if (Convert.ToInt32(Obj.data.attributes.discount) > 5 && Convert.ToInt32(Obj.data.attributes.discount) < 15)
                Moving("Coupon uncommon");
            if (Convert.ToInt32(Obj.data.attributes.discount) > 15)
                Moving("Coupon rare");
        }

        
    }

    void ResponseToUpdateCoins(string Response)
    {
        var Obj = JsonUtility.FromJson<Me>(Response);
        GlobalDataBase.Gold = Convert.ToInt32(Obj.data.attributes.coin);
        GlobalDataBase.UserID = Convert.ToUInt32(Obj.data.id);

        CoinsPanel.GetComponent<CoinsPanel>().UpdateText();
    }

    void ErrorsFromUpdateCoins(string Response)
    {
        Debug.Log(Response);
    }

    void Errors(string Response)
    {
        WeelPanel.transform.Find("Panel").GetComponent<Image>().raycastTarget = false;

        Debug.Log(Response);

        Regex SpinsLocked = new Regex(@"You have already received a coupon today");
        if (String.IsNullOrEmpty(Response) == false && SpinsLocked.IsMatch(Response))
        {
            PanelWithText.SetActive(true);
            var text = PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
            text.text = "You have reached your scroll limit for today.";
        }
    }

#region Whole Visual Process
    public void Appear()
    {
        var webRequest = UnityWebRequest.Get(URLStruct.GetCoin);
        webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + GlobalDataBase.Token);
        StartCoroutine(Sender.SendWebRequest(webRequest, ResponseToUpdateCoins, ErrorsFromUpdateCoins));

        WeelPanel.transform.Find("Panel").GetComponent<Image>().raycastTarget = false;
        WeelPanel.transform.DOMove(Vector2.zero, 0.25F);
    }

    private void Moving(string type)
    {

        /*  needSector = 
         * 1 - 250 монет
         * 2 - Самый обсосный купон
         * 3 - 750 монет
         * 4 - Средненький купон
         * 5 - 100 монет
         * 6 - ЛЕГЕНДАРНЫЙ купон */

        if (type == "Coins 100") Weel.GetComponent<RouletteMover>().needSector = 5;
        if (type == "Coins 250") Weel.GetComponent<RouletteMover>().needSector = 1;
        if (type == "Coins 750") Weel.GetComponent<RouletteMover>().needSector = 3;
        if (type == "Coupon usual") Weel.GetComponent<RouletteMover>().needSector = 2;
        if (type == "Coupon uncommon") Weel.GetComponent<RouletteMover>().needSector = 4;
        if (type == "Coupon rare") Weel.GetComponent<RouletteMover>().needSector = 6;

        Weel.GetComponent<RouletteMover>().Spin();
    }

    public void Disappear()
    {
        WeelPanel.transform.DOMove(StartPosition, 0.25F);
    }

    private void ActivateWinGift()
    {
        WinGiftPanel.transform.DOMove(Vector2.zero, 0.25F);
    }

    private void WinGiftText(string Description)
    {
        var Text = WinGiftPanel.transform.Find("Panel/Text").gameObject;
        Text.GetComponent<TMP_Text>().text = "Your prise is\n" + Description;
    }

    public void EndWinGift()
    {
        Weel.GetComponent<RouletteMover>().SetToStartVector();

        WinGiftPanel.transform.DOMove(StartPosition, 0.25F);
    }
#endregion
}

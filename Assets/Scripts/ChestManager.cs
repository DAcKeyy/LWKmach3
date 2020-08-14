using System;
using UnityEngine;
using System.Collections;
using LWT.View;
using UnityEngine.UI;
using DG.Tweening;
using ModestTree;

public enum ChestDrop
{
    Coupon,
    buff
}

public class ChestManager : MonoBehaviour
{
    public static Action<ChestDrop, string, string> ChestOpening;
    public static Action UnknownChest;

    [SerializeField] CouponsManager couponsManger = null;
    [SerializeField] float FadeDuration = 1;
    [SerializeField] GameObject MainPanel = null;
    [SerializeField] Counter BombCounter = null;
    [SerializeField] Counter TimeCounter = null;

    private bool isOpend;

    void Start()
    {
        //BombCounter.SetText(Prefs.BombBuff);
        //TimeCounter.SetText(Prefs.TimeBuff);

        if (GlobalDataBase.LootChests.ChestsList.Count != 0)
        {
            gameObject.GetComponent<Image>().raycastTarget = true;
            //MainPanel.GetComponent<Image>().DOFade(0.6f, FadeDuration).OnComplete(() => StartCoroutine(OpenChests()));
        }
    }

    private void OnEnable()
    {
        ChestView.AnimationEnded += ChestOpend;
        ChestView.BombGotten += BombBuffIncrement;
        ChestView.TimeGotten += TimeBuffIncrement;
    }

    private void OnDisable()
    {
        ChestView.AnimationEnded -= ChestOpend;
        ChestView.BombGotten -= BombBuffIncrement;
        ChestView.TimeGotten -= TimeBuffIncrement;
    }
    void ChestOpend()
    {
        isOpend = true;
    }

    void BombBuffIncrement()
    {
        BombCounter.Increment();
        Prefs.BombBuff++;
    }
    void TimeBuffIncrement()
    {
        TimeCounter.Increment();
        Prefs.TimeBuff++;
    }

    IEnumerator WaitForAnimation(int ID)
    {
        yield return new WaitUntil(() => isOpend == true);
        //chest.isOpend = true;
        Debug.Log("Removed: " + GlobalDataBase.LootChests.ChestsList[ID].json);
        GlobalDataBase.LootChests.ChestsList.RemoveAt(ID);
        isOpend = false;
    }

    IEnumerator OpenChests()
    {
        Сoupon coupon = new Сoupon();

        for (int i = GlobalDataBase.LootChests.ChestsList.Count - 1; i >= 0; i--)
        {

            var chest = GlobalDataBase.LootChests.ChestsList[i];

            if (chest == null) continue;
            if (chest.json.IsEmpty()) continue;

            Debug.Log(chest.json);

            LootBox Obj = JsonUtility.FromJson<LootBox>(chest.json);

            switch (Obj.data.type)
            {
                case "coupons":
                    coupon.id = (++Prefs.CoponsAddedTotal).ToString();
                    coupon.company_name = Obj.data.attributes.company;
                    coupon.contact = Obj.data.attributes.contact;
                    coupon.description = Obj.data.attributes.description;
                    coupon.expiration_date = Obj.data.attributes.expiration_date;
                    coupon.promo = Obj.data.attributes.coupon;
                    coupon.isChecked = GlobalDataBase.FalseString;

                    couponsManger.SaveCoupon(coupon);
                    Debug.Log("Saved promo:" + coupon.promo);

                    ChestOpening(ChestDrop.Coupon, coupon.promo, coupon.company_name);
                    yield return StartCoroutine(WaitForAnimation(i));                 
                    break;
                case "boosters":
                    ChestOpening(ChestDrop.buff, Obj.data.attributes.name, null);
                    yield return StartCoroutine(WaitForAnimation(i));
                    break;
                default:
                    UnknownChest();
                    break;
            }  
        }

        couponsManger.CheckCoupons();
        //MainPanel.GetComponent<Image>().DOFade(0, FadeDuration / 3).OnComplete(() => gameObject.GetComponent<Image>().raycastTarget = false);
    }
}
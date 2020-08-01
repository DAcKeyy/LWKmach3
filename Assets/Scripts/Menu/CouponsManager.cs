using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CouponsManager : MonoBehaviour
{
    private Scene currentScene;
    private string JSONcoupon;
    private List<Сoupon> Coupons = new List<Сoupon>();

    [SerializeField] private Transform Prefab = null;
    [SerializeField] private Transform Parent = null;
    [SerializeField] private GameObject Text = null;
    [SerializeField] private GameObject NotificationCounter = null;
    [SerializeField] private Counter NotificationCounterScript = null;

    private void Awake()
    {
        //Сoupon сoupon = new Сoupon();

        currentScene = SceneManager.GetActiveScene();

        //GetCouponFromServer();
        InstantiateCoupon();
    }
    private void OnEnable()
    {
        CheckCoupons();
        CouponsPanelControler.Check_for_notifications_to_show += ShowNotificationPanel;
        CurrentCoupon.Checked += UpdateCoupon;
    }
    private void OnDisable()
    {
        CouponsPanelControler.Check_for_notifications_to_show -= ShowNotificationPanel;
        CurrentCoupon.Checked -= UpdateCoupon;
    }

    private void ShowNotificationPanel(CouponsPanelControler obj)
    {
        obj.ToDeleted = Prefs.CoponsDeleted;
        obj.Show();
        Prefs.CoponsDeleted = 0;
    }

    #region GetCouponFromServer
    public void GetCouponFromServer()
    {   //Код ПОСТ, ГЕТ и прочих запросов и тд... и и нацализация SaveCoupon
        //Расшифровка принятого жысона



        TokenForm tokenForm = new TokenForm(GlobalDataBase.Token);
        
        var webRequest = UnityWebRequest.Post(URLStruct.GetAccountInfo, tokenForm.Form);
    }

    #endregion

    #region Save/Delete
    public void SaveCoupon(Сoupon coupon)
    {
        DataSaver.SaveData(coupon, coupon.company_name + "_" + coupon.promo + "_Coupon");
        if (Prefs.CoponsAdded == Int32.MaxValue - 1) Prefs.CoponsAdded = 0;
        else Prefs.CoponsAdded++;

        InstantiateCoupon();
    }

    public void DeleteInMenu(CurrentCoupon obj)
    {
        DataSaver.DeleteData(obj._сoupon.company_name + "_" + obj._сoupon.promo + "_Coupon");

        InstantiateCoupon();
    }

    public void DeleteCoupon(Сoupon obj)
    {
        DataSaver.DeleteData(obj.company_name + "_" + obj.promo + "_Coupon");
        Prefs.CoponsDeleted++;
        InstantiateCoupon();
    }
    #endregion

    private void UpdateCoupon(CurrentCoupon obj)
    {
        Coupons = DataSaver.FindData<Сoupon>();
        foreach(var coupon in Coupons)
        {
            if(coupon.id == obj._сoupon.id)
            {
                DataSaver.DeleteData(coupon.company_name + "_" + coupon.promo + "_Coupon");
                coupon.isChecked = obj._сoupon.isChecked;
                DataSaver.SaveData(coupon, coupon.company_name + "_" + coupon.promo + "_Coupon");
            }
        }
        CheckCoupons();
    }

    public void CheckCoupons()
    {
        Coupons = DataSaver.FindData<Сoupon>();

        if (Coupons.Count != 0)
        {
            int counter = 0;
            foreach (Сoupon coupon in Coupons)
            {
                Debug.Log(coupon.isChecked);

                if (coupon.isChecked == GlobalDataBase.FalseString)
                {
                    counter++;
                }
            }
            Prefs.CoponsAdded = counter;
        }
        else Prefs.CoponsAdded = 0;
        SetNotificationText();
    }

    private void SetNotificationText()//реализовал тут, в падлу придумывать классы ради классов
    {
        if (Prefs.CoponsAdded != 0 || Prefs.CoponsDeleted != 0)
        {
            NotificationCounterScript.SetText((Prefs.CoponsAdded + Prefs.CoponsDeleted).ToString());
        }
        else NotificationCounter.SetActive(false);
    }

    private void ClearCoupon()
    {
        foreach (Transform child in Parent)
        {
            Destroy(child.gameObject);
        }
    }

    private void InstantiateCoupon()
    {
        //взять все сохранёные из папки data(Win: AppData\LocalLow\TWIRLgames\LWKmach3\) 
        //купоны и отобразить их в меню купоны 
        //Удалить "просроченные" купоны DeleteCoupon'ом

        ClearCoupon();

        if (currentScene.name == "Menu" || currentScene.name == "Start")
        {
            List<Сoupon> сoupons_to_delete = new List<Сoupon>();

            Coupons = DataSaver.FindData<Сoupon>();

            if (Coupons.Count != 0)
            {
                Text.GetComponent<TMP_Text>().enabled = false;

                foreach (Сoupon coupon in Coupons)
                {
                    if (string.IsNullOrEmpty(coupon.expiration_date))
                    {
                        coupon.expiration_date = "Uses for once";
                    }
                    else
                    {
                        if (DateTime.Parse(coupon.expiration_date).CompareTo(DateTime.Today) <= 0)
                        {
                            сoupons_to_delete.Add(coupon);
                            continue;
                        }
                    }

                    var CouponObj = Instantiate(Prefab, Parent); //!!!!!!!!
                    CouponObj.name = coupon.company_name + "_" + coupon.promo + "_Coupon";

                    CouponObj.gameObject.GetComponent<CurrentCoupon>()._сoupon = coupon;

                    if(coupon.isChecked == GlobalDataBase.FalseString)             
                        for(int i = 0; i < CouponObj.childCount; i++)
                            CouponObj.GetChild(i).gameObject.SetActive(true);                  
                }

                foreach (Сoupon coupon in сoupons_to_delete)
                {
                    DeleteCoupon(coupon);
                }
            }
            else
            {
                Text.GetComponent<TMP_Text>().enabled = true;
            }
        }
    }
}

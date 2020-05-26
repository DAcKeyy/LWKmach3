﻿using System;
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

    private void Awake()
    {
        Сoupon сoupon = new Сoupon();

        Debug.Log( JsonUtility.ToJson(сoupon));

        currentScene = SceneManager.GetActiveScene();

        //GetCouponFromServer();
        InstantiateCoupon();
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

        InstantiateCoupon();
    }
    #endregion

    private void ClearCoupon()
    {
        if (currentScene.name == "Menu")//проверка - В меню ли я?
        {
            foreach (Transform child in Parent)
            {
                Destroy(child.gameObject);
            }
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
            List<Сoupon> сoupons_to_delete = new List<Сoupon>();//это женерик нужен для хранения купонов, подлежащих 

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
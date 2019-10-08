using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class CouponsManger : MonoBehaviour
{
    private Scene currentScene;
    private string JSONcoupon;
    private List<Сoupon> Coupons = new List<Сoupon>();

    [SerializeField] private Transform Prefab = null;
    [SerializeField] private Transform Parent = null;
    [SerializeField] private GameObject Text = null;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        //GetCouponFromServer();
        InstantiateCoupon();
    }

    #region GetCouponFromServer
    private void GetCouponFromServer()
    {   //Код ПОСТ, ГЕТ и прочих запросов и тд... и и нацализация SaveCoupon
        //Расщифровка принятого жысона

        if (string.IsNullOrEmpty(JSONcoupon)) //Тут рекурсия, лучше не придумал
        {
            StartCoroutine("POST");
        }
        else
        {
            Debug.Log(JSONcoupon);

            SaveCoupon(JsonUtility.FromJson<Сoupon>(JSONcoupon));
        }
    }
    public IEnumerator POST()
    {
        var URL = "https://wingift.cf/api/promo/show";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, ""))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                JSONcoupon = webRequest.downloadHandler.text;
                GetCouponFromServer();
            }
        }
    }
    #endregion

    #region Save/Delete
    private void SaveCoupon(Сoupon coupon)
    {
        DataSaver.saveData(coupon, coupon.company_name + "_" + coupon.promo + "_Coupon");

        InstantiateCoupon();
    }

    public void DeleteInMenu(CurrentCoupon obj)
    {
        DataSaver.deleteData(obj._сoupon.company_name + "_" + obj._сoupon.promo + "_Coupon");

        InstantiateCoupon();
    }

    public void DeleteCoupon(Сoupon obj)
    {
        DataSaver.deleteData(obj.company_name + "_" + obj.promo + "_Coupon");

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

        if (currentScene.name == "Menu")//проверка - В меню ли я?
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

                foreach(Сoupon coupon in сoupons_to_delete)
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

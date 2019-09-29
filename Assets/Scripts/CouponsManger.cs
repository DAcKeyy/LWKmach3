using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System;

public class CouponsManger : MonoBehaviour
{
    Scene currentScene;
    string JSONcoupon;
    List<Сoupon> Coupons = new List<Сoupon>();

    [SerializeField] Transform Prefab = null;
    [SerializeField] Transform Parent = null;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        //GetCouponFromServer();
        InstantiateCoupon();
    }

    void GetCouponFromServer()
    {   //Код ПОСТ, ГЕТ и прочих запросов и тд... и и нацализация SaveCoupon
        //Расщифровка принятого жысона

        if(string.IsNullOrEmpty(JSONcoupon)) //Тут рекурсия, лучше не придумал
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

    #region Save/Delete
    void SaveCoupon(Сoupon coupon)
    {        
        DataSaver.saveData(coupon, coupon.company_name + "_" + coupon.promo + "_Coupon");

        InstantiateCoupon();
    }

    public void DeleteCoupon(CurrentCoupon obj)
    {
        Debug.Log(obj._сoupon.company_name);
        DataSaver.deleteData(obj._сoupon.company_name + "_" + obj._сoupon.promo + "_Coupon");

        InstantiateCoupon();
    }
    #endregion

    void InstantiateCoupon()    
    {
        //взять все сохранёные из папки data(Win: AppData\LocalLow\TWIRLgames\LWKmach3\) 
            //купоны и отобразить их в меню купоны 
        //Удалить "просроченные" купоны DeleteCoupon'ом

        if (currentScene.name == "Menu")//проверка - В меню ли я?
        {
            Coupons = DataSaver.FindData<Сoupon>();

            if(Coupons != null)
            {
                foreach (Transform child in Parent) Destroy(child.gameObject);

                foreach (Сoupon coupon in Coupons)
                {
                    if(string.IsNullOrEmpty(coupon.expiration_date))
                    {
                        coupon.expiration_date = "Uses for once";
                    }
                    else
                    {
                        if( DateTime.Parse(coupon.expiration_date) .CompareTo(DateTime.Today) <= 0)
                        {
                            CurrentCoupon obj = new CurrentCoupon();
                            obj._сoupon = coupon;
                            DeleteCoupon(obj);
                        }
                    }
                    
                    Instantiate(Prefab, Parent);
                    Prefab.gameObject.GetComponent<CurrentCoupon>()._сoupon.company_name = coupon.company_name;
                    Prefab.gameObject.GetComponent<CurrentCoupon>()._сoupon.promo = coupon.promo;
                    Prefab.gameObject.GetComponent<CurrentCoupon>()._сoupon.description = coupon.description;
                    Prefab.gameObject.GetComponent<CurrentCoupon>()._сoupon.contact = coupon.contact;
                    Prefab.gameObject.GetComponent<CurrentCoupon>()._сoupon.expiration_date = coupon.expiration_date;
                }
            }
        }
    }
}

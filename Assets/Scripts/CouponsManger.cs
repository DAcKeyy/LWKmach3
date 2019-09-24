using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class CouponsManger : MonoBehaviour
{
    string JSONcoupon;
    List<Сoupon> Coupons = new List<Сoupon>();
    [SerializeField] Transform Prefab = null;
    [SerializeField] Transform Parent = null;

    private void Start()
    {
        GetCouponFromServer();
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

    void DeleteCoupon(Сoupon coupon)
    {
        DataSaver.deleteData(coupon.company_name + "_" + coupon.promo + "_Coupon");


    }
    #endregion

    void InstantiateCoupon()    
    {
        //взять все сохранёные из папки data(Win: AppData\LocalLow\TWIRLgames\LWKmach3\) 
            //купоны и отобразить их в меню купоны 
        //Удалить "просроченные" купоны DeleteCoupon'ом

        Scene currentScene = SceneManager.GetActiveScene(); //проверка - В меню ли я?
        if (currentScene.name == "Menu")
        {
            Coupons = DataSaver.FindData<Сoupon>();

            if(Coupons.Count != 0)
            {
                foreach (Сoupon coupon in Coupons)
                {

                    
                    Instantiate(Prefab, Parent);
                    Prefab.gameObject.GetComponent<CurrentCoupon>().company = coupon.company_name;
                    Prefab.gameObject.GetComponent<CurrentCoupon>().coupon = coupon.promo;
                    Prefab.gameObject.GetComponent<CurrentCoupon>().discription = coupon.description;
                    Prefab.gameObject.GetComponent<CurrentCoupon>().contact = coupon.contact;
                    Prefab.gameObject.GetComponent<CurrentCoupon>().lifeSpain = coupon.expiration_date;
                }
            }
        }
    }
}

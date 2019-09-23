using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CouponsManger : MonoBehaviour
{
    List<Сoupon> Coupons = new List<Сoupon>();
    [SerializeField] Transform Prefab = null;
    [SerializeField] Transform Parent = null;

    private void Start()
    {
        InstantiateCoupon();
    }

    void GetCouponFromServer()
    {
        //Код ПОСТ, ГЕТ и прочих запросов и тд... и и нацализация SaveCoupon
        //Расщифровка принятого жысона 


        //var coupon = JsonUtility.FromJson<Сoupon>(JSONcoupon.ToString());
    }

    #region Save/Delete
    void SaveCoupon(Сoupon coupon)
    {        
        DataSaver.saveData(coupon, coupon.Company + "_" + coupon.coupon + "_Coupon");
    }

    void DeleteCoupon(Сoupon coupon)
    {
        DataSaver.deleteData(coupon.Company + "_" + coupon.coupon + "_Coupon");
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

            foreach (Сoupon coupon in Coupons)
            {
                Instantiate(Prefab, Parent);
                Prefab.gameObject.GetComponent<CurrentCoupon>().company = coupon.Company;
                Prefab.gameObject.GetComponent<CurrentCoupon>().coupon = coupon.coupon;
                Prefab.gameObject.GetComponent<CurrentCoupon>().discription = coupon.Discription;
                Prefab.gameObject.GetComponent<CurrentCoupon>().contact = coupon.Contact;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class CouponsManger : MonoBehaviour
{
    private void Start()
    {
        Сoupon xc = new Сoupon();

        xc.Company = "1";
        xc.Contact = "2285648";
        xc.coupon = "6754";
        xc.CouponLifeSpain = "80000";
        xc.Discription = "PEPEPEPEPPEE";

        Сoupon cx = new Сoupon();

        xc.Company = "2";
        xc.Contact = "1313123123";
        xc.coupon = "47567657";
        xc.CouponLifeSpain = "357456";
        xc.Discription = "OEOROROAROAROARR";

        SaveCoupon(xc);
        SaveCoupon(cx);

        InstantiateCoupon();
    }

    void GetCouponFromServer()
    {
        //Код ПОСТ, ГЕТ и прочих запросов и тд... и и нацализация SaveCoupon
        //Расщифровка принятого жысона 


        //var coupon = JsonUtility.FromJson<Сoupon>(JSONcoupon.ToString());
    }

    void SaveCoupon(object coupon)
    {
        DataSaver.saveData(coupon, "coupons");
    }

    void DeleteCoupon(object coupon)
    {
        

        /* Принять объект обращения
         * Найти его в PlayerPrefs и удалить
         * Удалить купон из меню */

    }

    void InstantiateCoupon()    
    {
        //взять лист сохранёных купонов из PlayerPrefs отобразить их в меню купоны 
        //Удалить "просроченные" купоны DeleteCoupon'ом

        Scene currentScene = SceneManager.GetActiveScene(); //проверка - В меню ли я?
        if (currentScene.name == "Menu")
        {

        }
    }
}

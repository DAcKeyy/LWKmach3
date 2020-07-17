using System;
using UnityEngine;
using System.Collections;

public enum ChestDrop
{
    Coupon,
    buff
}

public class ChestManager : MonoBehaviour
{

    CouponsManager couponsManger = new CouponsManager();
    //public static Action<ChestDrop> ChestOpening;
    
    void Start()
    {
        couponsManger = GameObject.Find("EventSystem").GetComponent<CouponsManager>();

        if (GlobalDataBase.LootChests.ChestsList.Count != 0)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        Сoupon coupon = new Сoupon();

        GlobalDataBase.LootChests.ChestsList.ForEach(Chest =>
        {
            LootBox Obj = JsonUtility.FromJson<LootBox>(Chest.json);

            switch (Obj.data.type)
            {
                case "coupons":
                    coupon.company_name = Obj.data.attributes.company;
                    coupon.contact = Obj.data.attributes.contact;
                    coupon.description = Obj.data.attributes.description;
                    coupon.expiration_date = Obj.data.attributes.expiration_date;
                    coupon.promo = Obj.data.attributes.coupon;
                    coupon.isChecked = "False";

                    couponsManger.SaveCoupon(coupon);
                    Debug.Log("Saved promo:" + coupon.promo);

                    StartCoroutine(WaitForAnimation(Chest));
                    //ChestOpening(ChestDrop.Coupon);    
                    break;
                case "buff":
                    //ChestOpening(ChestDrop.buff);
                    break;
            }
        });
    }

    IEnumerator WaitForAnimation(GlobalDataBase.LootChests.Chest chest)
    {
        yield return null;
        //chest.isOpend = true;
        GlobalDataBase.LootChests.ChestsList.Remove(chest);
    }
}
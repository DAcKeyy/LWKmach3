using UnityEngine;

public class ChestManager : MonoBehaviour
{
    CouponsManger couponsManger = new CouponsManger();

    void Start()
    {
        couponsManger = GameObject.Find("EventSystem").GetComponent<CouponsManger>();

        if(GlobalDataBase.ChestJSONs.JSONs.Count != 0)
        {
            Сoupon coupon = new Сoupon();

            GlobalDataBase.ChestJSONs.JSONs.ForEach(JSON =>
            {
                LootBox Obj = JsonUtility.FromJson<LootBox>(JSON);

                switch (Obj.data.type)
                {
                    case "coupons":
                        coupon.company_name = Obj.data.attributes.company;
                        coupon.contact = Obj.data.attributes.contact;
                        coupon.description = Obj.data.attributes.description;
                        coupon.expiration_date = Obj.data.attributes.expiration_date;
                        coupon.promo = Obj.data.attributes.coupon;

                        couponsManger.SaveCoupon(coupon);
                        GlobalDataBase.ChestJSONs.JSONs.Remove(JSON);
                        break;
                    case "buff":

                        break;
                }
            });
        }
    }
}

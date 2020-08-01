using TMPro;
using UnityEngine;
using System;

public class CurrentCoupon : MonoBehaviour
{
    public Сoupon _сoupon;
    private GameObject CheckPanel;
    public Action<CurrentCoupon> ToDelete;
    public static Action<CurrentCoupon> Checked;
    public GameObject NotificationMark;

    private void Start()
    {
        if (this.gameObject.name != "Check Panel")
        {
            this.transform.Find("Text").gameObject.GetComponent<TMP_Text>().text = _сoupon.company_name + "\n " + _сoupon.promo;
        }

        CheckPanel = GameObject.Find("Coupons Canvas/Coupons Panel/Check Panel").gameObject;
    }
    
    public void Delete()
    {
        ToDelete(this);
    }

    public void SetText()
    {
        CheckPanel.SetActive(true);
        CheckPanel.GetComponent<CurrentCoupon>()._сoupon = this.GetComponent<CurrentCoupon>()._сoupon;

        var Panel = CheckPanel.transform.Find("Well Panel/Panel").gameObject;

        Panel.transform.Find("Company Text").gameObject.GetComponent<TMP_Text>().text = _сoupon.company_name;
        Panel.transform.Find("Coupon Text").gameObject.GetComponent<TMP_Text>().text = _сoupon.promo;
        Panel.transform.Find("Description Text").gameObject.GetComponent<TMP_Text>().text = _сoupon.description;
        Panel.transform.Find("Contact Text").gameObject.GetComponent<TMP_Text>().text = _сoupon.contact;
        Panel.transform.Find("LifeSpain Text").gameObject.GetComponent<TMP_Text>().text = _сoupon.expiration_date;

        CheckCoupon();
    }

    private void CheckCoupon()
    {
        _сoupon.isChecked = GlobalDataBase.TrueString;
        Debug.Log(_сoupon.id + " " + _сoupon.isChecked);
        NotificationMark.SetActive(false);
        Checked(this);
    }
}
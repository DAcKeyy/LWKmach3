using UnityEngine;
using TMPro;

public class CurrentCoupon : MonoBehaviour
{
    public string company = "TWIRL Games"
        , coupon = "None"
        , discription = "Coupon isn't correct"
        , contact = "Contact:\ntwirlgamesteam@gmail.com";

    GameObject CheckPanel;

    private void Start()
    {
        this.transform.Find("Text").gameObject.GetComponent<TMP_Text>().text = company + "\n " + coupon;

        CheckPanel = GameObject.Find("Main Canvas/Coupons Panel/Check Panel").gameObject;
    }

    public void SetText()
    {
        CheckPanel.SetActive(true);
        var Panel = CheckPanel.transform.Find("Well Panel/Panel").gameObject;

        Panel.transform.Find("Company Text").gameObject.GetComponent<TMP_Text>().text = company;
        Panel.transform.Find("Coupon Text").gameObject.GetComponent<TMP_Text>().text = coupon;
        Panel.transform.Find("Description Text").gameObject.GetComponent<TMP_Text>().text = discription;
        Panel.transform.Find("Contact Text").gameObject.GetComponent<TMP_Text>().text = contact;
    }
}


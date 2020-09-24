using UnityEngine;
using System;
using TMPro;

public class CouponsPanelControler : MonoBehaviour//Одноразовое говно
{
    public int ToDeleted;
    [SerializeField] private GameObject NotificationPanel = null;
    [SerializeField] private TMP_Text Text = null;

    public static Action<CouponsPanelControler> Check_for_notifications_to_show;

    private void OnEnable()
    {
        Check_for_notifications_to_show(this);    
    }

    public void Show()
    {
        if(ToDeleted != 0)
        {
            NotificationPanel.SetActive(true);
            Text.text = ToDeleted + " coupons were removed due to the date of the promotion";
        }
        ToDeleted = 0;
    }
}
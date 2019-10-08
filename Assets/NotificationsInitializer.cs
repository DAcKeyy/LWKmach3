using UnityEngine;
using NotificationSamples;
using System;

public class NotificationsInitializer : MonoBehaviour
{
    [SerializeField] GameNotificationsManager notManager = null;
    int notDelay;

    private void Start()
    {
        InitializeNotification();
        CreateNotification("Bruh", "Не грусти, братан =)", DateTime.Now.AddSeconds(0));
    }

    private void InitializeNotification()
    {
        GameNotificationChannel channel = new GameNotificationChannel("Bruh", "Капец!", "Не грусти, братан =)");
        notManager.Initialize(channel);
    }

    private void CreateNotification(string title, string body, DateTime time)
    {
        IGameNotification notification = notManager.CreateNotification();
        if(notification != null)
        {
            notification.Title = title;
            notification.Body = body;
            notification.DeliveryTime = time;
            notManager.ScheduleNotification(notification);
        }
    }
}

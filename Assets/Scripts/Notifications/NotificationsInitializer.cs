using UnityEngine;
using NotificationSamples;
using System;

public class NotificationsInitializer : MonoBehaviour
{
    [SerializeField] GameNotificationsManager NotificationsManager = null;

    private void Start()
    {
        InitializeNotification();
    }

    private void InitializeNotification()
    {
        var channel = new GameNotificationChannel("1", "Chanel", "Mobile Notification");
        NotificationsManager.Initialize(channel);
    }

    public void OnTimeInput()
    {
        int time = 10;
        CreateNotification("Bruh", "Не грусти, братан =)", DateTime.Now.AddSeconds(time));
    }

    private void CreateNotification(string title, string body, DateTime time)
    {
        IGameNotification notification = NotificationsManager.CreateNotification();
        if(notification != null)
        {
            notification.Title = title;
            notification.Body = body;
            notification.DeliveryTime = time;
            notification.LargeIcon = "pepelarge";
            NotificationsManager.ScheduleNotification(notification);
        }
    }
}

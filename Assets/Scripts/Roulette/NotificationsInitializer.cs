using UnityEngine;
using NotificationSamples;
using System;

public class NotificationsInitializer : MonoBehaviour
{
    [SerializeField] GameNotificationsManager NotificationsManager = null;

    public const string ChannelId = "game_channel0";

    //void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}

    private void Start()
    {
        var channel1 = new GameNotificationChannel(ChannelId, "Game Chanel", "Generic Notification");

        NotificationsManager.Initialize(channel1);
    }

    public void OnTimeInput()
    {
        int time = 10;
        CreateNotification("Bruh", "Good *datetime*, young developer =)", ChannelId, DateTime.Now.AddSeconds(time));
    }

    private void CreateNotification(string title, string body, string channelId, DateTime time)
    {
        IGameNotification notification = NotificationsManager.CreateNotification();

        if(notification != null)
        {
            notification.Title = title;
            notification.Body = body;
            notification.Group = channelId;
            notification.DeliveryTime = time;
            notification.LargeIcon = "pepelarge";

            NotificationsManager.ScheduleNotification(notification);
        }
    }
}

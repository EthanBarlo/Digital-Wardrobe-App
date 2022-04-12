using System;
using System.Collections.Generic;
using System.Text;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using MyDigitalWardrobe.Views;

namespace MyDigitalWardrobe.Services
{
    public static class NotificationService
    {
        public static void Init()
        {
            NotificationCenter.Current.NotificationReceived += OnNotificationReceived;
            NotificationCenter.Current.NotificationTapped += Current_NotificationTapped;
        }
        
        private static void Current_NotificationTapped(NotificationEventArgs e)
        {
            Xamarin.Forms.Shell.Current.GoToAsync($"//{nameof(ViewItems)}");
            throw new NotImplementedException();
        }

        private static void OnNotificationReceived(NotificationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void SendNotification(string message)
        {
            var notification = new NotificationRequest
            {
                Title = "Digital Wardrobe",
                Description = message,
            };
            NotificationCenter.Current.Show(notification);
        }

        public static void ScheduleWarantyNotification(string message, DateTime date, int id){
            var notification = new NotificationRequest
            {
                Title = "Waranty Close to expiring!",
                Description = message,
                ReturningData = id.ToString(),
                Schedule = new NotificationRequestSchedule { NotifyTime = date }
            };
            NotificationCenter.Current.Show(notification);
        }
    }
}

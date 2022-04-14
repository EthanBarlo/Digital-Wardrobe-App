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
            NotificationCenter.Current.NotificationTapped += Current_NotificationTapped;
        }
        
        private static void Current_NotificationTapped(NotificationEventArgs e)
        {
            Xamarin.Forms.Shell.Current.GoToAsync($"//{nameof(ViewItems)}");
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

        public static void ScheduleWarantyNotification(string itemName, DateTime date, int id){
            var notification = new NotificationRequest
            {
                Title = itemName,
                Description = "Waranty Close to expiring!",
                ReturningData = id.ToString(),
                Schedule = new NotificationRequestSchedule { NotifyTime = date }
            };
            NotificationCenter.Current.Show(notification);
        }
    }
}

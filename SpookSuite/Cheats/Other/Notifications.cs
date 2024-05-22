using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Cheats
{
    public enum NotificationType
    { 
        Info,
        Warning,
        Error,
        Dev
    }

    public class Notifcation
    {
        private string title = "";
        private string description = "";
        private NotificationType type = NotificationType.Info;
        private int life = 500; //5s
        public Notifcation(string Title, string Descriptor)
        {
            title = Title;
            description = Descriptor;
            type = NotificationType.Info;
        }
        public Notifcation(string Title, string Descriptor, NotificationType Type)
        {
            title = Title;
            description = Descriptor;
            type = Type;
        }

        public NotificationType GetType() => type;
        public string GetTitle() => title;

        public void Draw(int id)
        {
            UI.HorizontalSpace(null, () =>
            {
                if (life <= 0)
                    Notifications.notifcations.Remove(this);
                else
                    life = life - 1;
            });
            UI.Label(description);
            //line showing life
        }
    }

    internal class Notifications : ToggleCheat
    {
        private static Rect windowRect = new Rect(1600, 10, 300, 100);
        public static Rect defaultRect = new Rect(1600, 10, 300, 100);
        public static List<Notifcation> notifcations = new List<Notifcation>();
        public static int width = 300, height = 100, spacing = 110;

        public static void PushNotifcation(Notifcation notifcation)
        {
            notifcations.Add(notifcation);
        }

        public static string GetNotifcationType(Notifcation noti)
        {
            switch (noti.GetType())
            {
                case NotificationType.Info: return "(-)";
                case NotificationType.Warning: return "(!)";
                case NotificationType.Error: return "(X)";
                case NotificationType.Dev: return "(DEV)";
                default: return "ERROR";
            }
        }

        public override void OnGui()
        {
            GUI.skin = ThemeUtil.Skin;
            GUI.color = Color.white;
            //GUI.color = new Color(1f, 1f, 1f, 1);
            for (int i = 0; i < notifcations.Count; i++)
            {
                Notifcation current = notifcations[i];
                if (current != notifcations.First())
                    windowRect = new Rect(windowRect.x, windowRect.y + spacing, width, height);
                else
                    windowRect = defaultRect;
                GUILayout.Window(100 + i, windowRect, new GUI.WindowFunction(current.Draw), $"{GetNotifcationType(current)} {current.GetTitle()}");
            }
        }
    }
}

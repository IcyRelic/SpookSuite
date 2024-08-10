using JetBrains.Annotations;
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
        private float life = 500; //5s
        public Rect lastRect = new Rect(1600, 10, 300, 100);
        public Rect desiredRect = new Rect(1600, 10, 300, 100);
        public Rect currentRect = new Rect(1600, 10, 300, 100);
        private float originalLife = 500;
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
        public Notifcation(string Title, string Descriptor, NotificationType Type, float Life) //put 1 for 1 sec, 2 for 2 sec etc rework so life is 90 if one
        {
            title = Title;
            description = Descriptor;
            type = Type;
            originalLife = Life * 100f;
            life = Life * 100f;
        }

        public NotificationType GetType() => type;
        public string GetTitle() => title;

        public float GetLife() => life;

        public void Draw(int id)
        {
            if (life <= 0 && currentRect.y == desiredRect.y)
                Notifications.notifcations.Remove(this);
            else
                life = life - .9f;
            UI.Label(description);
            GUI.DrawTexture(new Rect(5, 75, life, 5), Texture2D.whiteTexture); //make me not go off the notification!
        }
    }

    internal class Notifications : ToggleCheat
    {
        public static Rect defaultRect = new Rect(1600, 10, 300, 100);
        public static List<Notifcation> notifcations = new List<Notifcation>();
        public static int width = 300, height = 100, spacing = 110, animSpeed = 2;
        public static void PushNotifcation(Notifcation notifcation) => notifcations.Add(notifcation);
        public static void PushNotifcation(string title, string desc = "", NotificationType type = NotificationType.Info, float life = 5) => notifcations.Add(new Notifcation(title, desc, type, life));

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
                {
                    Notifcation last = notifcations[i - 1];
                    current.lastRect = last.desiredRect;

                    if (current.currentRect.y < last.currentRect.y)
                        current.currentRect = last.currentRect;

                    if (current.currentRect.y < current.desiredRect.y)
                    {
                        if (current.currentRect.y + animSpeed > current.desiredRect.y)
                            current.currentRect.y = current.desiredRect.y;
                        else
                            current.currentRect.y += animSpeed;
                    }
                    else if (current.currentRect.y > current.desiredRect.y) 
                        current.currentRect.y = current.desiredRect.y;

                    current.desiredRect = new Rect(last.currentRect.x, last.currentRect.y + spacing, width, height);
                }
                else
                {
                    //current.currentRect = defaultRect;
                    current.desiredRect = current.lastRect;
                    if (current.currentRect.y > current.desiredRect.y)
                    {
                        if (current.currentRect.y - animSpeed < current.desiredRect.y)
                            current.currentRect.y = current.desiredRect.y;
                        else
                            current.currentRect.y -= animSpeed;
                    }
                    else if (current.currentRect.y < current.desiredRect.y)
                        current.currentRect.y = current.desiredRect.y;
                }
                GUILayout.Window(100 + i, current.currentRect, new GUI.WindowFunction(current.Draw), $"{GetNotifcationType(current)} {current.GetTitle()}");
            }
        }
    }
}

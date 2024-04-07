using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class UnlimitedFilm : ToggleCheat
    {
        private float defaultMaxTime = 90;

        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;

            ItemInstance item = Player.localPlayer.data.currentItem;

            if (item.GetComponent<VideoCamera>() is null) return;

            VideoCamera camera = item.GetComponent<VideoCamera>();

            VideoInfoEntry videoInfoEntry = camera.Reflect().GetValue<VideoInfoEntry>("m_recorderInfoEntry");

            videoInfoEntry.timeLeft = videoInfoEntry.maxTime;
        }

    }
}

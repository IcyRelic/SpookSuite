using SpookSuite.Cheats.Core;
using SpookSuite.Util;

namespace SpookSuite.Cheats
{
    internal class UnlimitedFilm : ToggleCheat
    {
        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;

            ItemInstance item = Player.localPlayer.data.currentItem;
            if (item is null || item.GetComponent<VideoCamera>() is null) return;
            VideoCamera camera = item.GetComponent<VideoCamera>();

            VideoInfoEntry videoInfoEntry = camera.Reflect().GetValue<VideoInfoEntry>("m_recorderInfoEntry");
            videoInfoEntry.timeLeft = videoInfoEntry.maxTime;  
        }

    }
}

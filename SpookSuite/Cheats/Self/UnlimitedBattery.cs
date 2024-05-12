using SpookSuite.Cheats.Core;
using SpookSuite.Util;

namespace SpookSuite.Cheats
{
    internal class UnlimitedBattery : ToggleCheat
    {
        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;

            ItemInstance item = Player.localPlayer.data.currentItem;
            BatteryEntry battery = null;

            if(item is null) return;

            if (item.GetComponent<Flashlight>() is not null)
                battery = item.GetComponent<Flashlight>().Reflect().GetValue<BatteryEntry>("m_batteryEntry");

            if (item.GetComponent<Defib>() is not null)
                battery = item.GetComponent<Defib>().Reflect().GetValue<BatteryEntry>("m_batteryEntry");

            if (item.GetComponent<ShockStick>() is not null)
                battery = item.GetComponent<ShockStick>().Reflect().GetValue<BatteryEntry>("m_batteryEntry");

            if (item.GetComponent<RescueHook>() is not null)
                battery = item.GetComponent<RescueHook>().Reflect().GetValue<BatteryEntry>("m_batteryEntry");

            if (item.GetComponent<NorgGun>() is not null)
                battery = item.GetComponent<NorgGun>().Reflect().GetValue<BatteryEntry>("m_batteryEntry");

            if (item.GetComponent<PartyPopper>() is not null)
            {
                PartyPopper partyPopper = item.GetComponent<PartyPopper>();
                OnOffEntry usedEntry = partyPopper.Reflect().GetValue<OnOffEntry>("usedEntry");
                StashAbleEntry stashAbleEntry = partyPopper.Reflect().GetValue<StashAbleEntry>("stashAbleEntry");
                usedEntry.on = false;
                stashAbleEntry.isStashAble = true;
                stashAbleEntry.ClearDirty();
                usedEntry.ClearDirty();
                partyPopper.chargesLeftGO.SetActive(true);
                partyPopper.Reflect().SetValue("wasUsedOnConfig", false);

            }

            if (item.GetComponent<Flare>() is not null)
            {
                Flare flare = item.GetComponent<Flare>();
                LifeTimeEntry lifeTimeEntry = flare.Reflect().GetValue<LifeTimeEntry>("m_lifeTimeEntry");
                lifeTimeEntry.m_lifeTimeLeft = flare.maxLifeTime;
            }

            if (battery is not null)
                battery.AddCharge(10_000f);
        }

    }
}

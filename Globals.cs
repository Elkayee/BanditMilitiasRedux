using System.Collections.Generic;
using System.Diagnostics;
using SandBox.ViewModelCollection.Map;
using SandBox.ViewModelCollection.Map.Tracker;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BanditMilitias
{
    public struct Globals
    {
        // ── Constants ────────────────────────────────────────────────────────────

        internal const float MergeDistance = 1.5f;
        internal const float FindRadius = 20;
        internal const float MinDistanceFromHideout = 8;

        // ── Settings & Timers ────────────────────────────────────────────────────

        internal static Settings Settings;
        internal static readonly Stopwatch T = new();
        internal static double LastCalculated;
        internal static double PartyCacheInterval;

        // ── Power & Size Calculations ────────────────────────────────────────────

        internal static float CalculatedMaxPartySize;
        internal static float CalculatedGlobalPowerLimit;
        internal static float GlobalMilitiaPower;
        internal static float MilitiaPowerPercent;
        internal static float MilitiaPartyAveragePower;
        internal static float Variance => MBRandom.RandomFloatRanged(0.925f, 1.075f);

        // ── Party & Hero Tracking ────────────────────────────────────────────────

        internal static IEnumerable<ModBanditMilitiaPartyComponent> AllBMs;
        internal static List<Hero> Heroes = new();
        internal static List<CharacterObject> HeroTemplates = new();
        internal static int RaidCap;

        // ── Character Pools ──────────────────────────────────────────────────────

        internal static Dictionary<CultureObject, List<CharacterObject>> Recruits = new();
        internal static List<CharacterObject> BasicRanged = new();
        internal static List<CharacterObject> BasicInfantry = new();
        internal static List<CharacterObject> BasicCavalry = new();
        internal static CharacterObject Giant;

        // ── Equipment & Items ────────────────────────────────────────────────────

        internal static Dictionary<ItemObject.ItemTypeEnum, List<ItemObject>> ItemTypes = new();
        internal static List<EquipmentElement> EquipmentItems = new();
        internal static List<EquipmentElement> EquipmentItemsNoBow = new();
        internal static List<Equipment> BanditEquipment = new();
        internal static List<ItemObject> Arrows = new();
        internal static List<ItemObject> Bolts = new();
        internal static List<ItemObject> Mounts;
        internal static List<ItemObject> Saddles;

        // ── Map & UI ─────────────────────────────────────────────────────────────

        internal static Dictionary<MobileParty, BannerImageIdentifierVM> PartyImageMap = new();
        internal static readonly List<Banner> Banners = new();
        internal static MapTrackerProvider MapTrackerProvider;
        internal static object TrackerContainer;

        // ── World Objects ────────────────────────────────────────────────────────

        internal static List<Settlement> Hideouts;
        internal static Clan Looters;
        internal static Clan Wights; // ROT
        internal static HashSet<int> LordConversationTokens;

        // ── Stuck Detection (transient – not saved, resets on load) ──────────────

        internal static readonly Dictionary<MobileParty, (Vec2 LastPos, int HourCount)> StuckTracker = new();

        // ── Compatibility ────────────────────────────────────────────────────────

        // ArmsDealer compatibility
        internal static CultureObject BlackFlag => MBObjectManager.Instance.GetObject<CultureObject>("ad_bandit_blackflag");

        // ── Difficulty / Gold Maps ────────────────────────────────────────────────

        internal static Dictionary<TextObject, int> DifficultyXpMap = new()
        {
            { new TextObject("{=BMXpOff}Off"), 0 },
            { new TextObject("{=BMXpNormal}Normal"), 300 },
            { new TextObject("{=BMXpHard}Hard"), 600 },
            { new TextObject("{=BMXpHardest}Hardest"), 900 },
        };

        internal static Dictionary<TextObject, int> GoldMap = new()
        {
            { new TextObject("{=BMGoldLow}Low"), 250 },
            { new TextObject("{=BMGoldNormal}Normal"), 500 },
            { new TextObject("{=BMGoldRich}Rich"), 900 },
            { new TextObject("{=BMGoldRichest}Richest"), 2000 },
        };

        // ── Lifecycle ────────────────────────────────────────────────────────────

        public static void ClearGlobals()
        {
            PartyImageMap = new();
            ItemTypes = new();
            Recruits = new();
            EquipmentItems = new();
            BanditEquipment = new();
            Arrows = new();
            Bolts = new();
            LastCalculated = 0;
            PartyCacheInterval = 0;
            RaidCap = 0;
            HeroTemplates = new();
            Mounts = new();
            Saddles = new();
            Hideouts = new();
            AllBMs = new ModBanditMilitiaPartyComponent[] { };
            StuckTracker.Clear();

            /*
            foreach (var BM in Helper.GetCachedBMs(true).SelectQ(bm => bm.Party))
            {
                var index = MapMobilePartyTrackerVM.Trackers.FindIndexQ(t =>
                    t.TrackedParty == BM.MobileParty);
                if (index >= 0)
                    MapMobilePartyTrackerVM.Trackers.RemoveAt(index);
            }
            */
        }
    }
}

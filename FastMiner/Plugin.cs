using C3.ModKit;
using HarmonyLib;
using Unfoundry;

namespace FastMiner
{
    [UnfoundryMod(GUID)]
    public class Plugin : UnfoundryPlugin
    {
        public const string
            MODNAME = "FastMiner",
            AUTHOR = "erkle64",
            GUID = AUTHOR + "." + MODNAME,
            VERSION = "1.1.0";

        public static LogSource log;

        public static TypedConfigEntry<float> speedMultiplier;

        public Plugin()
        {
            log = new LogSource(MODNAME);

            new Config(GUID)
                .Group("Multipliers")
                    .Entry(out speedMultiplier, "speedMultiplier", 8.0f, "Mining speed multiplication factor.")
                .EndGroup()
                .Load()
                .Save();
        }

        public override void Load(Mod mod)
        {
            log.Log($"Loading {MODNAME}");
        }

        [HarmonyPatch]
        public static class Patch
        {
            [HarmonyPatch(typeof(ItemTemplate), nameof(ItemTemplate.onLoad))]
            [HarmonyPostfix]
            public static void itemTemplateOnLoad(ItemTemplate __instance)
            {
                if ((__instance.flags & ItemTemplate.ItemTemplateFlags.MINING_TOOL) != 0)
                {
                    var newMiningTimeReductionInSec = __instance.miningTimeReductionInSec * speedMultiplier.Get();
                    log.LogFormat("Fastinating {0} from {1} to {2}", __instance.identifier, __instance.miningTimeReductionInSec, newMiningTimeReductionInSec);
                    __instance.miningTimeReductionInSec = newMiningTimeReductionInSec;
                }
            }
        }
    }
}

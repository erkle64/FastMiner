using BepInEx;
using UnhollowerRuntimeLib;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FastMiner
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class BepInExLoader : BepInEx.IL2CPP.BasePlugin
    {
        public const string
            MODNAME = "FastMiner",
            AUTHOR = "erkle64",
            GUID = "com." + AUTHOR + "." + MODNAME,
            VERSION = "1.0.0.0";

        public static BepInEx.Logging.ManualLogSource log;

        public BepInExLoader()
        {
            log = Log;
        }

        public override void Load()
        {
            log.LogMessage("Registering ItemTemplateComponent in Il2Cpp");

            try
            {
                // Register our custom Types in Il2Cpp
                ClassInjector.RegisterTypeInIl2Cpp<ItemTemplateComponent>();

                var go = new GameObject("TrainerObject");
                go.AddComponent<ItemTemplateComponent>();
                Object.DontDestroyOnLoad(go);
            }
            catch
            {
                log.LogError("[Trainer] FAILED to Register Il2Cpp Type: ItemTemplateComponent!");
            }

            try
            {
                var harmony = new Harmony(GUID);

                // Our Primary Unity Event Hooks 

                var original = AccessTools.Method(typeof(ItemTemplate), "LoadAllItemTemplatesInBuild");
                log.LogMessage("[Trainer] Harmony - Original Method: " + original.DeclaringType.Name + "." + original.Name);
                var post = AccessTools.Method(typeof(ItemTemplateComponent), "LoadAllItemTemplatesInBuild");
                log.LogMessage("[Trainer] Harmony - Postfix Method: " + post.DeclaringType.Name + "." + post.Name);
                harmony.Patch(original, postfix: new HarmonyMethod(post));
            }
            catch
            {
                log.LogError("[Trainer] Harmony - FAILED to Apply Patch's!");
            }
        }
    }
}
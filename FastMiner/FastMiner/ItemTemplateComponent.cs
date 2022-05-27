using System;
using UnhollowerBaseLib;
using HarmonyLib;
using UnityEngine;

namespace FastMiner
{
    public class ItemTemplateComponent : MonoBehaviour
    {
        public ItemTemplateComponent (IntPtr ptr) : base(ptr)
        {
        }

        [HarmonyPostfix]
        public static Il2CppReferenceArray<ItemTemplate> LoadAllItemTemplatesInBuild(Il2CppReferenceArray<ItemTemplate> result)
        {
            BepInExLoader.log.LogMessage("Scanning item templates");
            for(int i = 0; i < result.Length; ++i)
            {
                if(result[i]._isMiningTool())
                {
                    BepInExLoader.log.LogMessage(string.Format("Fastinating {0} from {1} to {2}", result[i].identifier, result[i].miningTimeReductionInSec, result[i].miningTimeReductionInSec * 8));
                    result[i].miningTimeReductionInSec = result[i].miningTimeReductionInSec * 8;
                }
            }
            return result;
        }
    }
}
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;

namespace SmithForever.Patches
{
    /// <summary>
    /// Patches <see cref="DefaultSmithingModel.GetEnergyCostForRefining"/>
    /// to always return 0, removing the stamina cost for refining materials.
    /// </summary>
    [HarmonyPatch(typeof(DefaultSmithingModel), "GetEnergyCostForRefining",
        new[] { typeof(Crafting.RefiningFormula), typeof(Hero) },
        new[] { ArgumentType.Ref, ArgumentType.Normal })]
    internal static class RefiningStaminaPatch
    {
        /// <param name="__result">The return value of the original method.</param>
        /// <returns><c>false</c> to skip the original method.</returns>
        internal static bool Prefix(ref int __result)
        {
            __result = 0;
            return false;
        }
    }

    /// <summary>
    /// Patches <see cref="DefaultSmithingModel.GetEnergyCostForSmithing"/>
    /// to always return 0, removing the stamina cost for smithing weapons.
    /// </summary>
    [HarmonyPatch(typeof(DefaultSmithingModel), "GetEnergyCostForSmithing",
        new[] { typeof(ItemObject), typeof(Hero) })]
    internal static class SmithingStaminaPatch
    {
        /// <param name="__result">The return value of the original method.</param>
        /// <returns><c>false</c> to skip the original method.</returns>
        internal static bool Prefix(ref int __result)
        {
            __result = 0;
            return false;
        }
    }

    /// <summary>
    /// Patches <see cref="DefaultSmithingModel.GetEnergyCostForSmelting"/>
    /// to always return 0, removing the stamina cost for smelting items.
    /// </summary>
    [HarmonyPatch(typeof(DefaultSmithingModel), "GetEnergyCostForSmelting",
        new[] { typeof(ItemObject), typeof(Hero) })]
    internal static class SmeltingStaminaPatch
    {
        /// <param name="__result">The return value of the original method.</param>
        /// <returns><c>false</c> to skip the original method.</returns>
        internal static bool Prefix(ref int __result)
        {
            __result = 0;
            return false;
        }
    }
}

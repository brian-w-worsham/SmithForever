using HarmonyLib;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SmithForever
{
    /// <summary>
    /// Entry point for the SmithForever mod. Applies Harmony patches on load
    /// to eliminate stamina costs for Refining, Smelting, and Smithing.
    /// </summary>
    public class SubModule : MBSubModuleBase
    {
        /// <summary>Harmony instance used to apply and revert all patches.</summary>
        private Harmony _harmony;

        /// <summary>
        /// Called when the module is first loaded. Applies all Harmony patches
        /// and displays a confirmation message.
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                _harmony = new Harmony("com.smithforever.bannerlord");
                _harmony.PatchAll();
                InformationManager.DisplayMessage(
                    new InformationMessage("SmithForever: Loaded — unlimited smithing stamina!", Colors.Green));
            }
            catch (System.Exception ex)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage($"SmithForever load error: {ex.Message}", Colors.Red));
            }
        }

        /// <summary>
        /// Called when the module is unloaded. Reverts all Harmony patches
        /// applied by this mod.
        /// </summary>
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            _harmony?.UnpatchAll("com.smithforever.bannerlord");
        }
    }
}

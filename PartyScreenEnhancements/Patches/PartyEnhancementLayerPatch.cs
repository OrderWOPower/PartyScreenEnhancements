﻿using HarmonyLib;
using PartyScreenEnhancements.ViewModel;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace PartyScreenEnhancements.Patches
{
    /// <summary>
    /// Simple patch in order to create the required overlay on top of the PartyScreen
    /// </summary>
    [HarmonyPatch(typeof(ScreenBase))]
    public class PartyEnhancementLayerPatch
    {
        internal static GauntletLayer screenLayer;
        internal static PartyEnhancementsVM enhancementVm;

        [HarmonyPatch("AddLayer")]
        public static void Postfix(ref ScreenBase __instance)
        {
            if (__instance is GauntletPartyScreen partyScreen && screenLayer == null)
            {
                screenLayer = new GauntletLayer(10);

                var traverser = Traverse.Create(partyScreen);
                PartyVM partyVM = traverser.Field<PartyVM>("_dataSource").Value;
                PartyState partyState = traverser.Field<PartyState>("_partyState").Value;

                enhancementVm = new PartyEnhancementsVM(partyVM, partyState.PartyScreenLogic, partyScreen);
                screenLayer.LoadMovie("PartyScreenEnhancements", enhancementVm);
                screenLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
                partyScreen.AddLayer(screenLayer);
            }
        }

        [HarmonyPatch("RemoveLayer")]
        public static void Prefix(ref ScreenBase __instance, ref ScreenLayer layer)
        {
            if (__instance is GauntletPartyScreen && screenLayer != null && layer.Input.IsCategoryRegistered(HotKeyManager.GetCategory("PartyHotKeyCategory")))
            {
                __instance.RemoveLayer(screenLayer);
                enhancementVm.OnFinalize();
                enhancementVm = null;
                screenLayer = null;
            }
        }
    }
}
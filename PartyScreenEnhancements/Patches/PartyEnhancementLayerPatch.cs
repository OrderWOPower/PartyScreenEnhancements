﻿using System.Diagnostics;
using System.Linq;
using HarmonyLib;
using PartyScreenEnhancements.ViewModel;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace PartyScreenEnhancements.Patches
{
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
                screenLayer = new GauntletLayer(100);

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
            if (__instance is GauntletPartyScreen partyScreen && screenLayer != null && layer.Input.IsCategoryRegistered(HotKeyManager.GetCategory("PartyHotKeyCategory")))
            {
                __instance.RemoveLayer(screenLayer);
                enhancementVm.OnFinalize();
                enhancementVm = null;
                screenLayer = null;
            }
        }
    }
}
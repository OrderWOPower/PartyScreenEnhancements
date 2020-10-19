﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PartyScreenEnhancements.Saving;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;

namespace PartyScreenEnhancements.Patches
{
    [HarmonyPatch(typeof(PartyVM), "UpdateCurrentCharacterUpgrades")]
    public class UpgradeButtonTooltipPatch
    {
        private const string UPGRADE_TOOLTIP = "\nHold [CTRL] and [SHIFT] to select as preferred upgrade path";

        public static void Postfix(ref PartyVM __instance)
        {
            if (ScreenManager.TopScreen is GauntletPartyScreen && PartyScreenConfig.ExtraSettings.PathSelectTooltips)
            {
                var current_char = __instance.CurrentCharacter;
                if(current_char == null) return;

                // Pretty dirty way to do this, but ㄟ( ▔, ▔ )ㄏ it'll work for now.
                if (!current_char.Upgrade1Hint?.HintText.Contains(UPGRADE_TOOLTIP) ?? false) 
                    __instance.CurrentCharacter.Upgrade1Hint.HintText += UPGRADE_TOOLTIP;
                if (!current_char.Upgrade2Hint?.HintText.Contains(UPGRADE_TOOLTIP) ?? false)
                    __instance.CurrentCharacter.Upgrade2Hint.HintText += UPGRADE_TOOLTIP;
            }
        }

    }
}
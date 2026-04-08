using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace BanditMilitias.Patches
{
    /// <summary>
    /// Suppresses voice lines for female bandit heroes.
    /// The game ships zero female bandit voice recordings, so without this
    /// patch any female bandit hero plays a male voice line.
    /// Applied as a manual patch so it registers after all attribute-based
    /// patches and runs as the last postfix on the target method.
    /// </summary>
    internal sealed class VoicePatches
    {
        private static ILogger _logger;
        private static ILogger Logger => _logger ??= LogFactory.Get<VoicePatches>();

        internal static void ApplyManualPatch(Harmony harmony)
        {
            var original = AccessTools.Method(
                typeof(DefaultVoiceOverModel),
                nameof(DefaultVoiceOverModel.GetSoundPathForCharacter));

            if (original is null)
            {
                Logger.LogWarning("Could not find DefaultVoiceOverModel.GetSoundPathForCharacter to patch.");
                return;
            }

            harmony.Patch(original, postfix: new HarmonyMethod(
                AccessTools.Method(typeof(VoicePatches), nameof(GenderVoicePostfix)))
            {
                after = new[] { "BanditVoiceFix" }
            });

            Logger.LogInformation("Voice gender fix patch applied.");
        }

        internal static void GenderVoicePostfix(CharacterObject character, ref string __result)
        {
            if (string.IsNullOrEmpty(__result))
                return;

            if (character == null)
                return;

            try
            {
                if (!character.IsFemale)
                    return;

                if (character.Occupation != Occupation.Bandit)
                    return;

                __result = "";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in voice gender postfix");
            }
        }
    }
}
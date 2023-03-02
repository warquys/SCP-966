using HarmonyLib;
using Neuron.Core.Meta;
using PlayerRoles.PlayableScps;
using Scp966;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Patching;
using System;

namespace Scp966
{
    [HarmonyPatch]
    public static class FallingIntoAbyssPatch
    {
        static Scp966PluginClass _plugin;
        static FallingIntoAbyssPatch()
        {
            _plugin = Synapse.Get<Scp966PluginClass>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(VisionInformation), nameof(VisionInformation.CheckAttachments))]
        public static bool CheckAttachments(ref bool __result, ReferenceHub source)
        {
            try
            {
                var player = source.GetSynapsePlayer();
                if (_plugin.NightVisionPlayers.ContainsKey(player))
                {
                    __result = true;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                SynapseLogger<Synapse>.Error("NightVision Patch failed\n" + ex);
                return true;
            }
        }

        
    }

}

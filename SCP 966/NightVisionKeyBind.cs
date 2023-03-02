using Neuron.Core.Meta;
using Scp966;
using Synapse3.SynapseModule.KeyBind;
using Synapse3.SynapseModule.Player;
using System.Linq;

namespace Scp966
{
    [Automatic]
    [KeyBind(
    Bind = UnityEngine.KeyCode.B,
    CommandName = "NightVision",
    CommandDescription = "Changes between scp and proximity chat when you are talking as scp"
    )]
    internal class NightVisionKeyBind : KeyBind
    {
        Scp966PluginClass _plugin;
        public NightVisionKeyBind(Scp966PluginClass plugin)
        {
            _plugin = plugin;
        }

        public override void Execute(SynapsePlayer player)
        {
            if (!player.Inventory.Items.Any(p => p.Id == 138 || p.Id == 137))
                return;
            if (_plugin.NightVisionPlayers.ContainsKey(player))
            {
                _plugin.RemoveNightVisionPlayer(player);
                player.SendHint(_plugin.Translation.DeactivateNightVision);
            }
            else
            {
                _plugin.AddNightVisionPlayer(player);
                player.SendHint(_plugin.Translation.ActivateNightVision);
            }
        }
    }
}

using Neuron.Core.Meta;
using Neuron.Core.Plugins;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Scp966;
using Synapse3.SynapseModule.Command;
using System.Linq;

namespace Scp966
{
    [Automatic]
    [SynapseCommand(
    CommandName = "NightVision",
    Description = "Disable or Enable the night vision",
    Aliases = new[] { "nv", "Night" },
    Platforms = new[] { CommandPlatform.PlayerConsole }
    )]
    internal class NightVisionCommand : SynapseCommand
    {
        Scp966PluginClass _plugin;
        public NightVisionCommand(Scp966PluginClass plugin)
        {
            _plugin = plugin;
        }

        public override void Execute(SynapseContext context, ref CommandResult result)
        {

            if (context.Player.Inventory.Items.Any(p => p.Id == 138 || p.Id == 137))
            {
                if (_plugin.NightVisionPlayers.ContainsKey(context.Player))
                {
                    _plugin.RemoveNightVisionPlayer(context.Player);
                    result.Response = _plugin.Translation.DeactivateNightVision;
                }
                else
                {
                    _plugin.AddNightVisionPlayer(context.Player);
                    result.Response = _plugin.Translation.ActivateNightVision;
                }
                return;
            }

            result.Response = _plugin.Translation.NoNightVision;
            result.StatusCode = CommandStatusCode.Error;

        }
    }
}

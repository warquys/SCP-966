using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;

namespace Scp966
{
    [Automatic]
    public class Scp966Translation : Translations<Scp966Translation>
    {
        public string PickupNightVision { get; } = "You pickup a night vision, you can see in the dark!";
        public string ActivateNightVision { get; } = "You activate you night vision";
        public string DeactivateNightVision { get; } = "You deactivate you night vision";
        public string NoNightVision { get; } = "You don't ave night vision in you invotory";
    }
}

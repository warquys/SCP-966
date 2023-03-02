using Neuron.Core.Meta;
using Syml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scp966
{
    [Automatic]
    [DocumentSection("SCP082Config")]
    public class Scp966PluginConfig : IDocumentSection
    {
        public Scp966PlayerScript.Scp966Config Scp966Config { get; set; } = new Scp966PlayerScript.Scp966Config();
    }
}

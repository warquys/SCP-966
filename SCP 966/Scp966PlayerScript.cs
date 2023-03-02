using Neuron.Core.Meta;
using PlayerRoles;
using Synapse3.SynapseModule.Config;
using Synapse3.SynapseModule.Map.Rooms;
using Synapse3.SynapseModule.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Scp966.Scp966PlayerScript;

namespace Scp966
{
    [Automatic]
    [Role(
        Id = 966,
        Name = "SCP-966",
        TeamId = (uint)Team.SCPs
    )]
    public class Scp966PlayerScript : SynapseAbstractRole
    {
        private readonly Scp966PluginClass _plugin;

        public Scp966PlayerScript(Scp966PluginClass plugin)
        {
            _plugin = plugin;
        }

        protected override IAbstractRoleConfig GetConfig() => _plugin.Config.Scp966Config;

        protected override void OnSpawn(IAbstractRoleConfig config)
        {
            base.OnSpawn(config);
            Player.Invisible = Synapse3.SynapseModule.Enums.InvisibleMode.Visual;
        }

        protected override void OnDeSpawn(DeSpawnReason reason)
        {
            base.OnDeSpawn(reason);
            Player.Invisible = Synapse3.SynapseModule.Enums.InvisibleMode.None;
        }

        public class Scp966Config : IAbstractRoleConfig
        {
            public RoleTypeId Role => RoleTypeId.Scp0492;

            public RoleTypeId VisibleRole => RoleTypeId.Scp0492;

            public RoleTypeId OwnRole => RoleTypeId.Scp0492;

            public uint EscapeRole => RoleService.NoneRole;

            public float Health => 600;

            public float MaxHealth => 500;

            public float ArtificialHealth => 0;

            public float MaxArtificialHealth => 0;

            public RoomPoint[] PossibleSpawns => new RoomPoint[]
            {
                new RoomPoint("Scp372", new UnityEngine.Vector3(4.14f, 1.71f, 2.81f), Vector3.zero)
            };

            public SerializedPlayerInventory[] PossibleInventories => new SerializedPlayerInventory[]
            {
                new SerializedPlayerInventory()
                {
                    Items=new List<SerializedPlayerItem>()
                    {
                        new SerializedPlayerItem((int)ItemType.Coin,0f, 0u, Vector3.one,100,false)
                    }
                }
            };

            public bool Hierarchy => false;

            public SerializedVector3 Scale => new Vector3(0.9f, 0.9f, 0.9f);
        }

    }
}

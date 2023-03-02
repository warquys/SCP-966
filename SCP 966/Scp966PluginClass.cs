using HarmonyLib;
using MEC;
using Neuron.Core.Events;
using Neuron.Core.Meta;
using Neuron.Core.Plugins;
using PlayerRoles;
using PluginAPI.Core;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Dummy;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;
using Synapse3.SynapseModule.Map.Objects;
using Synapse3.SynapseModule.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scp966
{
    [HeavyModded]
    [Plugin(
     Name = "SCP 966",
     Description = "Role pour les events",
     Version = "1.0.0",
     Author = "Morality & VT"
     )]
    public class Scp966PluginClass : ReloadablePlugin<Scp966PluginConfig, Scp966Translation>
    {
        public Dictionary<SynapsePlayer, SynapseLight> NightVisionPlayers { get; } = new Dictionary<SynapsePlayer, SynapseLight>();

        public Harmony Harmony { get; private set; }

        public override void EnablePlugin()
        {
            Harmony = new Harmony("Scp966");
            Harmony.PatchAll();
            base.EnablePlugin();
        }


        public override void Disable()
        {
            Harmony.UnpatchAll();
            base.Disable();
        }

        public void AddNightVisionPlayer(SynapsePlayer player)
        {
            if (NightVisionPlayers.ContainsKey(player)) return;
            SynapseLogger<Scp966PluginClass>.Warn("NightVisionPlayers Add");
            var light = new SynapseLight(Color.green, 2.5f, 120, false, Vector3.zero, new Quaternion(0,0,0,0), Vector3.one);
            light.HideFromAll();
            light.ShowPlayer(player);
            NightVisionPlayers.Add(player, light);
        }

        public void RemoveNightVisionPlayer(SynapsePlayer player)
        {
            if (player == null) return;
            if (!NightVisionPlayers.ContainsKey(player)) return;
            SynapseLogger<Scp966PluginClass>.Warn("NightVisionPlayers Remove");
            NightVisionPlayers[player].Destroy();
            NightVisionPlayers.Remove(player);
        }
    }

    [Automatic]
    public class Scp966EventHandler : Listener
    {
        Scp966PluginClass _plugin;

        CoroutineHandle NightVisionCoroutine;

        public Scp966EventHandler(Scp966PluginClass plugin, 
            PlayerEvents player, RoundEvents round, ScpEvents scp)
        {
            player.SendPlayerData.Subscribe(OnSendData);
            player.Leave.Subscribe(OnLeave);
#if DEBUG
            player.KeyPress.Subscribe(OnPressKey);
#endif
            round.Start.Subscribe(OnRoundStart);
            scp.Scp0492Attack.Subscribe(OnAttack);
            _plugin = plugin;
        }

        private void OnAttack(Scp0492AttackEvent ev)
        {
            if (ev.Scp.RoleID == 966)
            {
                ev.Victim.GiveEffect(Effect.Burned, 2, 5);
            }
        }

        private void OnLeave(LeaveEvent ev)
        {
            _plugin.RemoveNightVisionPlayer(ev.Player);
        }

        private void OnRoundStart(RoundStartEvent ev)
        {
            Timing.KillCoroutines(NightVisionCoroutine);
            Timing.RunCoroutine(NightVisonCoroutine());
            _plugin.NightVisionPlayers.Clear();
        }

        private IEnumerator<float> NightVisonCoroutine()
        {
            List<SynapsePlayer> playerToRemove = new List<SynapsePlayer>();
            while (true)
            {
                foreach (var player in _plugin.NightVisionPlayers)
                {
                    if (!player.Key.Inventory.Items.Any(p => p.Id == 138 || p.Id == 137))
                    {
                        playerToRemove.Add(player.Key);
                        continue;
                    }
                    player.Value.Position = player.Key.Position + Vector3.up;
                }
                if (playerToRemove.Count != 0)
                {
                    foreach (var player in playerToRemove)
                        _plugin.RemoveNightVisionPlayer(player);
                    playerToRemove.Clear();
                }
                yield return 0.05f;
            }

        }

#if DEBUG
        private void OnPressKey(KeyPressEvent ev)
        {
            switch (ev.KeyCode)
            {
                case KeyCode.Alpha1:
                    new SynapseItem(138, ev.Player);
                    break;
                case KeyCode.Alpha2:
                    _plugin.AddNightVisionPlayer(ev.Player);
                    break;
                case KeyCode.Alpha3:
                    _plugin.RemoveNightVisionPlayer(ev.Player);
                    break;
                case KeyCode.Alpha4:
                    var dummy = new SynapseDummy(ev.Player.Position, ev.Player.Rotation, RoleTypeId.ClassD, "SCP-966");
                    dummy.RaVisible = false;
                    dummy.DestroyWhenDied = true;
                    dummy.Player.RoleID = 966;
                    dummy.Player.Position = ev.Player.Position;
                    break;
            }
        }

#endif


        private void OnSendData(SendPlayerDataEvent ev)
        {

            SynapseLogger<Scp966EventHandler>.Warn(ev.PlayerToSee.RoleID == 966
                && (_plugin.NightVisionPlayers.ContainsKey(ev.Player)
                || ev.Player.TeamID == (uint)Team.SCPs
                || ev.Player.TeamID == (uint)Team.Dead));

            if (ev.PlayerToSee.RoleID == 966 
                && (_plugin.NightVisionPlayers.ContainsKey(ev.Player)
                || ev.Player.TeamID == (uint)Team.SCPs
                || ev.Player.TeamID == (uint)Team.Dead))
            {
                ev.IsInvisible = false;
            }
        }
    }
}

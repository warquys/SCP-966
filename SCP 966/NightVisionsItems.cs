using MEC;
using Neuron.Core.Meta;
using Scp966;
using Scp914;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;
using Synapse3.SynapseModule.Map.Scp914;
using Synapse3.SynapseModule.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.Random;

namespace Scp966
{
    [Automatic]
    [Scp914Processor(ReplaceHandlers = new uint[]
    {
        138
    })]
    [Item(
    Name = "Night Vison Heavy Armor",
    Id = 138,
    BasedItemType = ItemType.ArmorHeavy
    )]
    internal class NightVisionHeavy : CustomItemHandler, ISynapse914Processor
    {
        private readonly Scp966PluginClass _plugin;

        public NightVisionHeavy(Scp966PluginClass plugin, 
            ItemEvents items, PlayerEvents player) : base(items, player)
        {
            _plugin = plugin;
        }

        public override void OnThrow(ThrowGrenadeEvent ev)
        {
            if (ev.Allow) return;
            _plugin.RemoveNightVisionPlayer(ev.Player);
        }

        public override void OnPickup(PickupEvent ev)
        {
            SynapseLogger<NightVisionHeavy>.Warn("OnPickup");
            if (!ev.Allow) return;
            _plugin.AddNightVisionPlayer(ev.Player);
            var message = ev.Player.GetTranslation(_plugin.Translation).PickupNightVision;
            ev.Player.SendHint(message);
        }

        public override void OnDrop(DropItemEvent ev)
        {
            if (!ev.Allow) return;
            _plugin.RemoveNightVisionPlayer(ev.Player);
        }

        public void CreateUpgradedItem(SynapseItem item, Scp914KnobSetting setting, Vector3 position = default)
        {
            uint id = setting switch
            {
                Scp914KnobSetting.Rough =>
                UnityEngine.Random.Range(0, 100) > 50 ? (uint)ItemType.ArmorLight :
                                                        (uint)ItemType.ArmorCombat,
                Scp914KnobSetting.Coarse => 137,
                Scp914KnobSetting.OneToOne => (uint)ItemType.ArmorHeavy,
                Scp914KnobSetting.Fine => (uint)ItemType.GunE11SR,
                Scp914KnobSetting.VeryFine => (uint)ItemType.ArmorHeavy,
                _ => uint.MaxValue
            };

            if (id == uint.MaxValue) return;

            item.Destroy();

            var state = item.State;
            var owner = item.ItemOwner;

            switch (state)
            {
                case ItemState.Map:
                    {
                        var synapseItem = new SynapseItem(id, position);
                        if (synapseItem.IsCustomItem)
                            synapseItem.Scale = new Vector3(1.2f, 1.2f, 1);
                    }
                    return;

                case ItemState.Inventory:
                    {
                        var synapseItem = new SynapseItem(id, owner);
                        if (synapseItem.IsCustomItem)
                            synapseItem.Scale = new Vector3(1.2f, 1.2f, 1);
                    }
                    return;
            }
        }
    }

    [Automatic]
    [Scp914Processor(ReplaceHandlers = new uint[]
    {
        137
    })]
    [Item(
    Name = "Night Vison Combat Armor",
    Id = 137,
    BasedItemType = ItemType.ArmorCombat
    )]
    internal class NightVisionLight : CustomItemHandler, ISynapse914Processor
    {
        private readonly Scp966PluginClass _plugin;

        public NightVisionLight(Scp966PluginClass plugin,
            ItemEvents items, PlayerEvents player) : base(items, player)
        {
            _plugin = plugin;
        }

        public override void OnThrow(ThrowGrenadeEvent ev)
        {
            if (ev.Allow)
                _plugin.RemoveNightVisionPlayer(ev.Player);
        }

        public override void OnPickup(PickupEvent ev)
        {
            if (ev.Allow)
                _plugin.RemoveNightVisionPlayer(ev.Player);
        }

        public override void OnDrop(DropItemEvent ev)
        {
            if (ev.Allow)
                _plugin.RemoveNightVisionPlayer(ev.Player);
        }

        public void CreateUpgradedItem(SynapseItem item, Scp914KnobSetting setting, Vector3 position = default)
        {
            uint id = setting switch
            {
                Scp914KnobSetting.Rough =>
                UnityEngine.Random.Range(0, 100) > 50 ? (uint)ItemType.Flashlight :
                                                        (uint)ItemType.ArmorLight,
                Scp914KnobSetting.Coarse => (uint)ItemType.ArmorLight,
                Scp914KnobSetting.OneToOne => (uint)ItemType.ArmorCombat,
                Scp914KnobSetting.Fine => 138,
                Scp914KnobSetting.VeryFine => (uint)ItemType.ArmorCombat,
                _ => uint.MaxValue
            };

            if (id == uint.MaxValue) return;

            item.Destroy();

            var state = item.State;
            var owner = item.ItemOwner;

            switch (state)
            {
                case ItemState.Map:
                    {
                        var synapseItem = new SynapseItem(id, position);
                        if (synapseItem.IsCustomItem)
                            synapseItem.Scale = new Vector3(1.2f, 1.2f, 1);
                    }
                    return;

                case ItemState.Inventory:
                    {
                        var synapseItem = new SynapseItem(id, owner);
                        if (synapseItem.IsCustomItem)
                            synapseItem.Scale = new Vector3(1.2f, 1.2f, 1);
                    }
                    return;
            }
        }
    }

    [Automatic]
    [Scp914Processor(ReplaceHandlers = new uint[]
    {
        (uint)ItemType.ArmorCombat
    })]
    internal class LightArmorProcess
    {
        public void CreateUpgradedItem(SynapseItem item, Scp914KnobSetting setting, Vector3 position = default)
        {
            uint id = setting switch
            {
                Scp914KnobSetting.Rough =>
                UnityEngine.Random.Range(0, 100) > 50 ? (uint)ItemType.Flashlight :
                                                        (uint)ItemType.ArmorLight,
                Scp914KnobSetting.Coarse => (uint)ItemType.ArmorLight,
                Scp914KnobSetting.OneToOne => 137,
                Scp914KnobSetting.Fine => (uint)ItemType.ArmorHeavy,
                Scp914KnobSetting.VeryFine => 137,
                _ => uint.MaxValue
            };

            if (id == uint.MaxValue) return;

            item.Destroy();

            var state = item.State;
            var owner = item.ItemOwner;

            switch (state)
            {
                case ItemState.Map:
                    {
                        var synapseItem = new SynapseItem(id, position);
                        if (synapseItem.IsCustomItem)
                            synapseItem.Scale = new Vector3(1.2f, 1.2f, 1);
                    }
                    return;

                case ItemState.Inventory:
                    {
                        var synapseItem = new SynapseItem(id, owner);
                        if (synapseItem.IsCustomItem)
                            synapseItem.Scale = new Vector3(1.2f, 1.2f, 1);
                    }
                    return;
            }
        }
    }

    [Automatic]
    [Scp914Processor(ReplaceHandlers = new uint[]
    {
        (uint)ItemType.ArmorHeavy
    })]
    internal class CombatArmorProcess
    {
        public void CreateUpgradedItem(SynapseItem item, Scp914KnobSetting setting, Vector3 position = default)
        {
            uint id = setting switch
            {
                Scp914KnobSetting.Rough =>
                UnityEngine.Random.Range(0, 100) > 50 ? (uint)ItemType.ArmorLight :
                                                        (uint)ItemType.ArmorCombat,
                Scp914KnobSetting.Coarse => (uint)ItemType.ArmorCombat,
                Scp914KnobSetting.OneToOne => 138,
                Scp914KnobSetting.Fine => (uint)ItemType.ArmorHeavy,
                Scp914KnobSetting.VeryFine => (uint)ItemType.GunLogicer,
                _ => uint.MaxValue
            };

            if (id == uint.MaxValue) return;

            item.Destroy();

            var state = item.State;
            var owner = item.ItemOwner;

            switch (state)
            {
                case ItemState.Map:
                    {
                        var synapseItem = new SynapseItem(id, position);
                        if (synapseItem.IsCustomItem)
                            synapseItem.Scale = new Vector3(1.2f, 1.2f, 1);
                    }
                    return;

                case ItemState.Inventory:
                    {
                        var synapseItem = new SynapseItem(id, owner);
                        if (synapseItem.IsCustomItem)
                            synapseItem.Scale = new Vector3(1.2f, 1.2f, 1);
                    }
                    return;
            }

        }
    }

}

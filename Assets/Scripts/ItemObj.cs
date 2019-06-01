using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Miren
{
    [CreateAssetMenu]
    public class ItemObj : SerializedScriptableObject
    {
        [NonSerialized, OdinSerialize, OnValueChanged(nameof(OnItemChange))]
        public Item Item;

        private void OnItemChange()
        {
            if (Item != null)
            {
                Item.Name = name;
            }
        }
    }

    [Serializable]
    public class Item
    {
        public string Name;
        public ushort ID;

        public ItemType Type;

        public Item(string name, ushort id, ItemType type)
        {
            Name = name;
            ID = id;
            Type = type;
        }
    }

    [Serializable]
    public class Resource : Item
    {
        public Resource(string name, ushort id) : base(name, id, ItemType.Resource)
        {

        }
    }

    public class MapResource : Resource
    {
        [MinValue(1)]
        public float HarvestDifficulty;
        public MapResource(string name, ushort id) : base(name, id)
        {
            
        }
    }
}

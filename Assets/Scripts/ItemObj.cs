using System;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Miren
{
    [CreateAssetMenu]
    public class ItemObj : SerializedScriptableObject
    {
        [NonSerialized, OdinSerialize]
        public Item Item;
    }
    
    public class ItemCollection : KeyedCollection<ushort, Item>
    {
        protected override ushort GetKeyForItem(Item item)
        {
            return item.ID;
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

    public enum ItemType
    {
        Resource,
        
    }

    public unsafe struct ItemData
    {
        public ushort ID;
        public fixed byte Data[14];
    }

    public struct ResourceData
    {
        
    }
}

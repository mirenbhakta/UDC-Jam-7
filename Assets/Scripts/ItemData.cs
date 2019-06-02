using System;
using UnityEngine;

namespace Miren
{
    public enum ItemType
    {
        Resource,
        Factory,
    }

    /*
    public interface IItemData
    {
        ushort ID { get; }
    }

    public static class ItemDataExtensions
    {
        public static unsafe TFrom Convert<TFrom, T>(this T data)
            where TFrom : unmanaged, IItemData
            where T : unmanaged, IItemData
        {
            TFrom* t = (TFrom*) &data;
            return *t;
        }

        public static bool IsValid<T>(this T data)
            where T : IItemData
        {
            return data.ID != 0;
        }
    }
    
    [Serializable]
    public unsafe struct ItemData : IItemData
    {
        ushort IItemData.ID => ID;

        public ushort ID;
        public fixed byte Data[14];

        public ItemData(ushort id)
        {
            ID = id;
        }
    }

    [Serializable]
    public struct ResourceData : IItemData
    {
        ushort IItemData.ID => ID;

        public ushort ID;
        public uint Count;

        public ResourceData(ushort id)
        {
            ID = id;
            Count = 0;
        }

        public ResourceData(ushort id, uint count)
        {
            ID = id;
            Count = count;
        }
    }

    [Serializable]
    public struct FactoryData : IItemData
    {
        ushort IItemData.ID => ID;

        public ushort ID;
        public uint FactoryIndex;

        public FactoryData(ushort id, uint factoryIndex)
        {
            ID = id;
            FactoryIndex = factoryIndex;
        }
    }
    */
}

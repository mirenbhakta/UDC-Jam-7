using UnityEngine;

namespace Miren
{
    public enum ItemType
    {
        Resource,

    }

    public unsafe struct ItemData
    {
        public ushort ID;
        public fixed byte Data[14];

        public ItemData(ushort id)
        {
            ID = id;
        }
    }

    public struct ResourceData
    {
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
}

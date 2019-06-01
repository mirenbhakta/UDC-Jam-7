using System.Collections.ObjectModel;
using Sirenix.OdinInspector;

namespace Miren
{
    public class ItemCollectionObj : SerializedScriptableObject
    {
        public ItemCollection Collection;

        
    }

    public class ItemCollection : KeyedCollection<ushort, Item>
    {
        protected override ushort GetKeyForItem(Item item)
        {
            return item.ID;
        }
    }
}
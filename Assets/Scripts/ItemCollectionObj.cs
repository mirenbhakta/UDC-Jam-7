using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Miren
{
    [CreateAssetMenu]
    public class ItemCollectionObj : SerializedScriptableObject
    {
        [FolderPath]
        public string Path;
        
        public ItemCollection Collection;

        #if UNITY_EDITOR
        private void OnEnable()
        {
            ProcessCollection();
        }
        
        public void ProcessCollection()
        {
            if (Collection == null)
                Collection = new ItemCollection();
            else
                Collection.Clear();

            IEnumerable<ItemObj> items = from str in UnityEditor.AssetDatabase.FindAssets("t: ItemObj")
                select UnityEditor.AssetDatabase.LoadAssetAtPath<ItemObj>(UnityEditor.AssetDatabase.GUIDToAssetPath(str));

            foreach (ItemObj obj in items)
            {
                Collection.Add(obj.Item);
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }

    public class ItemCollection : KeyedCollection<ushort, Item>
    {
        protected override ushort GetKeyForItem(Item item)
        {
            return item.ID;
        }
    }
}

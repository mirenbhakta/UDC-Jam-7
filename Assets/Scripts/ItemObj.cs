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

        private void Awake()
        {
            Item?.OnAwake();
        }

        public Item DirectReference()
        {
            return Item;
        }
    }

    [Serializable]
    public class Item
    {
        public Sprite Icon;

        public string Name;
        public ushort ID;

        public ItemType Type;

        public Item()
        {
        }

        public Item(string name, ushort id, ItemType type)
        {
            Name = name;
            ID = id;
            Type = type;
        }

        internal virtual void OnAwake()
        {

        }
    }

    [Serializable]
    public class Resource : Item
    {
        public Resource()
        {
            Type = ItemType.Resource;
        }

        public Resource(string name, ushort id) : base(name, id, ItemType.Resource)
        {

        }
    }

    public abstract class RenderedItem : Item
    {
        public Material[] Materials;
        public Mesh Mesh;
    }

    public class MapResource : RenderedItem
    {
        [MinValue(1)]
        public float HarvestDifficulty;

        [SerializeField]
        [LabelText("Yield Resource")]
        [ValidateInput(nameof(ValidateResource), "Item must be ItemType.Resource and must not be another MapResource!")]
        private ItemObj editorYield;

        private static bool ValidateResource(ItemObj obj)
        {
            if (obj == null)
            {
                return true;
            }

            return obj.Item.Type == ItemType.Resource && !(obj.Item is MapResource);
        }

        [NonSerialized]
        public Resource Yield;

        public MapResource()
        {
            Type = ItemType.Resource;
        }

        internal override void OnAwake()
        {
            Yield = editorYield.DirectReference() as Resource;
        }
    }

    public struct RecipeItem
    {
        public Item Item;
        public uint Count;

        public RecipeItem(Item item, uint count)
        {
            Item = item;
            Count = count;
        }
    }

    public class Factory : RenderedItem
    {
        public float SpeedMultiplier;

        [SerializeField]
        private FactoryRecipe[] factoryRecipes;

        public Factory()
        {
            Type = ItemType.Factory;
        }

        internal override void OnAwake()
        {
            for (int i = 0; i < factoryRecipes.Length; i++)
            {
                factoryRecipes[i].OnAwake();
            }
        }
    }
}

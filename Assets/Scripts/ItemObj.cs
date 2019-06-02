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
                if (Item is MapResource mapResource)
                {
                    mapResource.OnValueChanged(this);
                }
            }
        }

        private void Awake()
        {
            Item.OnAwake();
        }

        public Item DirectReference()
        {
            return Item;
        }
    }

    [Serializable]
    public class Item
    {
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
        }

        public Resource(string name, ushort id) : base(name, id, ItemType.Resource)
        {

        }
    }

    public class MapResource : Resource
    {
        [MinValue(1)]
        public float HarvestDifficulty;

        [SerializeField]
        [LabelText("Yield Resource")]
        [ValidateInput(nameof(ValidateResource), "Item must be ItemType.Resource!")]
        private ItemObj editorYield;

        private static bool ValidateResource(ItemObj obj)
        {
            if (obj == null)
            {
                return true;
            }

            return obj.Item.Type == ItemType.Resource;
        }

        internal void OnValueChanged(ItemObj obj)
        {
            editorYield = obj;
        }

        [NonSerialized]
        public Resource Yield;

        public MapResource()
        {
        }

        public MapResource(string name, ushort id) : base(name, id)
        {

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
        public float Time;

        public RecipeItem(Item item, uint count, float time)
        {
            Item = item;
            Count = count;
            Time = time;
        }
    }

    public class Factory : Item
    {
        [SerializeField]
        private FactoryRecipe factoryRecipe;

        [NonSerialized]
        public RecipeItem[] Ingredients, Products;

        public Factory(string name, ushort id) : base(name, id, ItemType.Factory)
        {

        }

        internal override void OnAwake()
        {
            Ingredients = factoryRecipe.IngredientReferences();
            Products = factoryRecipe.ProductReferences();
        }
    }

    public struct FactoryRecipe
    {
        public struct EditorRecipeItem
        {
            public ItemObj Item;

            [MinValue(1)]
            public uint Count;

            public float Time;
        }

        [SerializeField, LabelText("Consumed Items")]
        private EditorRecipeItem[] editorItemIngredients;

        [SerializeField, LabelText("Produced Items")]
        private EditorRecipeItem[] editorItemProducts;

        public RecipeItem[] IngredientReferences()
        {
            RecipeItem[] items = new RecipeItem[editorItemIngredients.Length];
            for (int i = 0; i < items.Length; i++)
            {
                EditorRecipeItem e = editorItemIngredients[i];
                items[i] = new RecipeItem(e.Item.DirectReference(), e.Count, e.Time);
            }

            return items;
        }

        public RecipeItem[] ProductReferences()
        {
            RecipeItem[] items = new RecipeItem[editorItemProducts.Length];
            for (int i = 0; i < editorItemProducts.Length; i++)
            {
                EditorRecipeItem e = editorItemProducts[i];
                items[i] = new RecipeItem(e.Item.DirectReference(), e.Count, e.Time);
            }

            return items;
        }
    }
}

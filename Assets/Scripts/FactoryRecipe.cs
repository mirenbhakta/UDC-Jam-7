using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Miren
{
    [CreateAssetMenu]
    public class FactoryRecipe : SerializedScriptableObject
    {
        [SerializeField]
        internal ushort id;

        private struct EditorRecipeItem
        {
            public ItemObj Item;

            [MinValue(1)]
            public uint Count;
        }

        public float Time;

        [SerializeField, LabelText("Consumed Items"), ListDrawerSettings(Expanded = true)]
        private EditorRecipeItem[] editorItemIngredients = new EditorRecipeItem[1];

        [SerializeField, LabelText("Produced Items"), ListDrawerSettings(Expanded = true)]
        private EditorRecipeItem[] editorItemProducts = new EditorRecipeItem[1];

        [NonSerialized]
        public RecipeItem[] Ingredients, Products;

        public void OnAwake()
        {
            Ingredients = new RecipeItem[editorItemIngredients.Length];
            for (int i = 0; i < editorItemIngredients.Length; i++)
            {
                EditorRecipeItem e = editorItemIngredients[i];
                Ingredients[i] = new RecipeItem(e.Item.DirectReference(), e.Count);
            }

            Products = new RecipeItem[editorItemProducts.Length];
            for (int i = 0; i < editorItemProducts.Length; i++)
            {
                EditorRecipeItem e = editorItemProducts[i];
                Products[i] = new RecipeItem(e.Item.DirectReference(), e.Count);
            }
        }
    }
}

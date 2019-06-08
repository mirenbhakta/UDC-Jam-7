using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Miren
{
    [CreateAssetMenu]
    public class FactoryRecipeCollectionObj : SerializedScriptableObject
    {
        [FolderPath]
        public string Path;

        public FactoryRecipeCollection Collection;

#if UNITY_EDITOR
        [Button("Refresh")]
        public void ProcessCollection()
        {
            if (Collection == null)
                Collection = new FactoryRecipeCollection();
            else
                Collection.Clear();

            IEnumerable<FactoryRecipe> items = from str in UnityEditor.AssetDatabase.FindAssets("t: FactoryRecipe")
                select UnityEditor.AssetDatabase.LoadAssetAtPath<FactoryRecipe>(
                    UnityEditor.AssetDatabase.GUIDToAssetPath(str));

            foreach (FactoryRecipe obj in items)
            {
                if (Collection.Contains(obj))
                {
                    continue;
                }
                
                try
                {
                    Collection.Add(obj);
                }
                catch (ArgumentException e)
                {
                    ushort i;
                    for (i = 0; i < Collection.Count(); i++)
                    {
                        if (!Collection.Contains(i))
                        {
                            break;
                        }
                    }

                    obj.id = i;
                    Collection.Add(obj);
                }
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

    public class FactoryRecipeCollection : KeyedCollection<ushort, FactoryRecipe>
    {
        protected override ushort GetKeyForItem(FactoryRecipe item)
        {
            return item.id;
        }
    }
}

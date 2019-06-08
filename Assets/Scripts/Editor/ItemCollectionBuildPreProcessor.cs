using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Miren.Editor
{
    public class ItemCollectionBuildPreProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => -100;

        public void OnPreprocessBuild(BuildReport report)
        {
            IEnumerable<ItemCollectionObj> collections = from str in AssetDatabase.FindAssets("t:ItemCollectionObj")
                select AssetDatabase.LoadAssetAtPath<ItemCollectionObj>(AssetDatabase.GUIDToAssetPath(str));

            foreach (ItemCollectionObj collection in collections)
            {
                collection.ProcessCollection();
            }

            IEnumerable<FactoryRecipeCollectionObj> recipes =
                from str in AssetDatabase.FindAssets("t:FactoryRecipeCollectionObj")
                select AssetDatabase.LoadAssetAtPath<FactoryRecipeCollectionObj>(AssetDatabase.GUIDToAssetPath(str));

            foreach (FactoryRecipeCollectionObj obj in recipes)
            {
                obj.ProcessCollection();
            }
        }
    }
}

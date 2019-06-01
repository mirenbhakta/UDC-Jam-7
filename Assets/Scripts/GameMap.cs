using System;
using UnityEngine;

namespace Miren
{
    [RequireComponent(typeof(TerrainGenerator))]
    public class GameMap : MonoBehaviour
    {
        [SerializeField]
        private TerrainGenerator generator;

        [SerializeField]
        private ItemCollectionObj items;
        
        
        private void Awake()
        {
            generator.Init();
        }
    }
}

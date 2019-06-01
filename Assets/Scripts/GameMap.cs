using System;
using UnityEngine;

namespace Miren
{
    [RequireComponent(typeof(TerrainGenerator))]
    public class GameMap : MonoBehaviour
    {
        [SerializeField]
        private TerrainGenerator generator;

        private void Awake()
        {
            generator.Init();
        }
    }
}

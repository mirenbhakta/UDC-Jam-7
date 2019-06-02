using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.Mathematics;

using Random = Unity.Mathematics.Random;

namespace Miren
{
    [RequireComponent(typeof(TerrainGenerator))]
    public class GameMap : MonoBehaviour
    {
        [SerializeField]
        internal TerrainGenerator generator;

        [SerializeField]
        internal ItemCollectionObj items;

        [SerializeField]
        private int[] sizes;

        [SerializeField]
        private bool generateRandom;

        [SerializeField]
        private NoiseSettings settings;

        [SerializeField]
        private uint seed;

        [SerializeField]
        private MapSize mapSize;

        [SerializeField]
        private float mapHeight;

        private void Awake()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            if (generateRandom)
            {
                seed = (uint) Environment.TickCount;
            }

            settings.Init(seed);
            int size = sizes[(int) mapSize];
            float[,] heightMap = generator.Init(size, settings, mapSize, mapHeight);

            
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(seed);
            writer.Write((int) mapSize);
        }

        public void Load(BinaryReader reader)
        {
            seed = reader.ReadUInt32();
            mapSize = (MapSize) reader.ReadInt32();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (sizes.Length == 3) return;

            int[] n = new int[3];
            Array.Copy(sizes, n, Mathf.Min(n.Length, sizes.Length));
            sizes = n;
        }
#endif
    }
}

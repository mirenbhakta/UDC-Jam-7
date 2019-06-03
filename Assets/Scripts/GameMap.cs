using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

namespace Miren
{
    [RequireComponent(typeof(TerrainGenerator), typeof(ResourceGenerator))]
    public class GameMap : MonoBehaviour
    {
        [SerializeField]
        internal TerrainGenerator terrainGenerator;

        [SerializeField]
        internal ResourceGenerator resourceGenerator;

        [SerializeField]
        internal ItemCollectionObj items;

        [SerializeField]
        private int[] mapSizes;

        [SerializeField]
        public MapResourceObject mapResourcePrefab;

        [SerializeField]
        private FactoryObject factoryPrefab;

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
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            if (generateRandom)
            {
                seed = (uint) Environment.TickCount;
            }

            Random rand = new Random(seed);

            settings.Init(rand);
            int size = mapSizes[(int) mapSize];
            float[,] heightMap = terrainGenerator.Generate(size, settings, mapSize, mapHeight);

            resourceGenerator.GenerateResources(rand, terrainGenerator.terrain, size, mapHeight);
            sw.Stop();
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
            if (mapSizes.Length == 3) return;

            int[] n = new int[3];
            Array.Copy(mapSizes, n, Mathf.Min(n.Length, mapSizes.Length));
            mapSizes = n;
        }
#endif
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Miren.UnityToolbag;
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
        private uint seed;

        [SerializeField]
        private MapSize mapSize;

        [SerializeField]
        private float mapHeight;

        private MapResourceObject[] resourceInstances;

        private string mapName;
        
        internal event Action OnGenerate;

        private void Awake()
        {
            //GenerateMap();
        }

        public void SetMapSize(int size)
        {
            mapSize = (MapSize) size;
        }

        public void SetGenerateRandom(bool gen)
        {
            generateRandom = gen;
        }

        public void SetSeed(string seed)
        {
            uint.TryParse(seed, out this.seed);
        }

        public void GenerateMap()
        {
            if (resourceInstances != null)
            {
                for (int i = 0; i < resourceInstances.Length; i++)
                {
                    MapResourceObject instance = resourceInstances[i];
                    if (instance != null)
                    {
                        Destroy(instance.gameObject);
                    }
                }
            }

            if (generateRandom || seed == 0)
            {
                seed = (uint) Environment.TickCount;
            }

            Random rand = new Random(seed);

            int size = mapSizes[(int) mapSize];
            terrainGenerator.Generate(size, rand, mapHeight);

            resourceInstances = resourceGenerator.GenerateResources(rand, terrainGenerator.terrain, size);

            OnGenerate?.Invoke();
        }

        public void Save()
        {
            using (FileStream fs = new FileStream(Path.Combine(StandardPaths.saveDataDirectory, mapName),
                FileMode.OpenOrCreate,
                FileAccess.Write))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {

                }
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(seed);
            writer.Write((int) mapSize);

            for (int i = 0; i < resourceInstances.Length; i++)
            {
                MapResourceObject instance = resourceInstances[i];
                if (instance == null)
                {
                    writer.Write(-i);
                    continue;
                }

                writer.Write(i);
                instance.Save(writer);
            }
        }

        public void Load(BinaryReader reader)
        {
            seed = reader.ReadUInt32();
            mapSize = (MapSize) reader.ReadInt32();

            int size = mapSizes[(int) mapSize];
            int featureSize = size / 16;

            resourceInstances = new MapResourceObject[featureSize * featureSize];

            for (int i = 0; i < resourceInstances.Length; i++)
            {
                int index = reader.ReadInt32();
                if (index < 0)
                {
                    continue;
                }

                MapResourceObject obj = resourceGenerator.CreateInstance();
                obj.Load(reader, items.Collection);
            }
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

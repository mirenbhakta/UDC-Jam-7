using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using JetBrains.Annotations;
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
        private CameraController cameraController;

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

        public void Clear()
        {
            if (resourceInstances == null) return;
            for (int i = 0; i < resourceInstances.Length; i++)
            {
                MapResourceObject instance = resourceInstances[i];
                if (instance != null)
                {
                    Destroy(instance.gameObject);
                }
            }
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

            cameraController.Init();
        }

        public const string SaveFileExtension = ".save";

        private const int Version = 4;

        public static FileInfo GetSavePath(string fileName)
        {
            return new FileInfo(Path.Combine(StandardPaths.saveDataDirectory, fileName));
        }

        public void Save(FileInfo file)
        {
            mapName = file.Name;

            using (FileStream fs = file.Exists ? file.OpenWrite() : file.Create())
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    writer.Write(Version);
                    Save(writer);
                }
            }
        }

        public void Save()
        {
            if (mapName == null)
            {
                mapName = "saveOnQuit";
            }

            Save(GetSavePath(mapName + SaveFileExtension));
        }

        [CanBeNull]
        public string Load(FileData data)
        {
            this.mapName = data.Info.Name;
            using (FileStream fs = data.Info.OpenRead())
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    int version = reader.ReadInt32();
                    if (Version != version)
                    {
                        return "File cannot be loaded because it is an outdated version.";
                    }

                    Load(reader);
                }
            }

            return null;
        }

        private void Save(BinaryWriter writer)
        {
            writer.Write(seed);
            writer.Write((int) mapSize);

            int length = resourceInstances?.Length ?? 0;
            writer.Write(length);

            for (int i = 0; i < length; i++)
            {
                MapResourceObject instance = resourceInstances[i];
                if (instance == null)
                {
                    writer.Write(-i - 1);
                    continue;
                }

                writer.Write(i);
                instance.Save(writer);
            }
        }

        private void Load(BinaryReader reader)
        {
            seed = reader.ReadUInt32();
            mapSize = (MapSize) reader.ReadInt32();

            int size = mapSizes[(int) mapSize];

            terrainGenerator.Generate(size, new Random(seed), mapHeight);

            int length = reader.ReadInt32();

            resourceInstances = new MapResourceObject[length];

            for (int i = 0; i < length; i++)
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

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Miren
{
    public class ResourceGenerator : SerializedMonoBehaviour
    {
        [SerializeField]
        private MapResourceObject resourcePrefab;

        [SerializeField]
        private FactoryObject factoryPrefab;
        
        [SerializeField]
        private ItemObj[] ironResources;

        [SerializeField]
        private ItemObj[] copperResources;

        [SerializeField]
        private ItemObj[] coalResources;

        [SerializeField]
        private ItemObj[] factories;

        private ItemObj[] everything;

        private void Awake()
        {
            everything = ironResources.Concat(copperResources).Concat(coalResources).Concat(factories).ToArray();
        }

        public MapResourceObject[] GenerateResources(Random rand, Terrain terrain, int size)
        {
            int featureSize = size / 16;
            int halfSize = size / 2 - 16 / 2;

            MapResourceObject[] mapResourceObjects = new MapResourceObject[featureSize * featureSize];

            for (int z = 0; z < featureSize; z++)
            {
                for (int x = 0; x < featureSize; x++)
                {
                    uint index = rand.NextUInt(0, 5);

                    ItemObj[] resources;

                    switch (index)
                    {
                        default:
                            continue;
                        case 0:
                            resources = ironResources;
                            break;
                        case 1:
                            resources = copperResources;
                            break;
                        case 2:
                            resources = coalResources;
                            break;
                    }

                    if (resources.Length == 0)
                    {
                        continue;
                    }

                    ItemObj obj = resources[rand.NextInt(0, resources.Length)];

                    switch (obj.Item)
                    {
                        case MapResource resourceObject:
                        {
                            MapResourceObject instance = mapResourceObjects[x + z * featureSize] =
                                Instantiate(resourcePrefab, transform);
                            Vector3 pos = new Vector3(x * 16 - halfSize, 0, z * 16 - halfSize);
                            pos.y = terrain.SampleHeight(pos);

                            Quaternion rotation = Quaternion.Euler(0, rand.NextFloat(-180, 180), 0);

                            instance.Init(pos, rotation, obj.Item as MapResource, rand.NextUInt(3000, 6000));
                            break;
                        }

                        case Factory factory:

                            break;
                    }


                }
            }

            return mapResourceObjects;
        }


        public MapResourceObject CreateInstance()
        {
            return Instantiate(resourcePrefab, transform);
        }
    }
}

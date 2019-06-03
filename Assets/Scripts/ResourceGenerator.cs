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
        private NoiseSettings ironSettings;

        [SerializeField]
        private ItemObj[] ironResources;

        [SerializeField]
        private NoiseSettings copperSettings;

        [SerializeField]
        private ItemObj[] copperResources;

        [SerializeField]
        private NoiseSettings coalSettings;

        [SerializeField]
        private ItemObj[] coalResources;

        public void GenerateResources(Random rand, Terrain terrain, int size, float mapHeight)
        {
            int halfSize = size / 2;
            int featureSize = size / 16;

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

                    MapResourceObject instance = Instantiate(resourcePrefab, transform);
                    Vector3 pos = new Vector3(x * 16 - halfSize, 0, z * 16 - halfSize);
                    pos.y = terrain.SampleHeight(pos);

                    instance.Init(pos, obj.Item as MapResource);
                }
            }
        }
    }
}
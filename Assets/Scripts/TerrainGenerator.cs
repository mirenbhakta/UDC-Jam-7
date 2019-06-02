using Unity.Mathematics;
using UnityEngine;

namespace Miren
{
	public enum MapSize
	{
		Small = 0,
		Medium = 1,
		Large = 2,
	}

	[RequireComponent(typeof(Terrain), typeof(TerrainCollider))]
	public class TerrainGenerator : MonoBehaviour
	{
		[SerializeField]
		internal Terrain terrain;

		[SerializeField]
		private TerrainCollider terrainCollider;

		public float[,] Init(int size, NoiseSettings settings, MapSize mapSize, float mapHeight)
		{
			TerrainData terrainData = new TerrainData
			{
				heightmapResolution = size,
				size = new Vector3(size, mapHeight, size)
			};

			float[,] heightMap = Generate(terrainData, settings, size);

			terrain.terrainData = terrainData;
			terrainCollider.terrainData = terrainData;
			transform.position = new Vector3(size, 0, size) * -0.5f;

			return heightMap;
		}

		private float[,] Generate(TerrainData data, NoiseSettings settings, int size)
		{
			size += 1;
			float[,] heights = new float[size, size];
			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					float height = settings.Generate(new float2(y, x));
					heights[y, x] = height;
				}
			}

			data.SetHeights(0, 0, heights);
			return heights;
		}
	}
}

using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

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

		[SerializeField]
		private TerrainLayer grass, sand, rock;

		[SerializeField]
		private NoiseSettings heightSettings;

		[SerializeField]
		private NoiseSettings moistureSettings;

		[SerializeField]
		private NoiseSettings temperatureSettings;

		public void Generate(int size, Random rand, float mapHeight)
		{
			heightSettings.Init(ref rand);
			moistureSettings.Init(ref rand);
			temperatureSettings.Init(ref rand);

			if (terrain.terrainData != null)
			{
				Destroy(terrain.terrainData);
			}

			TerrainData terrainData = new TerrainData
			{
				heightmapResolution = size,
				size = new Vector3(size, mapHeight, size)
			};

			Generate(terrainData, size);

			terrain.terrainData = terrainData;
			terrainCollider.terrainData = terrainData;
			transform.position = new Vector3(size, 0, size) * -0.5f;
		}

		private void Generate(TerrainData terrainData, int size)
		{
			TerrainLayer[] layers = new[] {grass, rock, sand};

			terrainData.terrainLayers = layers;

			int heightMapSize = size + 1;
			float[,] heights = new float[heightMapSize, heightMapSize];

			for (int x = 0; x < heightMapSize; x++)
			{
				for (int y = 0; y < heightMapSize; y++)
				{
					float2 p = new float2(y, x);
					float height = heightSettings.GetFBM(p);

					heights[y, x] = height;
				}
			}

			int alphaSize = terrainData.alphamapResolution = size;

			float[,,] alpha = new float[alphaSize, alphaSize, layers.Length];

			for (int x = 0; x < alphaSize; x++)
			{
				for (int y = 0; y < alphaSize; y++)
				{
					float2 p = new float2(y, x);
					float temp = Scale(temperatureSettings.GetBillow(p));
					float moisture = Scale(moistureSettings.GetBillow(p));

					alpha[x, y, 0] = moisture;
					alpha[x, y, 1] = temp;

					alpha[x, y, 2] = 1 - (moisture + temp) / 2;
				}
			}

			terrainData.SetHeights(0, 0, heights);
			terrainData.SetAlphamaps(0, 0, alpha);
		}

		internal static float Scale(float x)
		{
			return (Mathf.Clamp(x * 20, -1, 1) + 1) / 2;
		}

		internal static float Hermite(float x)
		{
			return x * x * (3 - 2 * x);
		}

		internal static float Quintic(float x)
		{
			return x * x * x * (x * (x * 6 - 15) + 10);
		}
	}
}

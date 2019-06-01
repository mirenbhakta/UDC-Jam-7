using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

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
		private int[] sizes;

		[SerializeField]
		private Terrain terrain;

		[SerializeField]
		private TerrainCollider terrainCollider;

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
		
		public void Init()
		{
			if (generateRandom)
			{
				seed = (uint) Environment.TickCount;
			}

			settings.Init(seed);

			TerrainData terrainData = new TerrainData();

			int size = sizes[(int) mapSize];
			terrainData.heightmapResolution = size;
			terrainData.size = new Vector3(size, mapHeight, size);

			Generate(terrainData, size);

			terrain.terrainData = terrainData;
			terrainCollider.terrainData = terrainData;
			transform.position = new Vector3(size, 0, size) * -0.5f;
		}

		private void Generate(TerrainData data, int size)
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
		}

		private void OnValidate()
		{
			if (sizes.Length == 3) return;

			int[] n = new int[3];
			Array.Copy(sizes, n, Mathf.Min(n.Length, sizes.Length));
			sizes = n;
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
	}
}

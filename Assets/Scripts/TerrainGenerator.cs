using System;
using System.Collections.Generic;
using System.IO;
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
		private int[] sizes;

		[SerializeField]
		private Terrain terrain;

		[SerializeField]
		private TerrainCollider terrainCollider;

		public bool GenerateRandom;

		public NoiseSettings Settings;

		public uint Seed;

		public MapSize MapSize;

		public void Awake()
		{
			if (GenerateRandom)
			{
				Seed = (uint) Environment.TickCount;
			}

			Settings.Init(Seed);

			TerrainData terrainData = new TerrainData();

			int size = sizes[(int) MapSize];
			terrainData.heightmapResolution = size;
			terrainData.size = new Vector3(size, 5, size);

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
					float height = Settings.Generate(new float2(y, x));
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
			writer.Write(Seed);
			writer.Write((int) MapSize);
		}

		public void Load(BinaryReader reader)
		{
			Seed = reader.ReadUInt32();
			MapSize = (MapSize) reader.ReadInt32();
		}
	}
}

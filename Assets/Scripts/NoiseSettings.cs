using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

using Random = Unity.Mathematics.Random;

namespace Miren
{
	[Serializable]
	public unsafe struct NoiseSettings
	{
		public float Frequency;

		[Range(1, 16)]
		public int Octaves;

		[MinValue(1)]
		public float Lacunarity;

		[Range(0.00001f, 1)]
		public float Gain;

		private float fractalBounding;

		private fixed float offsets[32];

		public void Init(uint seed)
		{
			Random rand = new Random(seed);
			Init(rand);
		}

		public void Init(Random rand)
		{
			for (int i = 0; i < Octaves * 2; i++)
			{
				offsets[i] = rand.NextFloat();

			}

			float amp = Gain;
			float ampFractal = 1f;
			for (int i = 0; i < Octaves; i++)
			{
				ampFractal += amp;
				amp *= Gain;
			}

			fractalBounding = 1f / ampFractal;
		}

		public float Generate(float2 pos)
		{
			float2 p = pos * Frequency;

			float sum = 0;
			float amp = 1;

			for (int i = 0; i < Octaves; i++)
			{
				p *= Lacunarity;
				int i2 = i * 2;
				float2 n = p + new float2(offsets[i2], offsets[i2 + 1]);
				sum += Mathf.PerlinNoise(n.x, n.y) * amp;
				amp *= Gain;
			}

			return sum * fractalBounding;
		}
	}
}

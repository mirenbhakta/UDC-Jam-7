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
			Init(ref rand);
		}

		public void Init(ref Random rand)
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

		public float GetFBM(float2 pos)
		{
			pos *= Frequency;

			float sum = Noise(pos.x + offsets[0], pos.y + offsets[1]);
			float amp = 1;

			for (int i = 1; i < Octaves; i++)
			{
				pos *= Lacunarity;
				amp *= Gain;
				int i2 = i * 2;
				float2 n = pos + new float2(offsets[i2], offsets[i2 + 1]);
				sum += Noise(n) * amp;
			}

			return sum * fractalBounding;
		}

		public float GetBillow(float2 pos)
		{
			pos *= Frequency;

			float sum = Mathf.Abs(Noise(pos.x + offsets[0], pos.y + offsets[1])) * 2 - 1;
			float amp = 1;

			for (int i = 1; i < Octaves; i++)
			{
				pos *= Lacunarity;
				amp *= Gain;
				int i2 = i * 2;
				float2 n = pos + new float2(offsets[i2], offsets[i2 + 1]);
				sum += (Mathf.Abs(Noise(n)) * 2 - 1) * amp;
			}

			return sum * fractalBounding;
		}

		public float GetRigidMulti(float2 pos)
		{
			pos *= Frequency;
			float sum = 1 - Mathf.Abs(Noise(pos.x + offsets[0], pos.y + offsets[1]));
			float amp = 1;

			for (int i = 1; i < Octaves; i++)
			{
				pos *= Lacunarity;
				amp *= Gain;
				int i2 = i * 2;
				float2 n = pos + new float2(offsets[i2], offsets[i2 + 1]);
				sum -= (1 - Mathf.Abs(Noise(n))) * amp;
			}

			return sum;
		}

		private static float Noise(float2 n)
		{
			return OpenSimplex.Generate(n.x, n.y);
		}

		private static float Noise(float x, float y)
		{
			return Noise(new float2(x, y));
		}
	}
}

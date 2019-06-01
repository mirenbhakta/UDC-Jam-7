#if UNITY_EDITOR

using System;
using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miren
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class NoiseTest : MonoBehaviour
    {
        [SerializeField]
        private Material mat;

        [SerializeField]
        private NoiseSettings settings;
    
        private Texture2D tex;

        [SerializeField]
        private uint seed;
        
        [SerializeField]
        private bool autoUpdate;
        
        [SerializeField]
        private bool generate;
        
        
        void Start()
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer.material != null)
            {
                DestroyImmediate(renderer.material);
            }
            renderer.material = mat;

            renderer.material.mainTexture = tex = new Texture2D(128, 128);
        }

        private void OnValidate()
        {
            if (generate || autoUpdate)
            {
                settings.Init(seed);
                Generate();
                generate = false;
            }
        }

        private void Generate()
        {
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    float noise = settings.Generate(new float2(x, y));
                    tex.SetPixel(x, y, Color.Lerp(Color.white, Color.black, noise));
                }
            }
            tex.Apply();
        }
    }
}

#endif

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Miren
{
    public class ItemObjectBase<T> : SerializedMonoBehaviour
        where T : Item
    {
        [SerializeField]
        internal Transform rendererObject;

        [NonSerialized]
        public T Item;

        public void SetPosition(Vector3 pos)
        {
            Vector3 colliderPos = new Vector3(pos.x, pos.z, 0);
            transform.position = colliderPos;
            rendererObject.position = pos;
        }
    }

    public class MapResourceObject : ItemObjectBase<MapResource>
    {
        /*
        [SerializeField]
        [ValueDropdown(nameof(VariantDropdown))]
        private int itemVariant;

        private ValueDropdownList<int> VariantDropdown()
        {
            ValueDropdownList<int> list = new ValueDropdownList<int>();
            MapResourceVariant[] variants = (itemObject.DirectReference() as MapResource).Variants;
            for (int i = 0; i < variants.Length; i++)
            {
                var variant = variants[i];
                list.Add(variant.Name, i);
            }

            return list;
        }*/

        public uint Count;

        public void Init(Vector3 position, MapResource resource)
        {
            MeshFilter filter = rendererObject.GetComponent<MeshFilter>();
            filter.sharedMesh = resource.Mesh;
            MeshRenderer renderer = rendererObject.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = resource.Material;

            SetPosition(position);
        }
    }
}

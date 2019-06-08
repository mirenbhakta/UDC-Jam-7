using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Miren
{
    public abstract class ItemObjectBase : SerializedMonoBehaviour
    {
        public abstract void Save(BinaryWriter writer, object data);
        public abstract void Load(BinaryReader reader, object data);
    }

    public abstract class ItemObjectBase<T> : ItemObjectBase
        where T : Item
    {
        [SerializeField]
        internal Transform rendererObject;

        [NonSerialized]
        public T Item;

        protected void SetPosition(Vector3 pos)
        {
            transform.position = PlayerController.GetColliderPosition(pos);
            rendererObject.position = pos;
        }

        protected void SetRotation(Quaternion rotation)
        {
            rendererObject.localRotation = rotation;
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

        public void Init(Vector3 position, Quaternion rotation, MapResource resource, uint count)
        {
            Item = resource;
            Count = count;

            MeshFilter filter = rendererObject.GetComponent<MeshFilter>();
            filter.sharedMesh = resource.Mesh;
            MeshRenderer renderer = rendererObject.GetComponent<MeshRenderer>();
            renderer.sharedMaterials = resource.Materials;

            Vector3 size = resource.Mesh.bounds.size;
            size.x = Mathf.Round(size.x - 2f);
            size.z = Mathf.Round(size.z - 2f);
            Collider2D coll = GetComponent<Collider2D>();
            switch (coll)
            {
                case BoxCollider2D box:
                    box.size = new Vector2(size.x, size.z);
                    break;
                case CircleCollider2D circle:
                    circle.radius = Mathf.Max(size.x, size.z);
                    break;
            }

            SetRotation(rotation);
            SetPosition(position);
        }

        public override void Save(BinaryWriter writer, object data)
        {
            writer.Write(Item.ID);
            writer.Write(Count);
            Vector3 pos = rendererObject.position;
            writer.Write(pos.x);
            writer.Write(pos.y);
            writer.Write(pos.z);
            writer.Write(rendererObject.localEulerAngles.y);
        }

        public override void Load(BinaryReader reader, object data)
        {
            ItemCollection items = data as ItemCollection;
            ushort id = reader.ReadUInt16();
            MapResource item = items[id] as MapResource;
            uint count = reader.ReadUInt32();

            Vector3 pos;
            pos.x = reader.ReadSingle();
            pos.y = reader.ReadSingle();
            pos.z = reader.ReadSingle();

            Quaternion rot = Quaternion.Euler(0, reader.ReadSingle(), 0);

            Init(pos, rot, item, count);
        }
    }
}

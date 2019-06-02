using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Miren
{
    public class ResourceObject : SerializedMonoBehaviour
    {
        [SerializeField, LabelText("Item")]
        private ItemObj editorItem;

        [NonSerialized]
        public Item Item;
        
        public int Count;

        private void Awake()
        {
            Item = editorItem.DirectReference();
        }
    }
}
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Miren
{
    public class FileData : MonoBehaviour
    {
        [SerializeField]
        private Text label;

        [SerializeField]
        internal Toggle toggle;

        public FileInfo Info;

        public void SetInfo(FileInfo info)
        {
            label.text = Path.GetFileNameWithoutExtension(info.Name);
            Info = info;
        }
    }
}

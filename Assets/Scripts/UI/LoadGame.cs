using System.IO;
using System.Text;
using Miren.UnityToolbag;
using UnityEngine;
using UnityEngine.UI;

namespace Miren
{
    public class LoadGame : MonoBehaviour
    {
        private static readonly string searchFilter = "*" + GameMap.SaveFileExtension;

        [SerializeField]
        private Text text;

        private void OnEnable()
        {
            string path = StandardPaths.saveDataDirectory;
            DirectoryInfo dir = new DirectoryInfo(path);

            FileInfo[] infos = dir.GetFiles(searchFilter, SearchOption.AllDirectories);

            StringBuilder sb = new StringBuilder(infos.Length * 20);

            for (int i = 0; i < infos.Length; i++)
            {
                sb.Append(infos[i].ToString() + "\n");
            }

            text.text = sb.ToString();
        }
    }
}

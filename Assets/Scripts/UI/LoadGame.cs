using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Miren.UnityToolbag;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Miren
{
    public class LoadGame : MonoBehaviour
    {
        private static readonly string SearchFilter = "*" + GameMap.SaveFileExtension;

        [SerializeField]
        private GameMap map;

        [SerializeField]
        private RectTransform parent;

        [SerializeField]
        private FileData textPrefab;

        [SerializeField]
        private ToggleGroup toggleGroup;

        [SerializeField]
        private Text selectionErrorText;

        [SerializeField]
        private Text errorText;

        [SerializeField]
        private InputField inputField;

        private FileData selected;

        [SerializeField]
        private Button loadButton;

        private List<FileData> instances;

        private HashSet<FileInfo> loadedFiles;

        private string nameField;

        [SerializeField]
        private UnityEvent onSavePrompt;

        [SerializeField]
        private UnityEvent onFailLoad;

        private void Awake()
        {
            instances = new List<FileData>();
            loadedFiles = new HashSet<FileInfo>();
            loadButton.onClick.AddListener(LoadSelected);
        }

        private void OnEnable()
        {
            string path = StandardPaths.saveDataDirectory;
            DirectoryInfo dir = new DirectoryInfo(path);

            FileInfo[] infos = dir.GetFiles(SearchFilter, SearchOption.AllDirectories);

            if (infos.Length == 0)
            {
                loadButton.interactable = false;
                selectionErrorText.text = "No save files found, please start a new game from the main menu.";
            }

            if (infos.Length == loadedFiles.Count) return;
            HashSet<FileInfo> hashSet = new HashSet<FileInfo>(infos);
            if (hashSet.SetEquals(loadedFiles)) return;

            for (int i = 0; i < instances.Count; i++)
            {
                FileData data = instances[i];
                toggleGroup.UnregisterToggle(data.toggle);
                Destroy(data.gameObject);
            }

            instances.Clear();

            for (int i = 0; i < infos.Length; i++)
            {
                FileData data = Instantiate(textPrefab, parent);
                toggleGroup.RegisterToggle(data.toggle);
                data.toggle.group = toggleGroup;

                instances.Add(data);

                data.SetInfo(infos[i]);
            }

            loadedFiles = hashSet;
        }

        private void Update()
        {
            Toggle active = toggleGroup.ActiveToggles().FirstOrDefault(x => x.isOn);
            if (active == null)
            {
                selectionErrorText.text = "Please Select at least one save file.";
                return;
            }
            else
            {
                selectionErrorText.text = "";
            }

            FileData data = active.GetComponent<FileData>();

            if (data == selected) return;
            selected = data;

            inputField.text = Path.GetFileNameWithoutExtension(selected.Info.Name);

            loadButton.interactable = true;
        }

        public void LoadSelected()
        {
            string o = map.Load(selected);
            errorText.text = "";
            if (o == null) return;

            map.Clear();
            errorText.text = o;
            onFailLoad.Invoke();
        }

        public void DeleteSelected()
        {
            selected.Info.Delete();
            OnEnable();
        }

        public void SetNameField(string name)
        {
            nameField = name;
        }

        public void SaveSelected()
        {
            if (Contains(nameField))
            {
                onSavePrompt.Invoke();
                return;
            }

            SaveOverride();
        }

        public void SaveOverride()
        {
            FileInfo info = GameMap.GetSavePath(nameField + GameMap.SaveFileExtension);
            map.Save(info);

            OnEnable();
        }

        private bool Contains(string nameField)
        {
            bool contains = false;
            foreach (FileInfo fs in loadedFiles)
            {
                if (Path.GetFileNameWithoutExtension(fs.Name) == nameField)
                {
                    contains = true;
                }
            }

            return contains;
        }
    }
}

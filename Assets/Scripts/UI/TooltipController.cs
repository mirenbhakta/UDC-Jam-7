using UnityEngine;
using UnityEngine.UI;

namespace Miren
{
    public class TooltipController : MonoBehaviour
    {
        [SerializeField]
        private Text label;

        [SerializeField]
        private Text info;

        private ItemObjectBase current;

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            current = null;
            gameObject.SetActive(false);
        }

        public void ShowTooltip(ItemObjectBase obj)
        {
            if (current == obj) return;

            switch (obj)
            {
                case MapResourceObject resource:

                    ShowResourceTooltip(resource);
                    break;
                case FactoryObject factory:
                    ShowFactoryTooltip(factory);
                    break;

                default:
                    return;
            }
        }

        private void ShowResourceTooltip(MapResourceObject obj)
        {
            label.text = obj.Item.Name;
            info.text = obj.Count.ToString();
        }

        private void ShowFactoryTooltip(FactoryObject obj)
        {
            label.text = obj.Item.Name;

        }
    }
}

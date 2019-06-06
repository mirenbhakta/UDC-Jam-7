using UnityEngine;

namespace Miren
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private FactoryObject factoryPrefab;

        [SerializeField]
        private GameMap gameMap;

        [SerializeField]
        private Camera mainCam;

        [SerializeField]
        private Transform rayCaster;

        [SerializeField]
        private LayerMask tooltipMask;

        [SerializeField]
        private TooltipController tooltip;

        private Quaternion rotationTransform;
        private RaycastHit2D[] cache;

        private void Awake()
        {
            rotationTransform = Quaternion.Euler(-90, 0, 0);
            cache = new RaycastHit2D[32];
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            Vector3 mousePosition = Input.mousePosition;

            Ray r = mainCam.ScreenPointToRay(mousePosition);
            r.origin = rayCaster.position;
            r.direction = rotationTransform * r.direction;

            int count = Physics2D.GetRayIntersectionNonAlloc(r, cache, 1000f, tooltipMask);
            if (count > 0)
            {
                ItemObjectBase newObj = cache[0].transform.GetComponent<ItemObjectBase>();

                tooltip.Enable();
                tooltip.ShowTooltip(newObj);
            }
            else
            {
                tooltip.Disable();
            }
        }

        public static Vector3 GetColliderPosition(Vector3 vector)
        {
            return new Vector3(vector.x, vector.z + vector.y, 0);
        }
    }
}

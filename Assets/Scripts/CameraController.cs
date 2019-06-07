using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Miren
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform cameraTransform;

        [SerializeField]
        private GameMap map;

        [SerializeField]
        private float minZoom, maxZoom;

        [SerializeField]
        private float minZoomSpeed, maxZoomSpeed;

        [SerializeField]
        private float zoomTime;

        [SerializeField]
        private float zoomSpeed;

        private float zoomTarget, zoomVelocity;

        private float moveSpeed;

        private Vector3 min, max;

        [SerializeField]
        private Transform rayCasterPivot, rayCaster;

        public void Init()
        {
            TerrainData data = map.terrainGenerator.terrain.terrainData;
            Vector3 startPosition = new Vector3(0, data.size.y / 2f, 0);
            Init(startPosition, data);
        }

        public void Init(Vector3 startPosition)
        {
            TerrainData data = map.terrainGenerator.terrain.terrainData;
            Init(startPosition, data);
        }

        public void Init(Vector3 startPosition, TerrainData data)
        {
            rayCasterPivot.position = startPosition;
            transform.position = startPosition;

            Vector3 size = data.size * 0.5f;
            Vector3 offset = new Vector3();
            max = size - offset;
            min = -max;
            zoomTarget = Mathf.Lerp(maxZoom, minZoom, 0.5f);
        }

        private Vector3 Clamp(Vector3 pos)
        {
            pos.x = Mathf.Clamp(pos.x, min.x, max.x);
            pos.z = Mathf.Clamp(pos.z, min.z, max.z);
            return pos;
        }

        private void LateUpdate()
        {
            // rotation
            if (Input.GetKey(KeyCode.Mouse1))
            {
                float hDelta = Input.GetAxis("Mouse X");

                transform.Rotate(0, hDelta, 0, Space.World);
            }

            // position
            float speed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed *= 2;
            }

            Vector3 pos = transform.position;

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Quaternion rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

            Vector3 target = new Vector3(x, 0, y);
            if (target.sqrMagnitude > 1)
            {
                target.Normalize();
            }

            pos += rotation * target * speed * Time.deltaTime;

            pos = Clamp(pos);
            transform.position = pos;

            // scroll
            float scroll = Input.mouseScrollDelta.y * zoomSpeed;
            zoomTarget = Mathf.Clamp(zoomTarget + scroll, minZoom, maxZoom);

            float zoomT = Mathf.InverseLerp(maxZoom, minZoom, zoomTarget);
            moveSpeed = Mathf.Lerp(minZoomSpeed, maxZoomSpeed, zoomT);

            Vector3 cameraPos = cameraTransform.localPosition;
            cameraPos.z = Mathf.SmoothDamp(cameraPos.z, zoomTarget, ref zoomVelocity, zoomTime);
            cameraTransform.localPosition = cameraPos;

            rayCasterPivot.localPosition = pos;
            rayCasterPivot.localRotation = transform.localRotation;
            rayCaster.localPosition = cameraPos;
        }

        public void Pause()
        {
            enabled = false;
        }

        public void Resume()
        {
            enabled = true;
        }

        public void Save(BinaryWriter writer)
        {
            Vector3 position = transform.position;
            writer.Write(position.x);
            writer.Write(position.y);
            writer.Write(position.z);

            float angle = transform.localEulerAngles.y;
            writer.Write(angle);
        }

        public void Load(BinaryReader reader)
        {
            Vector3 position;
            position.x = reader.ReadSingle();
            position.y = reader.ReadSingle();
            position.z = reader.ReadSingle();

            float angle = reader.ReadSingle();

            Vector3 euler = transform.localEulerAngles;
            euler.y = angle;
            transform.localEulerAngles = euler;

            Init(position);
        }
    }
}

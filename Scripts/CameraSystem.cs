using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace RPG.CameraSystem
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private bool useEdgeScrolling = false;
        [SerializeField] private bool dragPanMoveActive = false;
        [SerializeField] private bool rotateActive = false;
        [SerializeField] private float fieldOfViewMax = 50;
        [SerializeField] private float fieldOfViewMin = 10;
        [SerializeField] private float followOffSetMin = 5f;
        [SerializeField] private float followOffSetMax = 50f;

        private Vector2 lastMousePosition;
        private float targetFieldOfView = 50;
        private Vector3 followOffset;
        private int cameraSetting = 0;

        private void Awake()
        {
            followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CameraMode();
                
        }

        private void CameraMode()
        {
            switch (cameraSetting)
            {
                case 0:
                    FreeLook();
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        private void FreeLook()
        {
            HandleCameraMovement();
            HandleCameraRotation();
            //HandleCameraZoom_FieldOfView();
            HandleCameraZoom_MovrForward();
        }

        private void HandleCameraZoom_MovrForward()
        {
            Vector3 zoomDir = followOffset.normalized;
            if (Input.mouseScrollDelta.y > 0)
            {
                followOffset -= zoomDir;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                followOffset += zoomDir;
            }
            if (followOffset.magnitude < followOffSetMin)
            {
                followOffset = zoomDir * followOffSetMin;
            }
            if (followOffset.magnitude > followOffSetMax)
            {
                followOffset = zoomDir * followOffSetMax;
            }

            float ZoomSpeed = 50f;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * ZoomSpeed);

        }

        private void HandleCameraZoom_FieldOfView()
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                targetFieldOfView -= 5;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                targetFieldOfView += 5;
            }

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

            float zoomSpeed = 10f;
            cinemachineVirtualCamera.m_Lens.FieldOfView =
                Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
        }

        private void HandleCameraMovement()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
            if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
            if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
            if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

            if (useEdgeScrolling)
            {
                EdgeScrolling();
            }

            HandleCameraMovementDragPen();

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 20f;

            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraMovementDragPen()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);
            if (Input.GetMouseButtonDown(2))
            {
                dragPanMoveActive = true;
                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(2))
            {
                dragPanMoveActive = false;
            }
            if (dragPanMoveActive)
            {
                Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

                float dragPanSpeed = 0.2f;
                inputDir.x = mouseMovementDelta.x * dragPanSpeed;
                inputDir.z = mouseMovementDelta.y * dragPanSpeed;
                lastMousePosition = Input.mousePosition;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 20f;

            transform.position -= moveDir * moveSpeed * Time.deltaTime;
        }
        private void EdgeScrolling()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);
            if (useEdgeScrolling)
            {
                int edgeScrollSize = 20;

                if (Input.mousePosition.x < edgeScrollSize)
                {
                    inputDir.x = -1f;
                }
                if (Input.mousePosition.y < edgeScrollSize)
                {
                    inputDir.z = -1f;
                }

                if (Input.mousePosition.x > Screen.width - edgeScrollSize)
                {
                    inputDir.x = +1f;
                }

                if (Input.mousePosition.y > Screen.height - edgeScrollSize)
                {
                    inputDir.z = +1f;
                }
            }
            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 20f;

            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }


        private void HandleCameraRotation()
        {
            /*
            float rotateDir = 0f;
            if (Input.GetKey(KeyCode.Q)) rotateDir = -1f;
            if (Input.GetKey(KeyCode.E)) rotateDir = +1f;

            float rotateSpeed = 100f;
            transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
            */
            Vector3 inputDir = new Vector3(0, 0, 0);
            float rotateDir = 0f;
            if (Input.GetMouseButtonDown(1))
            {
                rotateActive = true;
                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1))
            {
                rotateActive = false;
            }
            if (rotateActive)
            {
                Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;
                Vector2 mousePos = Input.mousePosition;

                if (lastMousePosition.x > mousePos.x)
                {
                    rotateDir = -1f;
                }
                if (lastMousePosition.x < mousePos.x)
                {
                    rotateDir = +1f;
                }

                lastMousePosition = Input.mousePosition;

                float rotateSpeed = 140f;

                transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
            }
            /*
            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 20f;

            transform.position -= moveDir * moveSpeed * Time.deltaTime;
            */

        }
    }
}

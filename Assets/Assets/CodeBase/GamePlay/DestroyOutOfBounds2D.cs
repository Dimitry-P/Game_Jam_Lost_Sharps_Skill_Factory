using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    using UnityEngine;

    public class DestroyOutOfBounds2D : MonoBehaviour
    {
        private Camera mainCamera;
        private float offscreenMargin = 2f;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

            if (viewportPos.x < -offscreenMargin || viewportPos.x > 1 + offscreenMargin ||
                viewportPos.y < -offscreenMargin || viewportPos.y > 1 + offscreenMargin)
            {
                Destroy(gameObject);
            }
        }
    }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class LaserShake : MonoBehaviour
    {
        public float shakeAmount = 0.05f;     // Насколько сильно трясётся
        public float shakeSpeed = 30f;        // Скорость тряски

        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.localPosition;
        }

        private void Update()
        {
            float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f;
            float offsetY = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f;

            Vector3 offset = new Vector3(offsetX, offsetY, 0f) * shakeAmount;
            transform.localPosition = initialPosition + offset;
        }
    }
}




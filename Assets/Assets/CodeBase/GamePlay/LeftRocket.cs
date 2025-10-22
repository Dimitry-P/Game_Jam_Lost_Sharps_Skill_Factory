using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace SpaceShooter
{
    public class LeftRocket : MonoBehaviour
    {
        [SerializeField] private GameObject rocketPrefab;
        [SerializeField] private Transform firePoint;

        public void Fire()
        {
            GameObject rocket = Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class RightRocket : MonoBehaviour
    {
        [SerializeField] private GameObject rocketPrefab;
        [SerializeField] private Transform firePoint_RightRocket;

        public void Fire()
        {
            GameObject rocket = Instantiate(rocketPrefab, firePoint_RightRocket.position, firePoint_RightRocket.rotation);

        }
    }
}

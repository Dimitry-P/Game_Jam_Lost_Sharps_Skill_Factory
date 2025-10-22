using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    public class Enemy_Ship : MonoBehaviour
    {
        public int health = 3;
        public bool isVulnerable = false;

        public void ApplyDamage(int damage)
        {
            if (!isVulnerable) return;

            health -= damage;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void SetVulnerability(bool state)
        {
            isVulnerable = state;
        }
    }
}


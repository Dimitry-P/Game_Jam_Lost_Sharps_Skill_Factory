using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceShooter
{
 
    public class GravityZone : MonoBehaviour
    {
        [SerializeField] private float m_Force;
        [SerializeField] private float m_Radius;
        private Projectile projectile;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.attachedRigidbody == null || collision.GetComponent<Projectile>()) return;
            var rb = collision.gameObject;
            if (rb.GetComponent<NoGravityFlag>() != null) return;

            Vector2 dir = transform.position - collision.transform.position;

            float dist = dir.magnitude;

            if (dist < m_Radius)
            {
                Vector2 force = dir.normalized * m_Force * (dist / m_Radius);
                collision.attachedRigidbody.AddForce(force, ForceMode2D.Force);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            GetComponent<CircleCollider2D>().radius = m_Radius;
        }
    }
#endif
}


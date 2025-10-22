using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;


namespace Common
{
    public abstract class ProjectileBase : Entity
    {
        public float m_Velocity;
        [SerializeField] private float m_Lifetime;
        [SerializeField] private int m_Damage;
    
        protected Destructible m_Parent;

        protected virtual void OnHit(Destructible destructible) { }
        protected virtual void OnHit(Collider2D collider2D) { }
        protected virtual void OnProjectileLifeEnd(Collider2D col, Vector2 pos) { }

        private float m_Timer;

        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            if (hit)
            {
                OnHit(hit.collider);
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();

                if (dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage);

                    OnHit(dest);
                }
                OnProjectileLifeEnd(hit.collider, hit.point);
            }


            m_Timer += Time.deltaTime;

            if(m_Timer > m_Lifetime) OnProjectileLifeEnd(hit.collider, hit.point);

            // === ”ничтожение за границами камеры ===
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
            {
                Destroy(gameObject);
            }

            transform.position += new Vector3(step.x, step.y, 0);
        }

        

        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;  
        }
    }
}


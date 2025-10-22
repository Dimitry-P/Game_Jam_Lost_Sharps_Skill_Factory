using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Common;

namespace SpaceShooter
{
    public class CollisionDamageApplicator : MonoBehaviour
    {
       

        [SerializeField] private float m_DamageConstant;
        [SerializeField] private float m_VelocityDamageModifier;


        public void OnCollisionEnter2D(Collision2D collision)
        {
           
                Debug.Log("Корабль столкнулся с: " + collision.gameObject.name);
                // остальной код
            
            var tagOfSmallMeteor = collision.gameObject.tag;
            if (tagOfSmallMeteor != "Meteor_Small" && tagOfSmallMeteor != "Vulnerable")
            {
                var destructible = transform.root.GetComponent<Destructible>();
                if (destructible != null)
                {

                    destructible.ApplyDamage((int)m_DamageConstant + (int)(m_VelocityDamageModifier * collision.relativeVelocity.magnitude));
                }
            }
        }
    }
}


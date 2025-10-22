using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
   
        public class HomingRocket : MonoBehaviour
        {
            public float speed = 5f;
            public float rotateSpeed = 200f;

        private Transform target;

            void Start()
            {
                FindClosestTarget();
                Destroy(gameObject, 10f);
           


        }

            void Update()
            {
                if (target == null)
                {
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                    return;
                }

                Vector2 direction = (target.position - transform.position).normalized;
                float angle = Vector3.SignedAngle(transform.up, direction, Vector3.forward);

                transform.Rotate(0, 0, angle * rotateSpeed * Time.deltaTime);

                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }

            void FindClosestTarget()
            {
                GameObject[] meteors = GameObject.FindGameObjectsWithTag("Meteor");
                float minDistance = Mathf.Infinity;

                foreach (var meteor in meteors)
                {
                    float dist = Vector2.Distance(transform.position, meteor.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        target = meteor.transform;
                    }
                }
            }

           
        
    }
}







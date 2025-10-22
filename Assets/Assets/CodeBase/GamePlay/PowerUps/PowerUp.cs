using UnityEngine;
using System.Collections.Generic;
using System.Collections;


namespace SpaceShooter
{
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class PowerUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpaceShip ship = collision.transform.root.GetComponent<SpaceShip>();

            if (ship != null  && Player.Instance.ActiveShip != null)
            {
                OnPickedUp(ship);

                Destroy(gameObject);    
            }
        }

        protected abstract void OnPickedUp(SpaceShip ship);
    }
}


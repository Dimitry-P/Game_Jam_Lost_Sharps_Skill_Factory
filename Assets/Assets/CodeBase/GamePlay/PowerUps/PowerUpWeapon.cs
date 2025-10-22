using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SpaceShooter
{
    public class PowerUpWeapon : PowerUp
    {
        [SerializeField] private TurretProperties m_Properties;

        protected override void OnPickedUp(SpaceShip ship)
        {
            ship.AssignedWeapon(m_Properties);
        }
    }
}


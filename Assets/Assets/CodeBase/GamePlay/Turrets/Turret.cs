using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;


namespace SpaceShooter
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        public SpaceShip m_Ship;
        private Vector3 velocity;

        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if(m_RefireTimer > 0)
            m_RefireTimer -= Time.deltaTime;    
        }

        //Public API -- Метод стрельбы
        public void Fire()
        {
            Debug.Log("Turret.Fire вызван для " + name);
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false) return;
            //У корабля вызываем метод DrawEnergy, т.е. отнять энергию. 
            // И отнимаем энергии столько, сколько у нас указано в нашем SO в Properties.
            // Здесь в любом случае этот метод: DrawEnergy(m_TurretProperties.EnergyUsage) выполнится,
            // и у нас отнимутся патроны. Но если метод вернёт false, то мы это проверим
            // и выйдем из метода, А последующий код не будет выполняться,
            // т.е. мы не создадим projectile.

            if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false) return;


           


            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab)?.GetComponent<Projectile>();
            //Т.о. в переменной projectile будет храниться только что созданный компонент префаба ProjectilePrefab
            if (projectile == null)
            {
                Debug.LogError("Projectile is null! Возможно, префаб не содержит компонент Projectile.");
                return;
            }

            // Отодвигаем снаряд немного вперёд, чтобы избежать коллизии с кораблём
            Vector3 spawnOffset = transform.up * 0.5f; // 0.5 единиц вперёд по направлению дула
            projectile.transform.position = transform.position + spawnOffset;
            projectile.transform.up = transform.up;

           
            projectile.SetParentShooter(m_Ship);

           m_RefireTimer = m_TurretProperties.RateOfFire;

            {
                //SFX
            }
        }


        public void AssignedLoadOut(TurretProperties properties)
        {
            if (m_Mode != properties.Mode) return;
            // Если мы хотим положить св-во для главного оружия во вторичную пушку, то так делать нельзя.

            m_RefireTimer = 0;
            m_TurretProperties = properties;    
        }
    }
}


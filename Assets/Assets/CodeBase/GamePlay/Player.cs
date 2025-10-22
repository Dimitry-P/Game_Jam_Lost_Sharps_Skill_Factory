using System.Collections;
using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        public static SpaceShip SelectedSpaceShip;

        [SerializeField] private int m_NumLives;
       
        [SerializeField] private SpaceShip m_PlayerShipPrefab;
        [SerializeField] private CameraController m_CameraController;
        [SerializeField] private MovementController m_MovementController;
        [SerializeField] private GameObject m_ExplosionPrefab;
        [SerializeField] private AudioClip explosionClip;

        private SpaceShip m_Ship;

        private int m_Score;
        private int m_NumKills;

        public int Score => m_Score;
        public int NumKills => m_NumKills;
        public int NumLives => m_NumLives;

        public SpaceShip ShipPrefab
        {
            get
            {
                if(SelectedSpaceShip == null)
                {
                    return m_PlayerShipPrefab;
                }
                else
                {
                    return SelectedSpaceShip;
                }
            }
        }

        public SpaceShip ActiveShip => m_Ship;

        private void Start()
        {
            Debug.Log("Player.Start вызван");
            m_MovementController.SetTargetShip(m_Ship);
            Respawn();
        }

        private void OnShipDeath()
        {
            m_NumLives--;
            Explosion();
           
            if (m_NumLives > 0)
            {
                StartCoroutine(RespawnWithDelay(2f));
            }
            else
            {
                // Игрок умер окончательно — можно добавить логику Game Over
                Debug.Log("Game Over");
            }
        }

        private IEnumerator RespawnWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Respawn();
        }

        private void Explosion()
        {
            if (m_ExplosionPrefab != null && m_Ship != null)
            {
                if (explosionClip != null)
                {
                    AudioSource.PlayClipAtPoint(explosionClip, m_Ship.transform.position, 1f);
                }
                GameObject explosion = Instantiate(m_ExplosionPrefab, m_Ship.transform.position, Quaternion.identity);
                ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
                if (ps != null)
                    ps.Play();
                
            }
        }

       

        private void Respawn()
        {
            Debug.Log("RespawnShip: начало");

            DestroyAllFragments();

            if (m_Ship != null)
            {
                Debug.Log("RespawnShip: уничтожен старый корабль");
                m_Ship.EventOnDeath.RemoveListener(OnShipDeath);
            }
               

            var newShipGO = Instantiate(ShipPrefab);
            Debug.Log("RespawnShip: создан новый корабль");

            newShipGO.tag = "Player"; // обязательно!

            Debug.Log("Новый корабль создан с тегом: " + newShipGO.tag);


            m_Ship = newShipGO.GetComponent<SpaceShip>();

            if (m_Ship == null)
            {
                Debug.LogError("Respawn: не удалось получить SpaceShip из newShipGO!");
            }

            m_Ship.EventOnDeath.AddListener(OnShipDeath);
    
            m_CameraController.SetTarget(m_Ship.transform);
            m_MovementController.SetTargetShip(m_Ship);

            // Передаём ссылку на новый корабль в Meteor
            if (Meteor.Instance != null)
            {
                Meteor.Instance.RespawnMeteors();
                Meteor.Instance.SetPlayer(m_Ship);
            }

                

            //RightRocket rl = newShipGO.GetComponent<RightRocket>();
            //rl.firePoint = newShipGO.transform.Find("FirePoint");
            //Debug.Log("RespawnShip: найден FirePoint");
            //rl.rocketPrefab = rocketPrefab;
            //Debug.Log("RespawnShip: установлен rocketPrefab");
            //rl.SpawnNewRocket();
            //Debug.Log("RespawnShip: вызван SpawnNewRocket");
        }

        public void DestroyAllFragments()
        {
            GameObject[] fragments = GameObject.FindGameObjectsWithTag("Meteor_Small");
            foreach (GameObject fragment in fragments)
            {
                Destroy(fragment);
            }
        }
        public void AddKill()
        {
            m_NumKills += 1;
        }

        public void AddScore(int num)
        {
            m_Score += num;
        }
    }
}











//private void Respawn()
//{
//    if (m_Ship != null)
//    {
//        Destroy(m_Ship.gameObject);
//    }

//    var spawnPosition = new Vector3(0, 0, 0); 
//    var newPlayerShip = Instantiate(m_PlayerShipPrefab.gameObject, spawnPosition, Quaternion.identity);
//    m_NumLives = 40;

//    Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);

//    m_Ship = newPlayerShip.GetComponent<SpaceShip>();

//    m_Ship.EventOnDeath.AddListener(OnShipDeath);

//    m_CameraController.SetTarget(m_Ship.transform);
//    m_CameraController.SetTargetCollisionEdge(m_Ship.transform);

//    m_MovementController.SetTargetShip(m_Ship);
//    m_MovementController.SetTargetShipCollisionEdge(m_Ship);
//}
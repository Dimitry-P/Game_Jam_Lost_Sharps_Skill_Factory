using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace SpaceShooter
{
    public class Meteor : MonoBehaviour
    {
        public static Meteor Instance;

        [SerializeField] private GameObject m_MeteorPrefab;
        //[SerializeField] private int meteorCount = 4;
        [SerializeField] private float spawnInterval = 4f;
        [SerializeField] private SpaceShip player;
        [SerializeField] private SpaceShip playerPrefab;
      
 
       
        private float spawnTimer = 0f;
        private float spawnX = 6f;
        private float spawnY = 10f;
      


        private void Awake()
        {
            Instance = this;
        }

       

        public void SetPlayer(SpaceShip ship)
        {
            if (ship == null)
            {
                Debug.LogWarning("SetPlayer получил null");
                return;
            }
            player = ship; // <-- ЭТО ГЛАВНОЕ!
            Debug.Log("SetPlayer вызван с кораблём: " + ship.name);
           
        }

      


        


        void Update()
        {
            if (player == null)
            {
                Debug.Log(" Meteor.Update: player всё ещё null");
                return;
            }
           
            
                spawnTimer += Time.deltaTime;
                if (spawnTimer >= spawnInterval)
                {
                    spawnTimer = 0f;
                    Debug.Log(" Спавним метеор");
                    SpawnMeteors();
                }
        
        }
        

        private void SpawnMeteors()
        {
            if (player == null)
                return; // Если игрок уничтожен, не спавним метеоры
            float widthX = UnityEngine.Random.Range(-spawnX, spawnX);
            var meteor = Instantiate(m_MeteorPrefab, new Vector3(widthX, UnityEngine.Random.Range(player.transform.position.y + spawnY, player.transform.position.y * 2 + spawnY), 0f), Quaternion.identity);
            Destroy(meteor, 50f);
           
        }

        public void RespawnMeteors()
        {
            Debug.Log("Meteor.RespawnMeteors вызван");
            GameObject[] oldMeteors = GameObject.FindGameObjectsWithTag("Meteor");
            foreach (GameObject meteor in oldMeteors)
            {
                Destroy(meteor);
            }

        }
       
    }
}




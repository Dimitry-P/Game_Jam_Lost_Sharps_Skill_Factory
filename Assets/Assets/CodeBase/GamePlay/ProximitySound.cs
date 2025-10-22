using System.Collections;
using UnityEngine;


namespace SpaceShooter
{
    public class ProximitySound : MonoBehaviour
    {
        public Transform player;
        private AudioSource audioSource;

        public float maxDistance = 20f;
        public float minDistance = 1f;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Stop();

            StartCoroutine(FindPlayerCoroutine());
        }

        private IEnumerator FindPlayerCoroutine()
        {
            int attempt = 0;

            while (player == null && attempt < 300)
            {
                GameObject found = GameObject.FindGameObjectWithTag("Player");

                if (found != null)
                {
                    player = found.transform;
                    Debug.Log(" Игрок найден с задержкой: " + player.name);
                    break;
                }
                else
                {
                    Debug.Log(" Попытка найти игрока #" + attempt);
                }

                attempt++;
                yield return new WaitForSeconds(0.1f);
            }

            if (player == null)
            {
                Debug.LogWarning(" Игрок так и не найден после 30 секунд!");
            }
        }




        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(" Trigger entered: " + collision.name);
            if (collision.CompareTag("Player"))
            {
                Debug.Log(" Игрок вошёл в зону!");
                audioSource.Play();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log(" Игрок вышел из зоны!");
                audioSource.Stop();
            }
        }

        private void Update()
        {
            if (player == null) return;

            float distance = Vector3.Distance(transform.position, player.position);
            float volume = 1 - Mathf.InverseLerp(maxDistance, minDistance, distance);
            audioSource.volume = Mathf.Clamp01(volume);
        }
    }

}

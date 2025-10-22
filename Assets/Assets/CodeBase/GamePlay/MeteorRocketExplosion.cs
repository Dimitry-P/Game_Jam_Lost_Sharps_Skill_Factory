using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;


namespace SpaceShooter
{
    using UnityEngine;

    public class MeteorRocketExplosion : MonoBehaviour
    {
        public AudioSource flightSound;
        public AudioSource explosionSound;
        private bool meteorIsDestroyed = false;
        [SerializeField] private GameObject explosionEffectPrefab;




        private void Start()
        {
            Destroy(gameObject, 10f); // на всякий случай автоудаление
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Meteor") && meteorIsDestroyed == false)
            {
                if (explosionEffectPrefab != null)
                {
                    Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                }
                meteorIsDestroyed = true;
                flightSound?.Stop();
                if (explosionSound != null)
                {
                    explosionSound.transform.parent = null;
                    explosionSound.Play();

                    // Уничтожить объект со звуком взрыва после проигрывания
                    Destroy(explosionSound.gameObject, explosionSound.clip.length);
                }
                Destructible meteorScript = other.GetComponent<Destructible>();
                if (meteorScript != null)
                {
                        meteorScript.SpawnFragments(2.0f);
                }

                //  ВЗРЫВ ПО РАДИУСУ
                Vector2 explosionCenter = transform.position;
                float explosionRadius = 5f; // подбери радиус под свой масштаб

                Collider2D[] hitObjects = Physics2D.OverlapCircleAll(explosionCenter, explosionRadius);

                foreach (Collider2D hit in hitObjects)
                {
                    Destructible destructible = hit.GetComponent<Destructible>();
                    if (destructible != null)
                    {
                        destructible.ApplyDamage(999); // наносим урон всем (можно сделать переменной)
                    }

                    // Добавим силу взрыва, если есть Rigidbody
                    Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 direction = rb.position - explosionCenter;
                        rb.AddForce(direction.normalized * 5f, ForceMode2D.Impulse);
                    }
                }
                Destroy(other.gameObject);
                Destroy(gameObject);

              
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 5f); // радиус взрыва
        }

    }
}



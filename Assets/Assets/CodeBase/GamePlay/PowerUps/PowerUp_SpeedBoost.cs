using UnityEngine;

namespace SpaceShooter
{
    public class PowerUp_SpeedBoost : MonoBehaviour
    {
        private float timer = 0f;
        private bool bonusWasTaken = false;

        private float boostedThrust = 20000f;
        private float originalThrust;

        private SpaceShip m_TargetShip;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                m_TargetShip = other.GetComponent<SpaceShip>();
                if (m_TargetShip == null)
                {
                    Debug.LogWarning(" Не найден компонент SpaceShip");
                    return;
                }

                // Сохраняем оригинальную скорость
                originalThrust = m_TargetShip.GetThrust();

                // Увеличиваем скорость
                m_TargetShip.SetThrust(boostedThrust);

                Debug.Log(" Скорость увеличена!");

                bonusWasTaken = true;

                // Прячем объект бонуса
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
            }
        }

        private void Update()
        {
            if (!bonusWasTaken || m_TargetShip == null) return;

            timer += Time.deltaTime;

            if (timer >= 10f)
            {
                m_TargetShip.SetThrust(originalThrust);
                Debug.Log(" Скорость вернулась к норме");

                Destroy(gameObject); // полностью удалим бонус
            }
        }
    }
}

using UnityEngine;

namespace SpaceShooter
{
    public class PowerUp_DisableLaser : MonoBehaviour
    {
        [SerializeField] private GameObject laserObject; // Ссылка на лазер

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (laserObject != null)
                {
                    laserObject.SetActive(false); // Выключаем лазер
                    Debug.Log(" Лазер отключён после подбора бонуса");
                }

                Destroy(gameObject); // Удаляем бонус
            }
        }
    }
}

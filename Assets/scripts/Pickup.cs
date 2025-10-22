using UnityEngine;
using SimpleFPS;
using UnityEngine.Events;

namespace Echoes_At_The_Last_Station
{
    public class Pickup : MonoBehaviour
    {
        public UnityEvent PlusScore;

        private bool playerInZone = false;
        private bool pickedUp = false;

        [SerializeField] private AddScore addScore;

        private void Update()
        {
            if (playerInZone && !pickedUp && Input.GetKeyDown(KeyCode.E))
            {
                pickedUp = true;

                PlusScore?.Invoke();
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<FirstPersonController>() != null)
            {
                playerInZone = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<FirstPersonController>() != null)
            {
                playerInZone = false;
            }
        }
    }
}

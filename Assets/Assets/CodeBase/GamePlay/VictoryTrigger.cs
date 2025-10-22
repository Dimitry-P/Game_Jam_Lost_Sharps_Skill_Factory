using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private GameObject winMessage;


    private void Start()
    {
        winMessage.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(" ѕобеда!"); // проверь, по€вл€етс€ ли лог в консоли
            Time.timeScale = 0f; // заморозка игры
            if (winMessage != null)
            {
                winMessage.SetActive(true); // показать надпись
            }
            else
            {
                Debug.LogWarning("winMessage не назначен в инспекторе!");
            }
        }
    }
}

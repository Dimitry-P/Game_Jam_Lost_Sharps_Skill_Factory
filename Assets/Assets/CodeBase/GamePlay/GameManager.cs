using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryText;

    public void Victory()
    {
        victoryText.SetActive(true);
        Time.timeScale = 0f;
    }
}

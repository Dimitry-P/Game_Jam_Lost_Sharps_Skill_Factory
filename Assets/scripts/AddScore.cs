// Â AddScore.cs
using UnityEngine;
using UnityEngine.UI;

public class AddScore : MonoBehaviour
{
    public static AddScore Instance { get; private set; }

    private int score = 0;
    public int Score => score;
    [SerializeField] private Text scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlusOne()
    {
        score++;
        scoreText.text = "Score: " + score;
    }
}

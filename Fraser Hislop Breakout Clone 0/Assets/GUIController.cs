using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIController : MonoBehaviour
{
    // Singleton Instance
    private static GUIController _instance;
    public static GUIController Instance { get { return _instance; } }

    // UI Components
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI livesText;
    [SerializeField]
    private TextMeshProUGUI speedText;

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    public void SetScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void SetLivesText(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    public void SetSpeedText(float speed)
    {
        speedText.text = "Speed: " + speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Singleton Instance
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    // Caching
    private GUIController guiController;

    int score = 0;
    public int Score { get { return score; } }
    private int scorePerBrick = 100;

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;

    }

    private void Start()
    {
        guiController = GUIController.Instance;

        ResetScore();
    }

    public void IncreaseScore()
    {
        score += scorePerBrick;
        guiController.SetScoreText(score);
    }

    public void ResetScore()
    {
        score = 0;
        guiController.SetScoreText(score);
    }
}
